using BeerRecieper.Api.Features.BeerRecipes.Contracts.Commands;
using BeerRecieper.Api.Features.BeerRecipes.Data;
using BeerRecieper.Api.Features.Common;
using BeerRecieper.Api.Features.MaltPlans.Contracts.Commands;

namespace BeerRecieper.Api.Features.BeerRecipes.Handlers;

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