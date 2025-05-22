using System.Runtime.CompilerServices;
using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

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

    [TestMethod]
    public async Task GenerateClient()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<IEndpointInvoker, InProcessEndpointInvoker>();

        var endpoints = new List<EndpointDataSource>
        {
            new MockEndpointDataSource(
                CreateEndpoint("/getAll", HttpMethods.Get, "getAll"),
                CreateEndpoint("/getOne/{id}", HttpMethods.Get, "getOne")
            ),
        };
        services.AddScoped<EndpointDataSource>(_ => new CompositeEndpointDataSource(endpoints));

        var client = ClientGenerator.For<ITestApi>(services.BuildServiceProvider());

        // Act & Assert
       Assert.IsNotNull(client);

    }

    [TestMethod]
    public async Task GenerateClientDynamically_ShouldGenerateCorrectImplementation()
    {
        // Arrange
        var app = await TestProgram.CreateAppAsync(
            services =>
            {
                services.AddScoped<IEndpointInvoker, InProcessEndpointInvoker>();
                services.AddScoped<ITestApi>(services => ClientGenerator.For<ITestApi>(services));
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


    private sealed class MockEndpointDataSource : EndpointDataSource
    {
        private readonly RouteEndpoint[] _endpoints;

        public MockEndpointDataSource(params RouteEndpoint[] endpoints)
        {
            _endpoints = endpoints;
        }

        public override IReadOnlyList<Endpoint> Endpoints => _endpoints;

        public override IChangeToken GetChangeToken() => new MockChangeToken();
    }

    private sealed class MockChangeToken : IChangeToken
    {
        public bool HasChanged => false;

        public bool ActiveChangeCallbacks => false;

        public IDisposable RegisterChangeCallback(Action<object?> callback, object? state) =>
            new MockDisposable();
    }

    private sealed class MockDisposable : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
        }
    }

    private static RouteEndpoint CreateEndpoint(string pattern, string method, string name)
    {
        var routePattern = RoutePatternFactory.Parse(pattern);

        var metadata = new EndpointMetadataCollection(
            new object[]
            {
                new HttpMethodMetadata(new[] { method }),
                new EndpointNameMetadata(name),
            }
        );

        return new RouteEndpoint(
            requestDelegate: context => Task.CompletedTask,
            routePattern: routePattern,
            order: 0,
            metadata: metadata,
            displayName: pattern
        );
    }
}


