using Common;
using MaltPlans.Contracts;
using MaltPlans.Contracts.Commands;
using MaltPlans.Data;

namespace MaltPlans.Handlers;

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