using BeerRecipes.Contracts;
using BeerRecipes.Contracts.Commands;
using BeerRecipes.Data;
using Common;

namespace BeerRecipes.Handlers;

public class CreateBeerRecipeHandler : IHandler<CreateBeerRecipeCommand, BeerRecipeResponse>
{
    private readonly IBeerRecipeRepository _repository;

    public CreateBeerRecipeHandler(IBeerRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<BeerRecipeResponse> HandleAsync(CreateBeerRecipeCommand command)
    {
        var recipe = new BeerRecipe(command.Name, command.Description);
        await _repository.AddAsync(recipe);
        return BeerRecipeMapper.ToResponse(recipe);
    }
}