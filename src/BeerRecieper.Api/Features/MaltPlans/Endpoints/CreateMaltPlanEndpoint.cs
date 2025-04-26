using Lab.BeerRecieper.Features.Common;
using Lab.BeerRecieper.Features.MaltPlans.Contracts;
using Lab.BeerRecieper.Features.MaltPlans.Contracts.Commands;
using Lab.BeerRecieper.Features.MaltPlans.Contracts.Requests;
using Microsoft.AspNetCore.Http;

namespace Lab.BeerRecieper.Features.MaltPlans.Endpoints;

public static class CreateMaltPlanEndpoint
{
    public static async Task<IResult> Handle(
        CreateMaltPlanRequest request,
        IHandler<CreateMaltPlanCommand, MaltPlanResponse> handler)
    {
        var command = new CreateMaltPlanCommand(request.TotalWeightKg);
        var result = await handler.HandleAsync(command);
        return Results.Created($"/maltplans/{result.Id}", result);
    }
}