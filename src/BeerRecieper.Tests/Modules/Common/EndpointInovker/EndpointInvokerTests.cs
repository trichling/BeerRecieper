using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BeerRecieper.Tests.Modules.Common.EndpointInovker;

[TestClass]
public class EndpointInvokerTests
{
    private readonly WebApplication _app;

    public EndpointInvokerTests()
    {
        _app = TestProgram
            .CreateAppAsync(
                services =>
                {
                    services.AddScoped<IEndpointInvoker, InProcessEndpointInvoker>();
                },
                app =>
                {
                    app.MapGet(
                            "/getAll",
                            static () =>
                                Results.Ok(
                                    new List<TestResponse>()
                                    {
                                        new() { Id = "1" },
                                        new() { Id = "2" },
                                    }
                                )
                        )
                        .WithName("getAll");
                    app.MapGet(
                            "/getOne/{id}",
                            static (string id, bool includeSome = false) =>
                                Results.Ok(new TestResponse { Id = id, IncludeSome = includeSome })
                        )
                        .WithName("getOne");
                }
            )
            .GetAwaiter()
            .GetResult();
    }

    [TestMethod]
    public async Task GetAllMaltPlans_ShouldReturnListOfMaltPlans()
    {
        // Arrange
        var endpointInvoker = _app.Services.GetRequiredService<IEndpointInvoker>();
        var result = await endpointInvoker.InvokeEndpointAsync<IEnumerable<TestResponse>>(
            "GET",
            "/getAll"
        );

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(IEnumerable<TestResponse>));
    }

    [TestMethod]
    public async Task GetMaltPlanByIdAsync_ShouldReturnMaltPlans()
    {
        // Arrange
        var endpointInvoker = _app.Services.GetRequiredService<IEndpointInvoker>();
        var result = await endpointInvoker.InvokeEndpointAsync<TestResponse>(
            "GET",
            "/getOne/3",
            new Dictionary<string, object> { { "id", "3" } },
            new Dictionary<string, object> { { "includeSome", "true" } }
        );

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(TestResponse));
        Assert.AreEqual("3", result.Id);
        Assert.IsTrue(result.IncludeSome);
    }
}

public class TestResponse
{
    public required string Id { get; set; }
    public bool IncludeSome { get; set; }
}
