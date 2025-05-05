namespace Common;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RestEase;
using System.Reflection;
using System.Runtime.Loader;

public static class ClientGenerator
{
    public static T For<T>() where T : class
    {
        var sourceCode = GenerateImplementation<T>();
        var assembly = CompileAssembly(sourceCode, typeof(T));
        var type = assembly.GetType($"{typeof(T).Namespace}.{typeof(T).Name.Substring(1)}");

        return (T)Activator.CreateInstance(type, null);
    }

    private static string GenerateImplementation<T>()
    {
        var interfaceType = typeof(T);
        var className = interfaceType.Name.Substring(1); // Remove 'I' prefix
        var namespaceName = interfaceType.Namespace;

        var methods = new List<string>();
        foreach (var method in interfaceType.GetMethods())
        {
            var parameters = method.GetParameters();
            var parameterList = string.Join(", ", parameters.Select(p => $"{p.ParameterType.Name} {p.Name}"));
            var returnType = method.ReturnType.Name;

            var routeValues = string.Join(Environment.NewLine,
                parameters.Where(p => p.GetCustomAttribute<PathAttribute>() != null)
                         .Select(p => $"{{ \"{p.Name}\", {p.Name}.ToString() }},"));

            var queryValues = string.Join(Environment.NewLine,
                parameters.Where(p => p.GetCustomAttribute<QueryAttribute>() != null)
                         .Select(p => $"{{ \"{p.Name}\", {p.Name}.ToString() }},"));

            var methodTemplate = $@"
    public {returnType} {method.Name}({parameterList})
    {{
        var methodCallInfo = _endpointInvoker.GetMethodCallInfo<{interfaceType.Name}>(""{method.Name}"");

        var routeValues = new Dictionary<string, object>()
        {{
            {routeValues}
        }};

        var queryValues = new Dictionary<string, object>()
        {{
            {queryValues}
        }};

        return _endpointInvoker.InvokeEndpointAsync<{method.ReturnType.GenericTypeArguments[0].Name}>(
            methodCallInfo.HttpMethod,
            methodCallInfo.Path,
            routeValues,
            queryValues,
            null
        );
    }}";
            methods.Add(methodTemplate);
        }

        return $@"
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace {namespaceName}
{{
    public class {className} : {interfaceType.Name}
    {{
        private readonly IEndpointInvoker _endpointInvoker;

        public {className}(IEndpointInvoker endpointInvoker)
        {{
            _endpointInvoker = endpointInvoker;
        }}

        {string.Join(Environment.NewLine, methods)}
    }}
}}";
    }

    private static Assembly CompileAssembly(string sourceCode, Type interfaceType)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Task).Assembly.Location),
            MetadataReference.CreateFromFile(interfaceType.Assembly.Location)
        };

        var compilation = CSharpCompilation.Create(
            assemblyName: Path.GetRandomFileName(),
            syntaxTrees: new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            var failures = result.Diagnostics
                .Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

            throw new Exception($"Compilation failed: {string.Join(Environment.NewLine, failures)}");
        }

        ms.Seek(0, SeekOrigin.Begin);
        return AssemblyLoadContext.Default.LoadFromStream(ms);
    }
}