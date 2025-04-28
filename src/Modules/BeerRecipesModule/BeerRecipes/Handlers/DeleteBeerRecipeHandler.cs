using BeerRecipes.Contracts.Commands;
using BeerRecipes.Data;
using Common;
using MaltPlans.Contracts.Api;

namespace BeerRecipes.Handlers;

public class DeleteBeerRecipeHandler : IHandler<DeleteBeerRecipeCommand, Unit>
{
    private readonly IBeerRecipeRepository _repository;
    private readonly IMaltPlanHttpApi _maltPlanHttpApi;

    public DeleteBeerRecipeHandler(IBeerRecipeRepository repository, IMaltPlanHttpApi maltPlanHttpApi)
    {
        _repository = repository;
        _maltPlanHttpApi = maltPlanHttpApi;
    }

    public async Task<Unit> HandleAsync(DeleteBeerRecipeCommand command)
    {
        var result = await _maltPlanHttpApi.GetAllMaltPlansAsync();

        await _repository.DeleteAsync(command.Id);
        return Unit.Value;
    }
}