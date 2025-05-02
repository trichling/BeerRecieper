using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RestEase;

namespace BeerRecieper.Tests.Modules.Common;

[TestClass]
public class ApiClientGeneratorTests
{
    [TestMethod]
    public async Task GenerateApiClient_ShouldGenerateCorrectImplementation()
    {
        // Arrange
        var app = await TestProgram.CreateAppAsync(
            services =>
            {
                services.AddScoped<IEndpointInvoker, InProcessEndpointInvoker>();
                services.AddScoped<ITestApi, TestApi>();
            },
            app =>
            {
                app.MapGet(
                        "/getAll",
                        () =>
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
                        (string id, bool includeSome = false) =>
                            Results.Ok(new TestResponse { Id = id, IncludeSome = includeSome })
                    )
                    .WithName("getOne");
            }
        );

        var testApi = app.Services.GetRequiredService<ITestApi>();

        // Act
        var allResults = await testApi.GetAllAsync();
        var oneResult = await testApi.GetOneAsync(3, includeSome: true);

        // Assert
        Assert.IsNotNull(allResults);
        Assert.AreEqual(2, allResults.Count());
        Assert.AreEqual("1", allResults.First().Id);
        Assert.AreEqual("2", allResults.Last().Id);

        Assert.IsNotNull(oneResult);
        Assert.AreEqual("3", oneResult.Id);
        Assert.IsTrue(oneResult.IncludeSome);
    }
}


