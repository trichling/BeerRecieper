using BeerRecieper.Api.Features.Common;
using BeerRecieper.Api.Features.MaltPlans.Contracts;
using BeerRecieper.Api.Features.MaltPlans.Contracts.Commands;
using BeerRecieper.Api.Features.MaltPlans.Contracts.Requests;
using Microsoft.AspNetCore.Http;

namespace BeerRecieper.Api.Features.MaltPlans.Endpoints;

public static class UpdateMaltAmountEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        string maltName,
        UpdateMaltAmountRequest request,
        IHandler<UpdateMaltAmountCommand, MaltPlanResponse?> handler)
    {
        try
        {
            var command = new UpdateMaltAmountCommand(id, maltName, request.RelativeAmount);
            var result = await handler.HandleAsync(command);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}