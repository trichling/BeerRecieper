using BeerRecieper.Api.Features.Common;
using BeerRecieper.Api.Features.MaltPlans.Contracts;
using BeerRecieper.Api.Features.MaltPlans.Contracts.Commands;
using BeerRecieper.Api.Features.MaltPlans.Contracts.Requests;

namespace BeerRecieper.Api.Features.MaltPlans.Endpoints;

public static class UpdateMaltPlanTotalWeightEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        UpdateMaltPlanWeightRequest request,
        IHandler<UpdateMaltPlanTotalWeightCommand, MaltPlanResponse?> handler)
    {
        try
        {
            var command = new UpdateMaltPlanTotalWeightCommand(id, request.TotalWeightKg);
            var result = await handler.HandleAsync(command);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}