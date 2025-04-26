namespace BeerRecieper.Api.Features.Common;

public record Unit
{
    private Unit() { }
    public static Unit Value => new();
}