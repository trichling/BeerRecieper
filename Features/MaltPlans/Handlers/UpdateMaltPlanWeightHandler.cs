using Lab.BeerRecieper.Features.Common;
using Lab.BeerRecieper.Features.MaltPlans.Contracts;
using Lab.BeerRecieper.Features.MaltPlans.Contracts.Commands;
using Lab.BeerRecieper.Features.MaltPlans.Data;

namespace Lab.BeerRecieper.Features.MaltPlans.Handlers;

public class UpdateMaltPlanWeightHandler : IHandler<UpdateMaltPlanWeightCommand, MaltPlanResponse?>
{
    private readonly IMaltPlanRepository _repository;

    public UpdateMaltPlanWeightHandler(IMaltPlanRepository repository)
    {
        _repository = repository;
    }

    public async Task<MaltPlanResponse?> HandleAsync(UpdateMaltPlanWeightCommand command)
    {
        var maltPlan = await _repository.GetByIdAsync(command.Id);
        if (maltPlan is null)
            return null;

        maltPlan.UpdateTotalWeight(command.TotalWeightKg);
        await _repository.UpdateAsync(maltPlan);
        return MaltPlanMapper.ToResponse(maltPlan);
    }
}