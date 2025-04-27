using Common;
using MaltPlans.Contracts;
using MaltPlans.Contracts.Commands;
using MaltPlans.Data;

namespace MaltPlans.Handlers;

public class RemoveMaltHandler : IHandler<RemoveMaltCommand, MaltPlanResponse?>
{
    private readonly IMaltPlanRepository _repository;

    public RemoveMaltHandler(IMaltPlanRepository repository)
    {
        _repository = repository;
    }

    public async Task<MaltPlanResponse?> HandleAsync(RemoveMaltCommand command)
    {
        var maltPlan = await _repository.GetByIdAsync(command.Id);
        if (maltPlan is null)
            return null;

        maltPlan.RemoveMalt(command.MaltName);
        await _repository.UpdateAsync(maltPlan);
        return MaltPlanMapper.ToResponse(maltPlan);
    }
}