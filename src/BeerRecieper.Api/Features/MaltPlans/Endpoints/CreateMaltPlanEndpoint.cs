using BeerRecieper.Api.Features.Common;
using BeerRecieper.Api.Features.MaltPlans.Contracts;
using BeerRecieper.Api.Features.MaltPlans.Contracts.Commands;
using BeerRecieper.Api.Features.MaltPlans.Contracts.Requests;
using Microsoft.AspNetCore.Http;

namespace BeerRecieper.Api.Features.MaltPlans.Endpoints;

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