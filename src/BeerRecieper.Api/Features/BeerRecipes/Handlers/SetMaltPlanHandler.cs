using BeerRecieper.Api.Features.BeerRecipes.Contracts;
using BeerRecieper.Api.Features.BeerRecipes.Contracts.Commands;
using BeerRecieper.Api.Features.BeerRecipes.Data;
using BeerRecieper.Api.Features.Common;
using BeerRecieper.Api.Features.MaltPlans.Data;

namespace BeerRecieper.Api.Features.BeerRecipes.Handlers;

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