namespace MaltPlans.Data;

public interface IMaltPlanRepository
{
    Task<MaltPlan?> GetByIdAsync(Guid id);
    Task<IEnumerable<MaltPlan>> GetAllAsync();
    Task AddAsync(MaltPlan maltPlan);
    Task UpdateAsync(MaltPlan maltPlan);
}