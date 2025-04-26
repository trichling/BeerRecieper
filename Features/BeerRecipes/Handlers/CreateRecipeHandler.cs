using Lab.BeerRecieper.Features.BeerRecipes.Contracts;
using Lab.BeerRecieper.Features.BeerRecipes.Contracts.Commands;
using Lab.BeerRecieper.Features.BeerRecipes.Data;
using Lab.BeerRecieper.Features.Common;

namespace Lab.BeerRecieper.Features.BeerRecipes.Handlers;

public class CreateRecipeHandler : IHandler<CreateBeerRecipeCommand, BeerRecipeResponse>
{
    private readonly IBeerRecipeRepository _repository;

    public CreateRecipeHandler(IBeerRecipeRepository repository)
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