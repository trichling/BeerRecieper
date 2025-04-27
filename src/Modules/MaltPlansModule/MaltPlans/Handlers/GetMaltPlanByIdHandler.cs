using Common;
using MaltPlans.Contracts;
using MaltPlans.Contracts.Commands;
using MaltPlans.Data;

namespace MaltPlans.Handlers;

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