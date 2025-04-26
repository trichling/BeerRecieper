namespace BeerRecieper.Api.Features.BeerRecipes.Data;

public class BeerRecipe
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Guid? MaltPlanId { get; private set; }

    public BeerRecipe(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        MaltPlanId = null;
    }

    internal void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }

    internal void SetMaltPlan(Guid maltPlanId)
    {
        MaltPlanId = maltPlanId;
    }

    internal void RemoveMaltPlan()
    {
        MaltPlanId = null;
    }
}