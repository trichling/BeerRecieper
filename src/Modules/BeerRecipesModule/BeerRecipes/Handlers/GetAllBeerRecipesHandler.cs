using BeerRecipes.Contracts;
using BeerRecipes.Data;
using Common;

namespace BeerRecipes.Handlers;

public class GetAllBeerRecipesHandler : IHandler<Unit, IEnumerable<BeerRecipeResponse>>
{
    private readonly IBeerRecipeRepository _repository;

    public GetAllBeerRecipesHandler(IBeerRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BeerRecipeResponse>> HandleAsync(Unit request)
    {
        var recipes = await _repository.GetAllAsync();
        return BeerRecipeMapper.ToResponse(recipes);
    }
}