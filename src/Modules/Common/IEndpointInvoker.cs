namespace Common;

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
    private readonly IApplicationBuilder _app;

    public InProcessEndpointInvoker(IServiceProvider serviceProvider, IApplicationBuilder app)
    {
        _serviceProvider = serviceProvider;
        _app = app;
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
            context.Request.RouteValues = new RouteValueDictionary(routeValues);
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
        await _app.Build()(context);

        // Read response
        responseStream.Position = 0;
        using var streamReader = new StreamReader(responseStream);
        var jsonResponse = await streamReader.ReadToEndAsync();

        if (context.Response.StatusCode != StatusCodes.Status200OK)
        {
            throw new InvalidOperationException($"Request failed with status code {context.Response.StatusCode}");
        }

        return JsonSerializer.Deserialize<TResponse>(jsonResponse) 
            ?? throw new InvalidOperationException("Response was null");
    }
}