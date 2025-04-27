using MaltPlans.Contracts.Requests;

namespace MaltPlans.Contracts.Api;

interface IMaltPlanHttpApi
{
    Task<IEnumerable<MaltPlanResponse>> GetAllMaltPlansAsync();
    Task<MaltPlanResponse> GetMaltPlanByIdAsync(Guid id);
    Task<MaltPlanResponse> CreateMaltPlanAsync(CreateMaltPlanRequest request);
    Task<MaltPlanResponse> UpdateMaltPlanWeightAsync(Guid id, UpdateMaltPlanWeightRequest request);
    Task DeleteMaltPlanAsync(Guid id);
}