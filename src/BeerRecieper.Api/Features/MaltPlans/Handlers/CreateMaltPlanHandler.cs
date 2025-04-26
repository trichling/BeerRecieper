using Lab.BeerRecieper.Features.Common;
using Lab.BeerRecieper.Features.MaltPlans.Contracts;
using Lab.BeerRecieper.Features.MaltPlans.Contracts.Commands;
using Lab.BeerRecieper.Features.MaltPlans.Data;

namespace Lab.BeerRecieper.Features.MaltPlans.Handlers;

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