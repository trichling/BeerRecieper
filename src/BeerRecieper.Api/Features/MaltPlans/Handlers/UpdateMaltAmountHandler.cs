using BeerRecieper.Api.Features.Common;
using BeerRecieper.Api.Features.MaltPlans.Contracts;
using BeerRecieper.Api.Features.MaltPlans.Contracts.Commands;
using BeerRecieper.Api.Features.MaltPlans.Data;

namespace BeerRecieper.Api.Features.MaltPlans.Handlers;

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