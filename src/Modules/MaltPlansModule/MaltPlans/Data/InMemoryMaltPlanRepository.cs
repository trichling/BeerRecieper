namespace MaltPlans.Data;

public class InMemoryMaltPlanRepository : IMaltPlanRepository
{
    private readonly Dictionary<Guid, MaltPlan> _maltPlans = GetMaltPlans();

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

    private static Dictionary<Guid, MaltPlan> GetMaltPlans() =>
        new Dictionary<Guid, MaltPlan>
        {
            { Guid.Parse("f1c29f6d-679d-4176-8c69-42591e5d3b89"), new MaltPlan(20.0) },
            { Guid.Parse("a2b5e9c8-7d34-4f12-b12e-890abc123def"), new MaltPlan(25.0) },
        };
}
