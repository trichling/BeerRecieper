using Lab.BeerRecieper.Features.BeerRecipes.Contracts;
using Lab.BeerRecieper.Features.BeerRecipes.Contracts.Commands;
using Lab.BeerRecieper.Features.Common;
using Microsoft.AspNetCore.Http;

namespace Lab.BeerRecieper.Features.BeerRecipes.Endpoints;

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