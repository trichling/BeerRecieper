namespace BeerRecieper.Api.Features.MaltPlans.Contracts.Requests;

public record CreateMaltPlanRequest(double TotalWeightKg);
public record UpdateMaltPlanWeightRequest(double TotalWeightKg);
public record AddMaltRequest(string MaltName, double RelativeAmount, double MinEbc, double MaxEbc);
public record UpdateMaltAmountRequest(double RelativeAmount);