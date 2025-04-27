using BeerRecipes.Contracts;
using BeerRecipes.Contracts.Commands;
using Common;
using Microsoft.AspNetCore.Http;

namespace BeerRecipes.Endpoints;

public static class GetRecipeByIdEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        IHandler<GetBeerRecipeByIdCommand, BeerRecipeResponse?> handler)
    {
        var result = await handler.HandleAsync(new GetBeerRecipeByIdCommand(id));
        return result is null ? Results.NotFound() : Results.Ok(result);
    }
}