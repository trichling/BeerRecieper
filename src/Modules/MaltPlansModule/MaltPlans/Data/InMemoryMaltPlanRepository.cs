namespace MaltPlans.Data;

public class InMemoryMaltPlanRepository : IMaltPlanRepository
{
    private readonly Dictionary<Guid, MaltPlan> _maltPlans = new();

    public Task<IEnumerable<MaltPlan>> GetAllAsync()
    {
        return Task.FromResult(_maltPlans.Values.AsEnumerable());
    }

    public Task<MaltPlan?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_maltPlans.GetValueOrDefault(id));
    }

    public Task AddAsync(MaltPlan maltPlan)
    {
        _maltPlans.Add(maltPlan.Id, maltPlan);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(MaltPlan maltPlan)
    {
        _maltPlans[maltPlan.Id] = maltPlan;
        return Task.CompletedTask;
    }
}