using BeerRecipes.Contracts;
using BeerRecipes.Contracts.Commands;
using BeerRecipes.Contracts.Requests;
using Common;
using Microsoft.AspNetCore.Http;

namespace BeerRecipes.Endpoints;

public static class SetMaltPlanEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        AddMaltPlanRequest request,
        IHandler<SetMaltPlanCommand, BeerRecipeResponse?> handler)
    {
        var command = new SetMaltPlanCommand(id, request.MaltPlanId);
        var result = await handler.HandleAsync(command);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }
}