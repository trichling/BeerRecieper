using BeerRecipes.Contracts;
using BeerRecipes.Contracts.Commands;
using BeerRecipes.Contracts.Requests;
using Common;
using Microsoft.AspNetCore.Http;

namespace BeerRecipes.Endpoints;

public static class UpdateRecipeEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        UpdateBeerRecipeRequest request,
        IHandler<UpdateBeerRecipeCommand, BeerRecipeResponse?> handler)
    {
        var command = new UpdateBeerRecipeCommand(id, request.Name, request.Description);
        var result = await handler.HandleAsync(command);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }
}