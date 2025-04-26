using BeerRecieper.Api.Features.BeerRecipes.Contracts;
using BeerRecieper.Api.Features.BeerRecipes.Contracts.Commands;
using BeerRecieper.Api.Features.BeerRecipes.Data;
using BeerRecieper.Api.Features.Common;

namespace BeerRecieper.Api.Features.BeerRecipes.Handlers;

public class GetBeerRecipeByIdHandler : IHandler<GetBeerRecipeByIdCommand, BeerRecipeResponse?>
{
    private readonly IBeerRecipeRepository _repository;

    public GetBeerRecipeByIdHandler(IBeerRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<BeerRecipeResponse?> HandleAsync(GetBeerRecipeByIdCommand command)
    {
        var recipe = await _repository.GetByIdAsync(command.Id);
        return recipe is null ? null : BeerRecipeMapper.ToResponse(recipe);
    }
}