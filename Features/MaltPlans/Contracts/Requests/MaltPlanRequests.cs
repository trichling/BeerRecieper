namespace Lab.BeerRecieper.Features.MaltPlans.Contracts.Requests;

public record CreateMaltPlanRequest(double TotalWeightKg);
public record UpdateMaltPlanWeightRequest(double TotalWeightKg);
public record AddMaltRequest(string MaltName, double RelativeAmount);
public record UpdateMaltAmountRequest(double RelativeAmount);