namespace BeerRecipes.Contracts.Requests;

public record CreateBeerRecipeRequest(string Name, string Description);
public record UpdateBeerRecipeRequest(string Name, string Description);
public record AddMaltPlanRequest(Guid MaltPlanId);