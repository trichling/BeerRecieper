using BeerRecieper.Api.Features.BeerRecipes.Contracts.Commands;
using BeerRecieper.Api.Features.BeerRecipes.Data;
using BeerRecieper.Api.Features.Common;

namespace BeerRecieper.Api.Features.BeerRecipes.Handlers;

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