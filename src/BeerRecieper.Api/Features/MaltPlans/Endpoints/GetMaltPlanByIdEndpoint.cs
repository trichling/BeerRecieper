using Lab.BeerRecieper.Features.Common;
using Lab.BeerRecieper.Features.MaltPlans.Contracts;
using Lab.BeerRecieper.Features.MaltPlans.Contracts.Commands;
using Microsoft.AspNetCore.Http;

namespace Lab.BeerRecieper.Features.MaltPlans.Endpoints;

public static class GetMaltPlanByIdEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        IHandler<GetMaltPlanByIdCommand, MaltPlanResponse?> handler)
    {
        var command = new GetMaltPlanByIdCommand(id);
        var result = await handler.HandleAsync(command);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }
}