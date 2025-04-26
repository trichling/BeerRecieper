using BeerRecieper.Api.Features.Common;
using BeerRecieper.Api.Features.MaltPlans.Contracts;

namespace BeerRecieper.Api.Features.MaltPlans.Endpoints;

public static class GetAllMaltPlansEndpoint
{
    public static async Task<IResult> Handle(
        IHandler<Unit, IEnumerable<MaltPlanResponse>> handler)
    {
        var result = await handler.HandleAsync(Unit.Value);
        return Results.Ok(result);
    }
}