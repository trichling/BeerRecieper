using BeerRecipes.Data;

namespace BeerRecipes.Data;

public interface IBeerRecipeRepository
{
    Task<BeerRecipe?> GetByIdAsync(Guid id);
    Task<IEnumerable<BeerRecipe>> GetAllAsync();
    Task AddAsync(BeerRecipe recipe);
    Task UpdateAsync(BeerRecipe recipe);
    Task DeleteAsync(Guid id);
}