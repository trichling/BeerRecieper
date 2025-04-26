using BeerRecieper.Api.Features.BeerRecipes.Contracts;
using BeerRecieper.Api.Features.BeerRecipes.Data;
using BeerRecieper.Api.Features.BeerRecipes.Contracts.Commands;

namespace BeerRecieper.Api.Features.BeerRecipes;

internal static class BeerRecipeMapper
{
    public static BeerRecipeResponse ToResponse(BeerRecipe recipe)
        => new(recipe.Id, recipe.Name, recipe.Description, recipe.MaltPlanId);

    public static IEnumerable<BeerRecipeResponse> ToResponse(IEnumerable<BeerRecipe> recipes)
        => recipes.Select(ToResponse);
}