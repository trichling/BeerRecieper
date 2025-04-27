using Common;
using MaltPlans.Contracts;
using MaltPlans.Contracts.Commands;
using MaltPlans.Contracts.Requests;
using Microsoft.AspNetCore.Http;

namespace MaltPlans.Endpoints;

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