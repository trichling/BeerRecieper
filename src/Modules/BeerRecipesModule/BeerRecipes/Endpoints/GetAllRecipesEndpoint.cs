using BeerRecipes.Contracts;
using Common;
using Microsoft.AspNetCore.Http;

namespace BeerRecipes.Endpoints;

public static class GetAllRecipesEndpoint
{
    public static async Task<IResult> Handle(
        IHandler<Unit, IEnumerable<BeerRecipeResponse>> handler)
    {
        var result = await handler.HandleAsync(Unit.Value);
        return Results.Ok(result);
    }
}