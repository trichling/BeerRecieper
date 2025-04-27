using Common;
using MaltPlans.Contracts;
using MaltPlans.Contracts.Commands;
using MaltPlans.Data;

namespace MaltPlans.Handlers;

public class UpdateMaltAmountHandler : IHandler<UpdateMaltAmountCommand, MaltPlanResponse?>
{
    private readonly IMaltPlanRepository _repository;

    public UpdateMaltAmountHandler(IMaltPlanRepository repository)
    {
        _repository = repository;
    }

    public async Task<MaltPlanResponse?> HandleAsync(UpdateMaltAmountCommand command)
    {
        var maltPlan = await _repository.GetByIdAsync(command.Id);
        if (maltPlan is null)
            return null;

        maltPlan.UpdateMaltAmount(command.MaltName, command.RelativeAmount);
        await _repository.UpdateAsync(maltPlan);
        return MaltPlanMapper.ToResponse(maltPlan);
    }
}