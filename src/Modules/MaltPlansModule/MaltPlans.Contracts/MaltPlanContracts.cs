using System.Text.Json.Serialization;

namespace MaltPlans.Contracts;

public record MaltResponse(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("relativeAmount")] double RelativeAmount,
    [property: JsonPropertyName("weightKg")] double WeightKg,
    [property: JsonPropertyName("minEbc")] double MinEbc,
    [property: JsonPropertyName("maxEbc")] double MaxEbc,
    [property: JsonPropertyName("averageEbc")] double AverageEbc
);

public record MaltWeightResponse(
    [property: JsonPropertyName("maltName")] string MaltName,
    [property: JsonPropertyName("weightKg")] double WeightKg
);

public record MaltPlanResponse(
    [property: JsonPropertyName("id")] Guid Id,
    [property: JsonPropertyName("totalWeightKg")] double TotalWeightKg,
    [property: JsonPropertyName("averageEbc")] double AverageEbc,
    [property: JsonPropertyName("malts")] IReadOnlyList<MaltResponse> Malts
);