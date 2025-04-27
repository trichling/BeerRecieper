namespace MaltPlans.Contracts;

public record MaltResponse(string Name, double RelativeAmount, double WeightKg, double MinEbc, double MaxEbc, double AverageEbc);
public record MaltWeightResponse(string MaltName, double WeightKg);
public record MaltPlanResponse(Guid Id, double TotalWeightKg, double AverageEbc, IReadOnlyList<MaltResponse> Malts);