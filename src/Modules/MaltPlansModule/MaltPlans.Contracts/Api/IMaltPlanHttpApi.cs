using MaltPlans.Contracts.Requests;
using RestEase;

namespace MaltPlans.Contracts.Api;

public interface IMaltPlanHttpApi
{
    [Get("/maltplans")]
    Task<IEnumerable<MaltPlanResponse>> GetAllMaltPlansAsync();
    [Get("/maltplans/{id}")]
    Task<MaltPlanResponse> GetMaltPlanByIdAsync(Guid id);
    [Post("/maltplans")]
    Task<MaltPlanResponse> CreateMaltPlanAsync(CreateMaltPlanRequest request);
    [Put("/maltplans/{id}")]
    Task<MaltPlanResponse> UpdateMaltPlanWeightAsync(Guid id, UpdateMaltPlanWeightRequest request);
    [Delete("/maltplans/{id}")]
    Task DeleteMaltPlanAsync(Guid id);
}