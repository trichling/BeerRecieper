namespace Common;

using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Primitives;

public interface IEndpointInvoker
{
    Task<TResponse> InvokeEndpointAsync<TResponse>(
        string httpMethod,
        string path,
        IDictionary<string, object>? routeValues = null,
        IDictionary<string, object>? queryValues = null,
        object? requestBody = null
    );
}

public class InProcessEndpointInvoker : IEndpointInvoker
{
    private readonly IServiceProvider _serviceProvider;
    private readonly EndpointDataSource _endpointDataSource;

    public InProcessEndpointInvoker(
        IServiceProvider serviceProvider,
        EndpointDataSource endpointDataSource
    )
    {
        _serviceProvider = serviceProvider;
        _endpointDataSource = endpointDataSource;
    }

    public async Task<TResponse> InvokeEndpointAsync<TResponse>(
        string httpMethod,
        string path,
        IDictionary<string, object>? routeValues = null,
        IDictionary<string, object>? queryValues = null,
        object? requestBody = null
    )
    {
        // Create HTTP context
        var context = CreateHttpContext(httpMethod, path, routeValues, queryValues, requestBody);

        // Execute the endpoint pipeline
        var endpoint = FindEndpoint(context);
        if (endpoint == null || endpoint.RequestDelegate == null)
            throw new InvalidOperationException($"No endpoint found for {httpMethod} {path}");

        await endpoint.RequestDelegate(context);

        // Read response
        context.Response.Body.Position = 0;
        using var streamReader = new StreamReader(context.Response.Body);
        var jsonResponse = await streamReader.ReadToEndAsync();

        if (
            context.Response.StatusCode != StatusCodes.Status200OK
            && context.Response.StatusCode != StatusCodes.Status201Created
        )
        {
            throw new InvalidOperationException(
                $"Request failed with status code {context.Response.StatusCode}"
            );
        }

        return JsonSerializer.Deserialize<TResponse>(jsonResponse)
            ?? throw new InvalidOperationException("Response was null");
    }

    private HttpContext CreateHttpContext(
        string httpMethod,
        string path,
        IDictionary<string, object>? routeValues,
        IDictionary<string, object>? queryValues,
        object? requestBody
    )
    {
        var context = new DefaultHttpContext { RequestServices = _serviceProvider };

        // Setup the request
        context.Request.Method = httpMethod;
        context.Request.Path = path;

        // Add route values if present
        if (routeValues != null)
        {
            var routeValueDict = new RouteValueDictionary();
            foreach (var kvp in routeValues)
            {
                routeValueDict[kvp.Key] = kvp.Value;
            }
            context.Request.RouteValues = routeValueDict;
        }

        // Add query collection to request
        if (queryValues != null)
        {
            var queryDict = new Dictionary<string, StringValues>();
            foreach (var kvp in queryValues)
            {
                queryDict[kvp.Key] = kvp.Value.ToString() ?? string.Empty;
            }
            context.Request.Query = new QueryCollection(queryDict);
        }

        // Add request body if present
        if (requestBody != null)
        {
            var json = JsonSerializer.Serialize(requestBody);
            var bytes = Encoding.UTF8.GetBytes(json);
            context.Request.Body = new MemoryStream(bytes);
            context.Request.ContentType = "application/json";
        }

        // Create response body stream
        context.Response.Body = new MemoryStream();

        return context;
    }

    private Endpoint? FindEndpoint(HttpContext context)
    {
        foreach (var endpoint in _endpointDataSource.Endpoints)
        {
            if (endpoint is RouteEndpoint routeEndpoint)
            {
                var values = new RouteValueDictionary();
                var matcher = new TemplateMatcher(
                    TemplateParser.Parse(routeEndpoint.RoutePattern.RawText ?? string.Empty),
                    new RouteValueDictionary()
                );

                if (matcher.TryMatch(context.Request.Path, values))
                {
                    return endpoint;
                }
            }
        }
        return null;
    }
}
