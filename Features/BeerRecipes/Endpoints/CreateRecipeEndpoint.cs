using Lab.BeerRecieper.Features.BeerRecipes.Contracts;
using Lab.BeerRecieper.Features.BeerRecipes.Contracts.Commands;
using Lab.BeerRecieper.Features.BeerRecipes.Contracts.Requests;
using Lab.BeerRecieper.Features.Common;
using Microsoft.AspNetCore.Http;

namespace Lab.BeerRecieper.Features.BeerRecipes.Endpoints;

public static class CreateRecipeEndpoint
{
    public static async Task<IResult> Handle(
        CreateBeerRecipeRequest request,
        IHandler<CreateBeerRecipeCommand, BeerRecipeResponse> handler)
    {
        var command = new CreateBeerRecipeCommand(request.Name, request.Description);
        var result = await handler.HandleAsync(command);
        return Results.Created($"/beerRecipes/{result.Id}", result);
    }
}