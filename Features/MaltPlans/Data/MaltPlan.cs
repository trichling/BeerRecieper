namespace Lab.BeerRecieper.Features.MaltPlans.Data;

public class MaltPlan
{
    private readonly List<Malt> _malts = new();
    public Guid Id { get; }
    public double TotalWeightKg { get; private set; }
    public IReadOnlyList<Malt> Malts => _malts;

    public MaltPlan(double totalWeightKg)
    {
        Id = Guid.NewGuid();
        TotalWeightKg = totalWeightKg;
    }

    public void AddMalt(string name, double relativeAmount)
    {
        if (relativeAmount <= 0)
            throw new ArgumentException("Relative amount must be greater than zero", nameof(relativeAmount));

        _malts.Add(new Malt(name, relativeAmount));
    }

    public void RemoveMalt(string name)
    {
        _malts.RemoveAll(m => m.Name == name);
    }

    public void UpdateMaltAmount(string name, double newRelativeAmount)
    {
        if (newRelativeAmount <= 0)
            throw new ArgumentException("Relative amount must be greater than zero", nameof(newRelativeAmount));

        var maltIndex = _malts.FindIndex(m => m.Name == name);
        if (maltIndex == -1)
            throw new InvalidOperationException($"Malt '{name}' not found");

        _malts[maltIndex] = new Malt(name, newRelativeAmount);
    }

    public double GetMaltWeightKg(string name)
    {
        var malt = _malts.FirstOrDefault(m => m.Name == name)
            ?? throw new InvalidOperationException($"Malt '{name}' not found");

        var totalRelativeAmount = _malts.Sum(m => m.RelativeAmount);
        return TotalWeightKg * (malt.RelativeAmount / totalRelativeAmount);
    }

    public void UpdateTotalWeight(double newTotalWeightKg)
    {
        if (newTotalWeightKg <= 0)
            throw new ArgumentException("Total weight must be greater than zero", nameof(newTotalWeightKg));

        TotalWeightKg = newTotalWeightKg;
    }
}