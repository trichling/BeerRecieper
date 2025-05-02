using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MaltPlans;
using Common;
using MaltPlans.Contracts.Api;
using BeerRecieper.Tests.Modules.Common.EndpointInovker;

namespace BeerRecieper.Tests;

public class TestProgram
{
    public static async Task<WebApplication> CreateAppAsync(
        Action<IServiceCollection>? configureServices = null,
        Action<WebApplication>? mapEndpoints = null)
    {
        // Create a new WebApplicationBuilder with random port
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            ApplicationName = typeof(TestProgram).Assembly.GetName().Name
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddRouting();

        if (configureServices != null)
        {
            // Add any additional services if needed
            configureServices(builder.Services);
        }

        // Build the app
        var app = builder.Build();

        // Configure routing middleware
        app.UseRouting();

        if (mapEndpoints != null)
        {
            // Map any additional endpoints if needed
            mapEndpoints(app);
        }

        await app.StartAsync();

        return app;
    }
}