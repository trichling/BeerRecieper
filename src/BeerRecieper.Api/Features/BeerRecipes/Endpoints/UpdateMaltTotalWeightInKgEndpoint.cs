using Lab.BeerRecieper.Features.BeerRecipes.Contracts.Commands;
using Lab.BeerRecieper.Features.Common;
using Microsoft.AspNetCore.Http;

namespace Lab.BeerRecieper.Features.BeerRecipes.Endpoints;

public static class UpdateMaltTotalWeightInKgEndpoint
{
    public static async Task<IResult> Handle(
        Guid id,
        double weight,
        IHandler<UpdateMaltTotalWeightInKgCommand, Unit> handler)
    {
        await handler.HandleAsync(new UpdateMaltTotalWeightInKgCommand(id, weight));
        return Results.NoContent();
    }
}