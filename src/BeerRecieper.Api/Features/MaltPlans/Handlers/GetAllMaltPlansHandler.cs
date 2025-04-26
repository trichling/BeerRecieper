using Lab.BeerRecieper.Features.Common;
using Lab.BeerRecieper.Features.MaltPlans.Contracts;
using Lab.BeerRecieper.Features.MaltPlans.Data;

namespace Lab.BeerRecieper.Features.MaltPlans.Handlers;

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