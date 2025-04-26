using BeerRecieper.Api.Features.BeerRecipes.Contracts;
using BeerRecieper.Api.Features.BeerRecipes.Data;
using BeerRecieper.Api.Features.Common;

namespace BeerRecieper.Api.Features.BeerRecipes.Handlers;

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