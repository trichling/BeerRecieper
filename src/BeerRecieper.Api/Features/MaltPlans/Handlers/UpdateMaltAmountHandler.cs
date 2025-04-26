using Lab.BeerRecieper.Features.Common;
using Lab.BeerRecieper.Features.MaltPlans.Contracts;
using Lab.BeerRecieper.Features.MaltPlans.Contracts.Commands;
using Lab.BeerRecieper.Features.MaltPlans.Data;

namespace Lab.BeerRecieper.Features.MaltPlans.Handlers;

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