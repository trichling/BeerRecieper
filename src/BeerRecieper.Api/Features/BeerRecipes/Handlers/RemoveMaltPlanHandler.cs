using BeerRecieper.Api.Features.BeerRecipes.Contracts;
using BeerRecieper.Api.Features.BeerRecipes.Contracts.Commands;
using BeerRecieper.Api.Features.BeerRecipes.Data;
using BeerRecieper.Api.Features.Common;

namespace BeerRecieper.Api.Features.BeerRecipes.Handlers;

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