namespace BeerRecieper.Api.Features.MaltPlans.Data;

public record Malt(string Name, double RelativeAmount, double MinEbc, double MaxEbc)
{
    public double AverageEbc => (MinEbc + MaxEbc) / 2;
}