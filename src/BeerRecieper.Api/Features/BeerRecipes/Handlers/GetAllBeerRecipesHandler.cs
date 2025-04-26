using Lab.BeerRecieper.Features.BeerRecipes.Contracts;
using Lab.BeerRecieper.Features.BeerRecipes.Data;
using Lab.BeerRecieper.Features.Common;

namespace Lab.BeerRecieper.Features.BeerRecipes.Handlers;

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