using BeerRecipes.Contracts;
using BeerRecipes.Contracts.Commands;
using BeerRecipes.Data;
using Common;

namespace BeerRecipes.Handlers;

public class RemoveMaltPlanHandler : IHandler<RemoveMaltPlanCommand, BeerRecipeResponse?>
{
    private readonly IBeerRecipeRepository _repository;

    public RemoveMaltPlanHandler(IBeerRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<BeerRecipeResponse?> HandleAsync(RemoveMaltPlanCommand command)
    {
        var recipe = await _repository.GetByIdAsync(command.Id);
        if (recipe is null)
            return null;

        recipe.RemoveMaltPlan();
        await _repository.UpdateAsync(recipe);
        return BeerRecipeMapper.ToResponse(recipe);
    }
}