using System.Diagnostics.CodeAnalysis;

namespace MusicNewsWatcher.Desktop.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<TSource> ExceptByProperty<TSource, TProperty>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TProperty> keySelector)
    {
        return first.ExceptBy(second, x => x, GenericComparer<TSource, TProperty>.Comparer(keySelector));
    }

}

public sealed class GenericComparer<T, TProperty> : IEqualityComparer<T>
{
    public static IEqualityComparer<T> Comparer(Func<T, TProperty> selector)
    {
        return new GenericComparer<T, TProperty>(selector);
    }

    private readonly Func<T, TProperty> selector;

    public GenericComparer(Func<T, TProperty> selector)
    {
        this.selector = selector;
    }

    public bool Equals(T? x, T? y)
    {
        if (x == null || y == null) return false;

        return Equals(selector(x), selector(y));
    }

    public int GetHashCode([DisallowNull] T obj)
    {
        object? value = selector(obj);

        if (value == null) return obj.GetHashCode();

        return value.GetHashCode();
    }
}