using Lab.BeerRecieper.Features.BeerRecipes.Contracts;
using Lab.BeerRecieper.Features.BeerRecipes.Contracts.Commands;
using Lab.BeerRecieper.Features.Common;
using Microsoft.AspNetCore.Http;

namespace Lab.BeerRecieper.Features.BeerRecipes.Endpoints;

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