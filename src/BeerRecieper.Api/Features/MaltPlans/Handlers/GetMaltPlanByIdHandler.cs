using Lab.BeerRecieper.Features.Common;
using Lab.BeerRecieper.Features.MaltPlans.Contracts;
using Lab.BeerRecieper.Features.MaltPlans.Contracts.Commands;
using Lab.BeerRecieper.Features.MaltPlans.Data;

namespace Lab.BeerRecieper.Features.MaltPlans.Handlers;

public class GetMaltPlanByIdHandler : IHandler<GetMaltPlanByIdCommand, MaltPlanResponse?>
{
    private readonly IMaltPlanRepository _repository;

    public GetMaltPlanByIdHandler(IMaltPlanRepository repository)
    {
        _repository = repository;
    }

    public async Task<MaltPlanResponse?> HandleAsync(GetMaltPlanByIdCommand command)
    {
        var maltPlan = await _repository.GetByIdAsync(command.Id);
        return maltPlan is null ? null : MaltPlanMapper.ToResponse(maltPlan);
    }
}