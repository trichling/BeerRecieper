using Lab.BeerRecieper.Features.BeerRecipes.Contracts;
using Lab.BeerRecieper.Features.BeerRecipes.Contracts.Commands;
using Lab.BeerRecieper.Features.BeerRecipes.Contracts.Requests;
using Lab.BeerRecieper.Features.Common;
using Microsoft.AspNetCore.Http;

namespace Lab.BeerRecieper.Features.BeerRecipes.Endpoints;

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