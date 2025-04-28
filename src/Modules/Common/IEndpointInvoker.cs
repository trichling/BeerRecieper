namespace Common;

using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.AspNetCore.Routing.Template;

public interface IEndpointInvoker
{
    Task<TResponse> InvokeEndpointAsync<TResponse>(
        string httpMethod,
        string path,
        IDictionary<string, object>? routeValues = null,
        object? requestBody = null);
}

public class InProcessEndpointInvoker : IEndpointInvoker
{
    private readonly IServiceProvider _serviceProvider;
    private readonly EndpointDataSource _endpointDataSource;

    public InProcessEndpointInvoker(
        IServiceProvider serviceProvider,
        EndpointDataSource endpointDataSource)
    {
        _serviceProvider = serviceProvider;
        _endpointDataSource = endpointDataSource;

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
                    new RouteValueDictionary());
                
                if (matcher.TryMatch(context.Request.Path, values))
                {
                    foreach (var value in values)
                    {
                        context.Request.RouteValues[value.Key] = value.Value;
                    }
                    return endpoint;
                }
            }
        }
        return null;
    }

    public async Task<TResponse> InvokeEndpointAsync<TResponse>(
        string httpMethod,
        string path,
        IDictionary<string, object>? routeValues = null,
        object? requestBody = null)
    {
        // Create HTTP context
        var context = new DefaultHttpContext
        {
            RequestServices = _serviceProvider
        };

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

        // Add request body if present
        if (requestBody != null)
        {
            var json = JsonSerializer.Serialize(requestBody);
            var bytes = Encoding.UTF8.GetBytes(json);
            context.Request.Body = new MemoryStream(bytes);
            context.Request.ContentType = "application/json";
        }

        // Create response body stream
        var responseStream = new MemoryStream();
        context.Response.Body = responseStream;

        // Execute the endpoint pipeline
        var endpoint = FindEndpoint(context);
        if (endpoint == null || endpoint.RequestDelegate == null)
            throw new InvalidOperationException($"No endpoint found for {httpMethod} {path}");
            
        await endpoint.RequestDelegate(context);

        // Read response
        responseStream.Position = 0;
        using var streamReader = new StreamReader(responseStream);
        var jsonResponse = await streamReader.ReadToEndAsync();

        if (context.Response.StatusCode != StatusCodes.Status200OK &&
            context.Response.StatusCode != StatusCodes.Status201Created)
        {
            throw new InvalidOperationException($"Request failed with status code {context.Response.StatusCode}");
        }

        return JsonSerializer.Deserialize<TResponse>(jsonResponse)
            ?? throw new InvalidOperationException("Response was null");
    }
}