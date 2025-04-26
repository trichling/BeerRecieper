using BeerRecieper.Api.Features.Common;
using BeerRecieper.Api.Features.MaltPlans.Contracts;
using BeerRecieper.Api.Features.MaltPlans.Data;

namespace BeerRecieper.Api.Features.MaltPlans.Handlers;

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