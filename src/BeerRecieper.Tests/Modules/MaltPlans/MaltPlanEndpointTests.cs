using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace BeerRecieper.Tests.Modules.MaltPlans;

[TestClass]
public class MaltPlanEndpointTests : IDisposable
{
    private WebApplicationFactory<TestProgram> _factory = null!;
    private WebApplication _app = null!;
    private EndpointDataSource _endpointDataSource = null!;

    [TestInitialize]
    public async Task Setup()
    {
        _app = await TestProgram.CreateAppAsync();

        // Get endpoint data source after app is built
        _endpointDataSource = _app.Services.GetRequiredService<EndpointDataSource>();
    }

    [TestMethod]
    public void EndpointDataSource_Contains_MaltPlanEndpoints()
    {
        Assert.IsNotNull(_endpointDataSource, "EndpointDataSource was not initialized");

        // Assert that endpoints exist
        var endpoints = _endpointDataSource.Endpoints.ToList();
        Assert.IsTrue(endpoints.Any(), "No endpoints were found");

        // Check for specific MaltPlan endpoints
        var endpointNames = endpoints
            .Select(e => e.Metadata.GetMetadata<EndpointNameMetadata>()?.EndpointName)
            .Where(name => name != null)
            .ToList();

        Assert.IsTrue(
            endpointNames.Contains("GetAllMaltPlans"),
            "GetAllMaltPlans endpoint not found"
        );
        Assert.IsTrue(
            endpointNames.Contains("GetMaltPlanById"),
            "GetMaltPlanById endpoint not found"
        );
        Assert.IsTrue(
            endpointNames.Contains("CreateMaltPlan"),
            "CreateMaltPlan endpoint not found"
        );
        Assert.IsTrue(
            endpointNames.Contains("UpdateMaltPlanWeight"),
            "UpdateMaltPlanWeight endpoint not found"
        );
    }

    public void Dispose()
    {
        _app?.DisposeAsync().AsTask().Wait();
        _factory?.Dispose();
    }
}