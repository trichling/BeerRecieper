using Lab.BeerRecieper.Features.BeerRecipes.Contracts;
using Lab.BeerRecieper.Features.BeerRecipes.Contracts.Commands;
using Lab.BeerRecieper.Features.BeerRecipes.Data;
using Lab.BeerRecieper.Features.Common;
using Lab.BeerRecieper.Features.MaltPlans.Data;

namespace Lab.BeerRecieper.Features.BeerRecipes.Handlers;

public class SetMaltPlanHandler : IHandler<SetMaltPlanCommand, BeerRecipeResponse?>
{
    private readonly IBeerRecipeRepository _recipeRepository;
    private readonly IMaltPlanRepository _maltPlanRepository;

    public SetMaltPlanHandler(IBeerRecipeRepository recipeRepository, IMaltPlanRepository maltPlanRepository)
    {
        _recipeRepository = recipeRepository;
        _maltPlanRepository = maltPlanRepository;
    }

    public async Task<BeerRecipeResponse?> HandleAsync(SetMaltPlanCommand command)
    {
        var recipe = await _recipeRepository.GetByIdAsync(command.Id);
        if (recipe is null)
            return null;

        var maltPlan = await _maltPlanRepository.GetByIdAsync(command.MaltPlanId);
        if (maltPlan is null)
            return null;

        recipe.SetMaltPlan(command.MaltPlanId);
        await _recipeRepository.UpdateAsync(recipe);
        return BeerRecipeMapper.ToResponse(recipe);
    }
}