using Common;
using MaltPlans.Contracts;
using Microsoft.AspNetCore.Http;

namespace MaltPlans.Endpoints;

public static class GetAllMaltPlansEndpoint
{
    public static async Task<IResult> Handle(
        IHandler<Unit, IEnumerable<MaltPlanResponse>> handler)
    {
        var result = await handler.HandleAsync(Unit.Value);
        return Results.Ok(result);
    }
}