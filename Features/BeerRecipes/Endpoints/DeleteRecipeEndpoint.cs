using Lab.BeerRecieper.Features.BeerRecipes.Contracts.Commands;
using Lab.BeerRecieper.Features.Common;
using Microsoft.AspNetCore.Http;

namespace Lab.BeerRecieper.Features.BeerRecipes.Endpoints;

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