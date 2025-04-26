using Lab.BeerRecieper.Features.BeerRecipes.Contracts;
using Lab.BeerRecieper.Features.BeerRecipes.Data;

namespace Lab.BeerRecieper.Features.BeerRecipes;

internal static class BeerRecipeMapper
{
    public static BeerRecipeResponse ToResponse(BeerRecipe recipe)
        => new(recipe.Id, recipe.Name, recipe.Description, recipe.MaltPlanId);

    public static IEnumerable<BeerRecipeResponse> ToResponse(IEnumerable<BeerRecipe> recipes)
        => recipes.Select(ToResponse);
}