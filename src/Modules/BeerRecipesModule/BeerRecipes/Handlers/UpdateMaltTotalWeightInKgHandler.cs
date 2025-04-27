using BeerRecipes.Contracts.Commands;
using BeerRecipes.Data;
using Common;
using MaltPlans.Contracts.Commands;

namespace BeerRecipes.Handlers;

public class UpdateMaltTotalWeightInKgHandler : IHandler<UpdateMaltTotalWeightInKgCommand, Unit>
{
    private readonly IBeerRecipeRepository _repository;
    private readonly IInvoke<UpdateMaltPlanTotalWeightCommand> _updateMaltPlanWeight;

    public UpdateMaltTotalWeightInKgHandler(
        IBeerRecipeRepository repository,
        IInvoke<UpdateMaltPlanTotalWeightCommand> updateMaltPlanWeight)
    {
        _repository = repository;
        _updateMaltPlanWeight = updateMaltPlanWeight;
    }

    public async Task<Unit> HandleAsync(UpdateMaltTotalWeightInKgCommand command)
    {
        var recipe = await _repository.GetByIdAsync(command.RecipeId);
        if (recipe?.MaltPlanId != null)
            await _updateMaltPlanWeight.InvokeAsync(new UpdateMaltPlanTotalWeightCommand(recipe.MaltPlanId.Value, command.TotalWeightKg));

        return Unit.Value;
    }
}