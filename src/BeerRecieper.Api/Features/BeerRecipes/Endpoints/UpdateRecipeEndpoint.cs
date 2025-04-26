using BeerRecieper.Api.Features.BeerRecipes.Contracts;
using BeerRecieper.Api.Features.BeerRecipes.Contracts.Commands;
using BeerRecieper.Api.Features.BeerRecipes.Contracts.Requests;
using BeerRecieper.Api.Features.Common;
using Microsoft.AspNetCore.Http;

namespace BeerRecieper.Api.Features.BeerRecipes.Endpoints;

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