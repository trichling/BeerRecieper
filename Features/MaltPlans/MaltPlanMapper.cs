using Lab.BeerRecieper.Features.MaltPlans.Contracts;
using Lab.BeerRecieper.Features.MaltPlans.Data;

namespace Lab.BeerRecieper.Features.MaltPlans;

internal static class MaltPlanMapper
{
    public static MaltPlanResponse ToResponse(MaltPlan maltPlan)
        => new(
            maltPlan.Id,
            maltPlan.TotalWeightKg,
            maltPlan.GetMaltPlanAverageEbc(),
            maltPlan.Malts.Select(m => new MaltResponse(
                m.Name,
                m.RelativeAmount,
                maltPlan.GetMaltWeightKg(m.Name),
                m.MinEbc,
                m.MaxEbc,
                m.AverageEbc)).ToList());

    public static IEnumerable<MaltPlanResponse> ToResponse(IEnumerable<MaltPlan> maltPlans)
        => maltPlans.Select(ToResponse);

    public static MaltWeightResponse ToMaltWeightResponse(string maltName, double weightKg)
        => new(maltName, weightKg);
}