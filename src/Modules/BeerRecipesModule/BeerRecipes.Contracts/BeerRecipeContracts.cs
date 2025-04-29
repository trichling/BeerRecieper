using System.Text.Json.Serialization;

namespace BeerRecipes.Contracts;

public record BeerRecipeResponse(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("maltPlanId")] Guid? MaltPlanId);