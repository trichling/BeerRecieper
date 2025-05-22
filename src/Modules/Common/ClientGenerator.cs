namespace Common;

using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using RestEase;

public static class ClientGenerator
{
    public static T For<T>(IServiceProvider services)
        where T : class
    {
        var sourceCode = ApiClientSourceTextGenerator.GenerateApiClient(typeof(T));
        var assembly = CompileAssembly(sourceCode, typeof(T));
        var type = assembly.GetType($"{typeof(T).Namespace}.{typeof(T).Name.Substring(1)}");

        var endpointInvoker = services.GetService<IEndpointInvoker>();

        return (T)Activator.CreateInstance(type, endpointInvoker);
    }

    private static Assembly CompileAssembly(string sourceCode, Type interfaceType)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(
            sourceCode,
            new CSharpParseOptions().WithLanguageVersion(LanguageVersion.Latest)
        );
        var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "mscorlib.dll")),
            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.dll")),
            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Core.dll")),
            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll")),
            MetadataReference.CreateFromFile(typeof(Assembly).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Task).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IEndpointInvoker).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(PathAttribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(QueryAttribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(BodyAttribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(RestClient).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Task).Assembly.Location),
            MetadataReference.CreateFromFile(interfaceType.Assembly.Location),
        };

        var compilation = CSharpCompilation.Create(
            assemblyName: Path.GetRandomFileName(),
            syntaxTrees: new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            var failures = result.Diagnostics.Where(diagnostic =>
                diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error
            );

            throw new Exception(
                $"Compilation failed: {string.Join(Environment.NewLine, failures)}"
            );
        }

        ms.Seek(0, SeekOrigin.Begin);
        return AssemblyLoadContext.Default.LoadFromStream(ms);
    }
}

public class ApiClientSourceTextGenerator
{
    public static string GenerateApiClient(Type interfaceType)
    {
        var interfaceName = interfaceType.Name;
        var className = interfaceName.StartsWith("I")
            ? interfaceName.Substring(1)
            : interfaceName + "Client";

        var sourceBuilder = new StringBuilder();
        sourceBuilder.AppendLine("using System;");
        sourceBuilder.AppendLine("using System.Collections.Generic;");
        sourceBuilder.AppendLine("using System.Threading.Tasks;");
        sourceBuilder.AppendLine("using Common;");
        sourceBuilder.AppendLine();

        // Add namespace if the interface has one
        var hasNamespace = !string.IsNullOrEmpty(interfaceType.Namespace);
        if (hasNamespace)
        {
            sourceBuilder.AppendLine($"namespace {interfaceType.Namespace};");
            sourceBuilder.AppendLine();
        }

        // Generate class declaration
        sourceBuilder.AppendLine($"public class {className} : {interfaceName}");
        sourceBuilder.AppendLine("{");

        // Add private field and constructor
        sourceBuilder.AppendLine("    private readonly IEndpointInvoker _endpointInvoker;");
        sourceBuilder.AppendLine();
        sourceBuilder.AppendLine($"    public {className}(IEndpointInvoker endpointInvoker)");
        sourceBuilder.AppendLine("    {");
        sourceBuilder.AppendLine("        _endpointInvoker = endpointInvoker;");
        sourceBuilder.AppendLine("    }");
        sourceBuilder.AppendLine();

        // Generate method implementations
        foreach (var method in interfaceType.GetMethods())
        {
            GenerateMethodImplementation(method, sourceBuilder);
            sourceBuilder.AppendLine();
        }

        sourceBuilder.AppendLine("}");

        return sourceBuilder.ToString();
    }

    private static void GenerateMethodImplementation(MethodInfo method, StringBuilder sourceBuilder)
    {
        var parameters = method.GetParameters();
        var parameterList = string.Join(
            ", ",
            parameters.Select(p => $"{p.ParameterType.Name} {p.Name}")
        );

        var interfaceType =
            method.DeclaringType
            ?? throw new InvalidOperationException("Method must have a declaring type");

        // Generate method signature
        sourceBuilder.AppendLine(
            $"    public async {GetTypeName(method.ReturnType)} {method.Name}({parameterList})"
        );
        sourceBuilder.AppendLine("    {");

        // Get method call info
        sourceBuilder.AppendLine(
            $"        var methodCallInfo = _endpointInvoker.GetMethodCallInfo<{interfaceType.Name}>(\"{method.Name}\");"
        );
        sourceBuilder.AppendLine();

        // Build route values dictionary if there are Path parameters
        var pathParams = parameters.Where(p =>
            p.GetCustomAttributes().Any(a => a.GetType().Name == "PathAttribute")
        );
        if (pathParams.Any())
        {
            sourceBuilder.AppendLine("        var routeValues = new Dictionary<string, object>()");
            sourceBuilder.AppendLine("        {");
            foreach (var param in pathParams)
            {
                sourceBuilder.AppendLine(
                     $"            {{ \"{param.Name}\", {param.Name}.ToString() }},"
                );
            }
            sourceBuilder.AppendLine("        };");
            sourceBuilder.AppendLine();
        }

        // Build query values dictionary if there are Query parameters
        var queryParams = parameters.Where(p =>
            p.GetCustomAttributes().Any(a => a.GetType().Name == "QueryAttribute")
        );
        if (queryParams.Any())
        {
            sourceBuilder.AppendLine("        var queryValues = new Dictionary<string, object>()");
            sourceBuilder.AppendLine("        {");
            foreach (var param in queryParams)
            {
                sourceBuilder.AppendLine(
                     $"            {{ \"{param.Name}\", {param.Name}.ToString() }},"
                );
            }
            sourceBuilder.AppendLine("        };");
            sourceBuilder.AppendLine();
        }

        // Find body parameter if any
        var bodyParam = parameters.FirstOrDefault(p =>
            p.GetCustomAttributes().Any(a => a.GetType().Name == "BodyAttribute")
        );

        // Generate the InvokeEndpointAsync call
        sourceBuilder.Append("        var result = await _endpointInvoker.InvokeEndpointAsync<");
        sourceBuilder.Append(GetTypeNameButSkipTask(method.ReturnType));
        sourceBuilder.AppendLine(">(");
        sourceBuilder.AppendLine("            methodCallInfo.HttpMethod,");
        sourceBuilder.AppendLine("            methodCallInfo.Path,");
        sourceBuilder.AppendLine($"            {(pathParams.Any() ? "routeValues" : "null")},");
        sourceBuilder.AppendLine($"            {(queryParams.Any() ? "queryValues" : "null")},");
        sourceBuilder.AppendLine($"            {(bodyParam != null ? bodyParam.Name : "null")}");
        sourceBuilder.AppendLine("        );");
        sourceBuilder.AppendLine();

        sourceBuilder.AppendLine("        return result;");
        sourceBuilder.AppendLine("    }");
    }

    private static string GetTypeName(Type type)
    {
        if (!type.IsGenericType)
            return type.Name;

        var genericArgs = string.Join(", ", type.GetGenericArguments().Select(t => GetTypeName(t)));
        var baseName = type.Name.Substring(0, type.Name.IndexOf('`'));
        return $"{baseName}<{genericArgs}>";
    }

    private static string GetTypeNameButSkipTask(Type type)
    {
        if (!type.IsGenericType)
            return type.Name;

        if (type.Name.StartsWith("Task"))
        {
            var genericArg = type.GetGenericArguments().FirstOrDefault();
            if (genericArg != null)
                return GetTypeName(genericArg);
        }

        return GetTypeName(type);
    }
}
