using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MaltPlans;
using Common;
using Microsoft.AspNetCore.Routing;
using MaltPlans.Contracts.Api;
using BeerRecieper.Tests.Modules.Common.EndpointInovker;

namespace BeerRecieper.Tests;

public class TestProgram
{
    public static async Task<WebApplication> CreateAppAsync(Action<IServiceCollection>? configureServices = null, Action<WebApplication> mapEndpoints = null)

    {
        // Create a new WebApplicationBuilder
        var builder = WebApplication.CreateBuilder();
        
        builder.Services.AddEndpointsApiExplorer();
        if (configureServices != null)
        {
            // Add any additional services if needed
            configureServices(builder.Services);
        }

        // Build the app
        var app = builder.Build();

        if (mapEndpoints != null)
        {
            // Map any additional endpoints if needed
            mapEndpoints(app);
        }

        // Map endpoints

        await app.StartAsync();

        return app;
    }
  
}