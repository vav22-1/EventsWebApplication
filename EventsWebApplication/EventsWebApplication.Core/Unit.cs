public struct Unit { }

public static class UnitExtensions
{
    public static Task<Unit> TaskFromResult()
    {
        return Task.FromResult(new Unit());
    }
}
