using BeerRecieper.Api.Features.Common;
using BeerRecieper.Api.Features.MaltPlans.Contracts;
using BeerRecieper.Api.Features.MaltPlans.Contracts.Commands;

namespace BeerRecieper.Api.Features.MaltPlans.Endpoints;

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