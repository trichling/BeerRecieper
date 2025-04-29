using Common;
using MaltPlans.Contracts;
using MaltPlans.Contracts.Commands;
using Microsoft.AspNetCore.Http;

namespace MaltPlans.Endpoints;

public static class GetMaltPlanByIdEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        bool includeMalt,
        IHandler<GetMaltPlanByIdCommand, MaltPlanResponse?> handler)
    {
        var command = new GetMaltPlanByIdCommand(id, includeMalt);
        var result = await handler.HandleAsync(command);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }
}