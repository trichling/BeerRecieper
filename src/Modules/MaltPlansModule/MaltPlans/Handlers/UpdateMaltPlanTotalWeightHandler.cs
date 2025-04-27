using Common;
using MaltPlans.Contracts;
using MaltPlans.Contracts.Commands;
using MaltPlans.Data;

namespace MaltPlans.Handlers;

public class UpdateMaltPlanTotalWeightHandler : IHandler<UpdateMaltPlanTotalWeightCommand, MaltPlanResponse?>
{
    private readonly IMaltPlanRepository _repository;

    public UpdateMaltPlanTotalWeightHandler(IMaltPlanRepository repository)
    {
        _repository = repository;
    }

    public async Task<MaltPlanResponse?> HandleAsync(UpdateMaltPlanTotalWeightCommand command)
    {
        var maltPlan = await _repository.GetByIdAsync(command.Id);
        if (maltPlan is null)
            return null;

        maltPlan.UpdateTotalWeight(command.TotalWeightKg);
        await _repository.UpdateAsync(maltPlan);
        return MaltPlanMapper.ToResponse(maltPlan);
    }
}