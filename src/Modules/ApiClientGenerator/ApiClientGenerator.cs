using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ApiClientGenerator;

[Generator(LanguageNames.CSharp)]
public class ApiClientGenerator : IIncrementalGenerator
{
    private int _fileCounter = 0;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Register a syntax receiver that will gather interface declarations
        var interfaceDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: (s, _) => s is InterfaceDeclarationSyntax { Identifier: { Text: var text } } && text.EndsWith("Api"),
                transform: (ctx, _) => (InterfaceDeclarationSyntax)ctx.Node)
            .Collect();

        // Register the source output
        context.RegisterSourceOutput(interfaceDeclarations, (spc, interfaceDecls) =>
        {
            foreach (var interfaceDecl in interfaceDecls)
            {
                var sourceText = GenerateApiClientSource(interfaceDecl);
                var uniqueCounter = Interlocked.Increment(ref _fileCounter);
                var fileName = $"{interfaceDecl.Identifier.Text}Client_{uniqueCounter}.g.cs";
                spc.AddSource(fileName, sourceText);
            }
        });
    }

    private static SourceText GenerateApiClientSource(InterfaceDeclarationSyntax interfaceDecl)
    {
        var namespaceName = GetNamespace(interfaceDecl);
        var interfaceName = interfaceDecl.Identifier.Text;
        var className = interfaceName.StartsWith("I") ? interfaceName.Substring(1) : interfaceName + "Client";

        var source = new StringBuilder();
        source.AppendLine("using System;");
        source.AppendLine("using System.Collections.Generic;");
        source.AppendLine("using System.Threading.Tasks;");
        source.AppendLine("using Common;");
        source.AppendLine("using RestEase;");
        source.AppendLine();

        if (!string.IsNullOrEmpty(namespaceName))
        {
            source.AppendLine($"namespace {namespaceName};");
            source.AppendLine();
        }

        source.AppendLine($"public class {className} : {interfaceName}");
        source.AppendLine("{");
        source.AppendLine("    private readonly IEndpointInvoker _endpointInvoker;");
        source.AppendLine();
        source.AppendLine($"    public {className}(IEndpointInvoker endpointInvoker)");
        source.AppendLine("    {");
        source.AppendLine("        _endpointInvoker = endpointInvoker;");
        source.AppendLine("    }");
        source.AppendLine();

        // Generate method implementations
        foreach (var methodDecl in interfaceDecl.Members.OfType<MethodDeclarationSyntax>())
        {
            GenerateMethodImplementation(methodDecl, source);
            source.AppendLine();
        }

        source.AppendLine("}");

        return SourceText.From(source.ToString(), Encoding.UTF8);
    }

    private static void GenerateMethodImplementation(MethodDeclarationSyntax methodDecl, StringBuilder source)
    {
        var methodName = methodDecl.Identifier.Text;
        var returnType = methodDecl.ReturnType.ToString();
        var parameters = methodDecl.ParameterList.Parameters;
        var parameterList = string.Join(", ", parameters.Select(p => $"{p.Type} {p.Identifier}"));

        source.AppendLine($"    public {returnType} {methodName}({parameterList})");
        source.AppendLine("    {");

        var interfaceIdentifier = ((InterfaceDeclarationSyntax)methodDecl.Parent).Identifier.Text;
        source.AppendLine($$"""        var methodCallInfo = _endpointInvoker.GetMethodCallInfo<{{interfaceIdentifier}}>("{{methodName}}");""");
        source.AppendLine();

        var pathParams = parameters.Where(p =>
            p.AttributeLists.SelectMany(al => al.Attributes)
             .Any(attr => attr.Name.ToString() == "Path"));

        if (pathParams.Any())
        {
            source.AppendLine("        var routeValues = new Dictionary<string, object>()");
            source.AppendLine("        {");
            foreach (var param in pathParams)
            {
                source.AppendLine($"            {{ \"{param.Identifier}\", {param.Identifier}.ToString() }},");
            }
            source.AppendLine("        };");
            source.AppendLine();
        }

        var queryParams = parameters.Where(p =>
            p.AttributeLists.SelectMany(al => al.Attributes)
             .Any(attr => attr.Name.ToString() == "Query"));

        if (queryParams.Any())
        {
            source.AppendLine("        var queryValues = new Dictionary<string, object>()");
            source.AppendLine("        {");
            foreach (var param in queryParams)
            {
                source.AppendLine($"            {{ \"{param.Identifier}\", {param.Identifier}.ToString() }},");
            }
            source.AppendLine("        };");
            source.AppendLine();
        }

        var bodyParam = parameters.FirstOrDefault(p =>
            p.AttributeLists.SelectMany(al => al.Attributes)
             .Any(attr => attr.Name.ToString() == "Body"));

        // Get the actual return type (skip Task<>)
        var returnTypeStr = returnType;
        if (returnType.StartsWith("Task<"))
        {
            returnTypeStr = returnType.Substring(5, returnType.Length - 6);
        }

        source.AppendLine($"        return _endpointInvoker.InvokeEndpointAsync<{returnTypeStr}>(");
        source.AppendLine("            methodCallInfo.HttpMethod,");
        source.AppendLine("            methodCallInfo.Path,");
        source.AppendLine($"            {(pathParams.Any() ? "routeValues" : "null")},");
        source.AppendLine($"            {(queryParams.Any() ? "queryValues" : "null")},");
        source.AppendLine($"            {(bodyParam != null ? bodyParam.Identifier.ToString() : "null")}");
        source.AppendLine("        );");
        source.AppendLine("    }");
    }

    private static string GetNamespace(InterfaceDeclarationSyntax interfaceDecl)
    {
        var parent = interfaceDecl.Parent;
        while (parent != null)
        {
            if (parent is BaseNamespaceDeclarationSyntax namespaceDecl)
            {
                return namespaceDecl.Name.ToString();
            }
            parent = parent.Parent;
        }
        return string.Empty;
    }
}