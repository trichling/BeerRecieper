using BeerRecieper.Api.Features.BeerRecipes.Contracts;
using BeerRecieper.Api.Features.BeerRecipes.Contracts.Commands;
using BeerRecieper.Api.Features.BeerRecipes.Data;
using BeerRecieper.Api.Features.Common;

namespace BeerRecieper.Api.Features.BeerRecipes.Handlers;

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