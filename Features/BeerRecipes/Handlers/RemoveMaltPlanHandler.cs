using Lab.BeerRecieper.Features.BeerRecipes.Contracts;
using Lab.BeerRecieper.Features.BeerRecipes.Contracts.Commands;
using Lab.BeerRecieper.Features.BeerRecipes.Data;
using Lab.BeerRecieper.Features.Common;

namespace Lab.BeerRecieper.Features.BeerRecipes.Handlers;

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