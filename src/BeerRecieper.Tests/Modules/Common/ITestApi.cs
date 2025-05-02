using RestEase;

namespace BeerRecieper.Tests.Modules.Common;

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