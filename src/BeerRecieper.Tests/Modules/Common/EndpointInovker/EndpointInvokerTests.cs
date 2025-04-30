using System.Runtime.CompilerServices;
using Common;
using MaltPlans;
using MaltPlans.Contracts;
using MaltPlans.Contracts.Api;
using MaltPlans.Contracts.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using RestEase;

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
                    services.AddScoped<ITestApi, TestApi>();
                },
                app =>
                {
                    app.MapGet(
                            "/getAll",
                            static () =>
                                Results.Ok(
                                    new List<TestResponse>()
                                    {
                                        new TestResponse() { Id = "1" },
                                        new TestResponse() { Id = "2" },
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
        var maltPlanApi = _app.Services.GetRequiredService<ITestApi>();

        // Act
        var result = await maltPlanApi.GetAllAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(IEnumerable<TestResponse>));
    }

    [TestMethod]
    public async Task GetMaltPlanByIdAsync_ShouldReturnMaltPlans()
    {
        // Arrange
        var maltPlanApi = _app.Services.GetRequiredService<ITestApi>();

        // Act
        var result = await maltPlanApi.GetOneAsync(3, true);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(TestResponse));
        Assert.AreEqual("3", result.Id);
        Assert.IsTrue(result.IncludeSome);
    }
}

public interface ITestApi
{
    [Get("/getAll")]
    Task<IEnumerable<TestResponse>> GetAllAsync();

    [Get("/getOne/{id}")]
    Task<TestResponse> GetOneAsync([Path] int id, [Query] bool includeSome = false);
}

public class TestResponse
{
    public string Id { get; set; }

    public bool IncludeSome { get; set; } = false;
}

public class TestApi : ITestApi
{
    private readonly IEndpointInvoker endpointInvoker;

    public TestApi(IEndpointInvoker endpointInvoker)
    {
        this.endpointInvoker = endpointInvoker;
    }

    public async Task<IEnumerable<TestResponse>> GetAllAsync()
    {
        var methodCallInfo = endpointInvoker.GetMethodCallInfo<ITestApi>();

        var result = await endpointInvoker.InvokeEndpointAsync<IEnumerable<TestResponse>>(
            methodCallInfo.HttpMethod,
            methodCallInfo.Path
        );

        return result;
    }

    public async Task<TestResponse> GetOneAsync(int id, bool includeSome = false)
    {
        var methodCallInfo = endpointInvoker.GetMethodCallInfo<ITestApi>(nameof(GetOneAsync));

        var result = await endpointInvoker.InvokeEndpointAsync<TestResponse>(
            methodCallInfo.HttpMethod,
            methodCallInfo.Path,
            new Dictionary<string, object> { { "id", id.ToString() } },
            new Dictionary<string, object> { { "includeSome", includeSome.ToString() } }
        );

        return result;
    }
}
