using Lab.BeerRecieper.Features.BeerRecipes.Contracts.Commands;
using Lab.BeerRecieper.Features.BeerRecipes.Data;
using Lab.BeerRecieper.Features.Common;

namespace Lab.BeerRecieper.Features.BeerRecipes.Handlers;

public class DeleteBeerRecipeHandler : IHandler<DeleteBeerRecipeCommand, Unit>
{
    private readonly IBeerRecipeRepository _repository;

    public DeleteBeerRecipeHandler(IBeerRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> HandleAsync(DeleteBeerRecipeCommand command)
    {
        await _repository.DeleteAsync(command.Id);
        return Unit.Value;
    }
}