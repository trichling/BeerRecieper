using Common;
using MaltPlans.Contracts;
using MaltPlans.Contracts.Commands;
using MaltPlans.Contracts.Requests;
using Microsoft.AspNetCore.Http;

namespace MaltPlans.Endpoints;

public static class AddMaltEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        AddMaltRequest request,
        IHandler<AddMaltCommand, MaltPlanResponse?> handler)
    {
        try
        {
            var command = new AddMaltCommand(id, request.MaltName, request.RelativeAmount, request.MinEbc, request.MaxEbc);
            var result = await handler.HandleAsync(command);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}