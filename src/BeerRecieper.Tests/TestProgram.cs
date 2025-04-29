using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MaltPlans;

namespace BeerRecieper.Tests;

public class TestProgram
{
    public static async Task<WebApplication> CreateAppAsync()
    {
        var builder = WebApplication.CreateBuilder();

        // Add services required for testing
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddMaltPlanServices();

        var app = builder.Build();

        // Map endpoints
        app.MapMaltPlanEndpoints();

        await app.StartAsync();

        return app;
    }
}