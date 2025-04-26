using Lab.BeerRecieper.Features.BeerRecipes.Contracts;
using Lab.BeerRecieper.Features.Common;
using Microsoft.AspNetCore.Http;

namespace Lab.BeerRecieper.Features.BeerRecipes.Endpoints;

public static class GetAllRecipesEndpoint
{
    public static async Task<IResult> Handle(
        IHandler<Unit, IEnumerable<BeerRecipeResponse>> handler)
    {
        var result = await handler.HandleAsync(Unit.Value);
        return Results.Ok(result);
    }
}