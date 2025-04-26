using BeerRecieper.Api.Features.BeerRecipes.Contracts;
using BeerRecieper.Api.Features.BeerRecipes.Contracts.Commands;
using BeerRecieper.Api.Features.BeerRecipes.Contracts.Requests;
using BeerRecieper.Api.Features.Common;
using Microsoft.AspNetCore.Http;

namespace BeerRecieper.Api.Features.BeerRecipes.Endpoints;

public static class CreateRecipeEndpoint
{
    public static async Task<IResult> Handle(
        CreateBeerRecipeRequest request,
        IHandler<CreateBeerRecipeCommand, BeerRecipeResponse> handler)
    {
        var command = new CreateBeerRecipeCommand(request.Name, request.Description);
        var result = await handler.HandleAsync(command);
        return Results.Created($"/beerRecipes/{result.Id}", result);
    }
}