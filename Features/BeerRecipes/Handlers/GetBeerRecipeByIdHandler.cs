using Lab.BeerRecieper.Features.BeerRecipes.Contracts;
using Lab.BeerRecieper.Features.BeerRecipes.Contracts.Commands;
using Lab.BeerRecieper.Features.BeerRecipes.Data;
using Lab.BeerRecieper.Features.Common;

namespace Lab.BeerRecieper.Features.BeerRecipes.Handlers;

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