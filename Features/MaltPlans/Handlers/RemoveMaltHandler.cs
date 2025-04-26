using Lab.BeerRecieper.Features.Common;
using Lab.BeerRecieper.Features.MaltPlans.Contracts;
using Lab.BeerRecieper.Features.MaltPlans.Contracts.Commands;
using Lab.BeerRecieper.Features.MaltPlans.Data;

namespace Lab.BeerRecieper.Features.MaltPlans.Handlers;

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