using BeerRecipes.Contracts;
using BeerRecipes.Contracts.Commands;
using BeerRecipes.Data;
using Common;

namespace BeerRecipes.Handlers;

public class SetMaltPlanHandler : IHandler<SetMaltPlanCommand, BeerRecipeResponse?>
{
    private readonly IBeerRecipeRepository _recipeRepository;

    public SetMaltPlanHandler(IBeerRecipeRepository recipeRepository)
    {
        _recipeRepository = recipeRepository;
    }

    public async Task<BeerRecipeResponse?> HandleAsync(SetMaltPlanCommand command)
    {
        var recipe = await _recipeRepository.GetByIdAsync(command.Id);
        if (recipe is null)
            return null;

        recipe.SetMaltPlan(command.MaltPlanId);
        await _recipeRepository.UpdateAsync(recipe);
        return BeerRecipeMapper.ToResponse(recipe);
    }
}