namespace BeerRecipes.Data;

public class InMemoryBeerRecipeRepository : IBeerRecipeRepository
{
    private readonly List<BeerRecipe> _recipes = new();

    public Task<BeerRecipe?> GetByIdAsync(Guid id)
    {
        var recipe = _recipes.FirstOrDefault(r => r.Id == id);
        return Task.FromResult(recipe);
    }

    public Task<IEnumerable<BeerRecipe>> GetAllAsync()
    {
        return Task.FromResult(_recipes.AsEnumerable());
    }

    public Task AddAsync(BeerRecipe recipe)
    {
        _recipes.Add(recipe);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(BeerRecipe recipe)
    {
        var index = _recipes.FindIndex(r => r.Id == recipe.Id);
        if (index != -1)
        {
            _recipes[index] = recipe;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _recipes.RemoveAll(r => r.Id == id);
        return Task.CompletedTask;
    }
}