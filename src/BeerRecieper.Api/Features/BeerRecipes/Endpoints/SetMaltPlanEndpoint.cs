using BeerRecieper.Api.Features.BeerRecipes.Contracts;
using BeerRecieper.Api.Features.BeerRecipes.Contracts.Commands;
using BeerRecieper.Api.Features.BeerRecipes.Contracts.Requests;
using BeerRecieper.Api.Features.Common;
using Microsoft.AspNetCore.Http;

namespace BeerRecieper.Api.Features.BeerRecipes.Endpoints;

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