namespace Lab.BeerRecieper.Features.BeerRecipes.Contracts.Commands;

public record CreateBeerRecipeCommand(string Name, string Description);
public record UpdateBeerRecipeCommand(Guid Id, string Name, string Description);
public record DeleteBeerRecipeCommand(Guid Id);
public record GetBeerRecipeByIdCommand(Guid Id);