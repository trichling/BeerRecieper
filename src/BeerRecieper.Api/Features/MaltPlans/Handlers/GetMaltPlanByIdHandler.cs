using BeerRecieper.Api.Features.Common;
using BeerRecieper.Api.Features.MaltPlans.Contracts;
using BeerRecieper.Api.Features.MaltPlans.Contracts.Commands;
using BeerRecieper.Api.Features.MaltPlans.Data;

namespace BeerRecieper.Api.Features.MaltPlans.Handlers;

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