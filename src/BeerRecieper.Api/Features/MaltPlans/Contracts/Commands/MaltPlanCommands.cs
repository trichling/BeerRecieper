namespace BeerRecieper.Api.Features.MaltPlans.Contracts.Commands;

public record CreateMaltPlanCommand(double TotalWeightKg);
public record UpdateMaltPlanTotalWeightCommand(Guid Id, double TotalWeightKg);
public record AddMaltCommand(Guid Id, string MaltName, double RelativeAmount, double MinEbc, double MaxEbc);
public record RemoveMaltCommand(Guid Id, string MaltName);
public record UpdateMaltAmountCommand(Guid Id, string MaltName, double RelativeAmount);
public record GetMaltPlanByIdCommand(Guid Id);