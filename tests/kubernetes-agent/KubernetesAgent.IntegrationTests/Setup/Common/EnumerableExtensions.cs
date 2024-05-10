namespace KubernetesAgent.Integration.Setup.Common;

public static class EnumerableExtensions
{
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> items)
        where T : class
        => items.Where(i => i != null)!;
}