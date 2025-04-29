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
        var result1 = await _maltPlanHttpApi.GetMaltPlanByIdAsync(Guid.Parse("f1c29f6d-679d-4176-8c69-42591e5d3b89"));

        await _repository.DeleteAsync(command.Id);
        return Unit.Value;
    }
}