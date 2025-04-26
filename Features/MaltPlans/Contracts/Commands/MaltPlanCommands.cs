namespace Lab.BeerRecieper.Features.MaltPlans.Contracts.Commands;

public record CreateMaltPlanCommand(double TotalWeightKg);
public record UpdateMaltPlanWeightCommand(Guid Id, double TotalWeightKg);
public record AddMaltCommand(Guid Id, string MaltName, double RelativeAmount);
public record RemoveMaltCommand(Guid Id, string MaltName);
public record UpdateMaltAmountCommand(Guid Id, string MaltName, double RelativeAmount);
public record GetMaltPlanByIdCommand(Guid Id);