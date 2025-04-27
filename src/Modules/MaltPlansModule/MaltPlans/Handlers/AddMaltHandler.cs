using Common;
using MaltPlans.Contracts;
using MaltPlans.Contracts.Commands;
using MaltPlans.Data;

namespace MaltPlans.Handlers;

public class AddMaltHandler : IHandler<AddMaltCommand, MaltPlanResponse?>
{
    private readonly IMaltPlanRepository _repository;

    public AddMaltHandler(IMaltPlanRepository repository)
    {
        _repository = repository;
    }

    public async Task<MaltPlanResponse?> HandleAsync(AddMaltCommand command)
    {
        var maltPlan = await _repository.GetByIdAsync(command.Id);
        if (maltPlan is null)
            return null;

        maltPlan.AddMalt(command.MaltName, command.RelativeAmount, command.MinEbc, command.MaxEbc);
        await _repository.UpdateAsync(maltPlan);
        return MaltPlanMapper.ToResponse(maltPlan);
    }
}