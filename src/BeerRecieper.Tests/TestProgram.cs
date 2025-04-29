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
    public static async Task<WebApplication> CreateAppAsync()
    {
        var builder = WebApplication.CreateBuilder();

        // Add services required for testing
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddMaltPlanServices();

        builder.Services.AddScoped<IEndpointInvoker>(sp =>
        {
            var endpointDataSource = sp.GetRequiredService<EndpointDataSource>();
            return new InProcessEndpointInvoker(sp, endpointDataSource);
        });

        builder.Services.AddTransient<IMaltPlanHttpApi, InProcessMaltPlanHttpApi>();

        var app = builder.Build();

        // Map endpoints
        app.MapMaltPlanEndpoints();

        await app.StartAsync();

        return app;
    }
}