using Common;
using MaltPlans.Contracts;
using MaltPlans.Data;

namespace MaltPlans.Handlers;

public class GetAllMaltPlansHandler : IHandler<Unit, IEnumerable<MaltPlanResponse>>
{
    private readonly IMaltPlanRepository _repository;

    public GetAllMaltPlansHandler(IMaltPlanRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<MaltPlanResponse>> HandleAsync(Unit request)
    {
        var maltPlans = await _repository.GetAllAsync();
        return MaltPlanMapper.ToResponse(maltPlans);
    }
}