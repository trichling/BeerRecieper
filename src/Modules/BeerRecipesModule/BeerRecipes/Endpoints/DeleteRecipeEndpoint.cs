using BeerRecipes.Contracts.Commands;
using Common;
using Microsoft.AspNetCore.Http;

namespace BeerRecipes.Endpoints;

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