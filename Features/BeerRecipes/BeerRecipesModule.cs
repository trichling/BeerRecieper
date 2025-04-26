using Lab.BeerRecieper.Features.BeerRecipes.Contracts;
using Lab.BeerRecieper.Features.BeerRecipes.Contracts.Commands;
using Lab.BeerRecieper.Features.BeerRecipes.Contracts.Requests;
using Lab.BeerRecieper.Features.BeerRecipes.Data;
using Lab.BeerRecieper.Features.BeerRecipes.Endpoints;
using Lab.BeerRecieper.Features.BeerRecipes.Handlers;
using Lab.BeerRecieper.Features.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lab.BeerRecieper.Features.BeerRecipes;

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
        return services;
    }

    public static void MapBeerRecipeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/beerRecipes").WithTags("Beer Recipes");

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
    }
}