using Lab.BeerRecieper.Features.BeerRecipes.Contracts;
using Lab.BeerRecieper.Features.BeerRecipes.Contracts.Commands;
using Lab.BeerRecieper.Features.BeerRecipes.Data;
using Lab.BeerRecieper.Features.Common;

namespace Lab.BeerRecieper.Features.BeerRecipes.Handlers;

public class UpdateBeerRecipeHandler : IHandler<UpdateBeerRecipeCommand, BeerRecipeResponse?>
{
    private readonly IBeerRecipeRepository _repository;

    public UpdateBeerRecipeHandler(IBeerRecipeRepository repository)
    {
        _repository = repository;
    }

    public async Task<BeerRecipeResponse?> HandleAsync(UpdateBeerRecipeCommand command)
    {
        var recipe = await _repository.GetByIdAsync(command.Id);
        if (recipe is null)
            return null;

        recipe.Update(command.Name, command.Description);
        await _repository.UpdateAsync(recipe);
        return BeerRecipeMapper.ToResponse(recipe);
    }
}