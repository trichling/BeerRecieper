using BeerRecipes.Contracts;
using BeerRecipes.Contracts.Commands;
using Common;
using Microsoft.AspNetCore.Http;

namespace BeerRecipes.Endpoints;

public static class RemoveMaltPlanEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        IHandler<RemoveMaltPlanCommand, BeerRecipeResponse?> handler)
    {
        var command = new RemoveMaltPlanCommand(id);
        var result = await handler.HandleAsync(command);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }
}