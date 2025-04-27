namespace Common;

public record Unit
{
    private Unit() { }
    public static Unit Value => new();
}