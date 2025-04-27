using Common;
using MaltPlans.Contracts;
using MaltPlans.Contracts.Commands;
using Microsoft.AspNetCore.Http;

namespace MaltPlans.Endpoints;

public static class RemoveMaltEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        string maltName,
        IHandler<RemoveMaltCommand, MaltPlanResponse?> handler)
    {
        var command = new RemoveMaltCommand(id, maltName);
        var result = await handler.HandleAsync(command);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }
}