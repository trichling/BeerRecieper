using BeerRecipes.Contracts.Commands;
using BeerRecipes.Data;
using Common;

namespace BeerRecipes.Handlers;

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