using BeerRecipes.Contracts;
using BeerRecipes.Contracts.Commands;
using BeerRecipes.Contracts.Requests;
using Common;
using Microsoft.AspNetCore.Http;

namespace BeerRecipes.Endpoints;

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