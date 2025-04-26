using BeerRecieper.Api.Features.BeerRecipes.Contracts;
using BeerRecieper.Api.Features.BeerRecipes.Contracts.Commands;
using BeerRecieper.Api.Features.Common;
using Microsoft.AspNetCore.Http;

namespace BeerRecieper.Api.Features.BeerRecipes.Endpoints;

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