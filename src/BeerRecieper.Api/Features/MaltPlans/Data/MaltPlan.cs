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

    public void AddMalt(string name, double relativeAmount, double minEbc, double maxEbc)
    {
        if (relativeAmount <= 0)
            throw new ArgumentException("Relative amount must be greater than zero", nameof(relativeAmount));
        if (minEbc < 0)
            throw new ArgumentException("Minimum EBC must be non-negative", nameof(minEbc));
        if (maxEbc < minEbc)
            throw new ArgumentException("Maximum EBC must be greater than or equal to minimum EBC", nameof(maxEbc));

        var index = _malts.FindIndex(m => m.Name == name);
        if (index != -1)
        {
            _malts[index] = _malts[index] with { RelativeAmount = _malts[index].RelativeAmount + relativeAmount };
        }
        else
        {
            _malts.Add(new Malt(name, relativeAmount, minEbc, maxEbc));
        }
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

        _malts[maltIndex] = _malts[maltIndex] with { RelativeAmount = newRelativeAmount };
    }

    public double GetMaltWeightKg(string name)
    {
        var malt = _malts.FirstOrDefault(m => m.Name == name)
            ?? throw new InvalidOperationException($"Malt '{name}' not found");

        var totalRelativeAmount = _malts.Sum(m => m.RelativeAmount);
        return TotalWeightKg * (malt.RelativeAmount / totalRelativeAmount);
    }

    public double GetMaltPlanAverageEbc()
    {
        if (!_malts.Any())
            return 0;

        var totalRelativeAmount = _malts.Sum(m => m.RelativeAmount);
        return _malts.Sum(m => (m.RelativeAmount / totalRelativeAmount) * m.AverageEbc);
    }

    public void UpdateTotalWeight(double newTotalWeightKg)
    {
        if (newTotalWeightKg <= 0)
            throw new ArgumentException("Total weight must be greater than zero", nameof(newTotalWeightKg));

        TotalWeightKg = newTotalWeightKg;
    }
}