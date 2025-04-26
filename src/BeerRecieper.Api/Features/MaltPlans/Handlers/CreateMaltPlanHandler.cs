using BeerRecieper.Api.Features.Common;
using BeerRecieper.Api.Features.MaltPlans.Contracts;
using BeerRecieper.Api.Features.MaltPlans.Contracts.Commands;
using BeerRecieper.Api.Features.MaltPlans.Data;

namespace BeerRecieper.Api.Features.MaltPlans.Handlers;

public class CreateMaltPlanHandler : IHandler<CreateMaltPlanCommand, MaltPlanResponse>
{
    private readonly IMaltPlanRepository _repository;

    public CreateMaltPlanHandler(IMaltPlanRepository repository)
    {
        _repository = repository;
    }

    public async Task<MaltPlanResponse> HandleAsync(CreateMaltPlanCommand command)
    {
        var maltPlan = new MaltPlan(command.TotalWeightKg);
        await _repository.AddAsync(maltPlan);
        return MaltPlanMapper.ToResponse(maltPlan);
    }
}