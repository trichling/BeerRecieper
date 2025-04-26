using BeerRecieper.Api.Features.BeerRecipes.Contracts.Commands;
using BeerRecieper.Api.Features.Common;
using Microsoft.AspNetCore.Http;

namespace BeerRecieper.Api.Features.BeerRecipes.Endpoints;

public static class DeleteRecipeEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        IHandler<DeleteBeerRecipeCommand, Unit> handler)
    {
        await handler.HandleAsync(new DeleteBeerRecipeCommand(id));
        return Results.NoContent();
    }
}