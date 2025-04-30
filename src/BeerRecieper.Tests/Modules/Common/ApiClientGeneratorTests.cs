using Common;
using RestEase;
using ApiClientGenerator;

namespace BeerRecieper.Tests.Modules.Common;

[TestClass]
public class ApiClientGeneratorTests
{
    [TestMethod]
    public void GenerateApiClient_ShouldGenerateCorrectImplementation()
    {
        // Arrange & Act
        var generatedCode = ApiClientSourceTextGenerator.GenerateApiClient(typeof(ITestApi));

        // Assert
        Assert.IsNotNull(generatedCode);
        Assert.IsTrue(generatedCode.Contains("public class TestApi : ITestApi"));
        Assert.IsTrue(generatedCode.Contains("private readonly IEndpointInvoker _endpointInvoker"));
        Assert.IsTrue(generatedCode.Contains("public async Task<IEnumerable<TestResponse>> GetAllAsync()"));
        Assert.IsTrue(generatedCode.Contains("public async Task<TestResponse> GetOneAsync(Int32 id, Boolean includeSome)"));
        Assert.IsTrue(generatedCode.Contains("var methodCallInfo = _endpointInvoker.GetMethodCallInfo<ITestApi>()"));
        Assert.IsTrue(generatedCode.Contains("routeValues = new Dictionary<string, object>"));
        Assert.IsTrue(generatedCode.Contains("queryValues = new Dictionary<string, object>"));
        Assert.IsTrue(generatedCode.Contains("var result = await _endpointInvoker.InvokeEndpointAsync<IEnumerable<TestResponse>>("));
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
    public string Id { get; set; } = string.Empty;
    public bool IncludeSome { get; set; } = false;
}