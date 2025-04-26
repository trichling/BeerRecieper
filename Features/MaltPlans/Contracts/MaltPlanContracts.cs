namespace Lab.BeerRecieper.Features.MaltPlans.Contracts;

public record MaltResponse(string Name, double RelativeAmount, double WeightKg);
public record MaltWeightResponse(string MaltName, double WeightKg);
public record MaltPlanResponse(Guid Id, double TotalWeightKg, IReadOnlyList<MaltResponse> Malts);