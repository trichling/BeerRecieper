using Lab.BeerRecieper.Features.Common;
using Lab.BeerRecieper.Features.MaltPlans.Contracts;
using Lab.BeerRecieper.Features.MaltPlans.Contracts.Commands;
using Lab.BeerRecieper.Features.MaltPlans.Contracts.Requests;
using Microsoft.AspNetCore.Http;

namespace Lab.BeerRecieper.Features.MaltPlans.Endpoints;

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