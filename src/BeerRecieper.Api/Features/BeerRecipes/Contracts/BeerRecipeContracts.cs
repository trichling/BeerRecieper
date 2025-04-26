namespace BeerRecieper.Api.Features.BeerRecipes.Contracts;

public record BeerRecipeResponse(Guid Id, string Name, string Description, Guid? MaltPlanId);