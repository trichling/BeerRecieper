using BeerRecipes.Contracts;
using BeerRecipes.Contracts.Commands;
using BeerRecipes.Contracts.Requests;
using BeerRecipes.Data;
using BeerRecipes.Endpoints;
using BeerRecipes.Handlers;
using MaltPlans.Contracts;
using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using MaltPlans.Contracts.Api;

namespace BeerRecipes;

public static class BeerRecipesModule
{
    public static IServiceCollection AddBeerRecipeServices(this IServiceCollection services)
    {
        services.AddSingleton<IBeerRecipeRepository, InMemoryBeerRecipeRepository>();
        services.AddScoped<IHandler<Unit, IEnumerable<BeerRecipeResponse>>, GetAllBeerRecipesHandler>();
        services.AddScoped<IHandler<GetBeerRecipeByIdCommand, BeerRecipeResponse?>, GetBeerRecipeByIdHandler>();
        services.AddScoped<IHandler<CreateBeerRecipeCommand, BeerRecipeResponse>, CreateBeerRecipeHandler>();
        services.AddScoped<IHandler<UpdateBeerRecipeCommand, BeerRecipeResponse?>, UpdateBeerRecipeHandler>();
        services.AddScoped<IHandler<DeleteBeerRecipeCommand, Unit>, DeleteBeerRecipeHandler>();
        services.AddScoped<IHandler<SetMaltPlanCommand, BeerRecipeResponse?>, SetMaltPlanHandler>();
        services.AddScoped<IHandler<RemoveMaltPlanCommand, BeerRecipeResponse?>, RemoveMaltPlanHandler>();
        services.AddScoped<IHandler<UpdateMaltTotalWeightInKgCommand, Unit>, UpdateMaltTotalWeightInKgHandler>();
        return services;
    }

    public static void RegisterExternalServicesForHttp(this IServiceCollection services)
    {
        services.AddHttpClient("MaltPlans", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5001");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        });
        // let IMaltPlanHtppApi use this http client
        services.AddScoped<IMaltPlanHttpApi>(services =>
        {
            throw new NotImplementedException("IMaltPlanHttpApi is not implemented yet");
        });
    }
    public static void MapBeerRecipeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/beerrecipes").WithTags("Beer Recipes");

        group.MapGet("/", ([FromServices] IHandler<Unit, IEnumerable<BeerRecipeResponse>> handler) =>
            GetAllRecipesEndpoint.Handle(handler))
            .WithName("GetAllRecipes");

        group.MapGet("/{id}", ([FromRoute] Guid id, [FromServices] IHandler<GetBeerRecipeByIdCommand, BeerRecipeResponse?> handler) =>
            GetRecipeByIdEndpoint.Handle(id, handler))
            .WithName("GetRecipeById");

        group.MapPost("/", ([FromBody] CreateBeerRecipeRequest request, [FromServices] IHandler<CreateBeerRecipeCommand, BeerRecipeResponse> handler) =>
            CreateRecipeEndpoint.Handle(request, handler))
            .WithName("CreateRecipe");

        group.MapPut("/{id}", ([FromRoute] Guid id, [FromBody] UpdateBeerRecipeRequest request, [FromServices] IHandler<UpdateBeerRecipeCommand, BeerRecipeResponse?> handler) =>
            UpdateRecipeEndpoint.Handle(id, request, handler))
            .WithName("UpdateRecipe");

        group.MapDelete("/{id}", ([FromRoute] Guid id, [FromServices] IHandler<DeleteBeerRecipeCommand, Unit> handler) =>
            DeleteRecipeEndpoint.Handle(id, handler))
            .WithName("DeleteRecipe");

        group.MapPut("/{id}/maltPlan", ([FromRoute] Guid id, [FromBody] AddMaltPlanRequest request, [FromServices] IHandler<SetMaltPlanCommand, BeerRecipeResponse?> handler) =>
            SetMaltPlanEndpoint.Handle(id, request, handler))
            .WithName("SetMaltPlan");

        group.MapDelete("/{id}/maltplan", ([FromRoute] Guid id, [FromServices] IHandler<RemoveMaltPlanCommand, BeerRecipeResponse?> handler) =>
            RemoveMaltPlanEndpoint.Handle(id, handler))
            .WithName("RemoveMaltPlan");

        group.MapPut("/{id}/malttotalweight/{weight}", async ([FromRoute] Guid id, [FromRoute] double weight, [FromServices] IHandler<UpdateMaltTotalWeightInKgCommand, Unit> handler) =>
            await UpdateMaltTotalWeightInKgEndpoint.Handle(id, weight, handler))
            .WithName("UpdateMaltPlanTotalWeight");

    }
}