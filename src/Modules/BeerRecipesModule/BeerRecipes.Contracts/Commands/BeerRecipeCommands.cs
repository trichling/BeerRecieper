namespace BeerRecipes.Contracts.Commands;

public record CreateBeerRecipeCommand(string Name, string Description);
public record UpdateBeerRecipeCommand(Guid Id, string Name, string Description);
public record DeleteBeerRecipeCommand(Guid Id);
public record GetBeerRecipeByIdCommand(Guid Id);
public record SetMaltPlanCommand(Guid Id, Guid MaltPlanId);
public record RemoveMaltPlanCommand(Guid Id);
public record UpdateMaltTotalWeightInKgCommand(Guid RecipeId, double TotalWeightKg);