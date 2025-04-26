using BeerRecieper.Api.Features.Common;
using BeerRecieper.Api.Features.MaltPlans.Contracts;
using BeerRecieper.Api.Features.MaltPlans.Contracts.Commands;
using BeerRecieper.Api.Features.MaltPlans.Data;

namespace BeerRecieper.Api.Features.MaltPlans.Handlers;

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