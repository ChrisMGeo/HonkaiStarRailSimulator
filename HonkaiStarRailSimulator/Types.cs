using System.Numerics;

namespace HonkaiStarRailSimulator;

public interface IOption<T>
{
    TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone);
    void Match(Action<T> onSome, Action onNone);
    IOption<TResult> Bind<TResult>(Func<T, IOption<TResult>> f);
    IOption<TResult> Map<TResult>(Func<T, TResult> f);
    T Or(T aDefault);
}

public class Some<T> : IOption<T>
{
    private T _data;

    private Some(T data)
    {
        _data = data;
    }

    public static IOption<T> Of(T data) => new Some<T>(data);

    public TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> _) =>
        onSome(_data);

    public void Match(Action<T> onSome, Action onNone) =>
        onSome(_data);

    public IOption<TResult> Bind<TResult>(Func<T, IOption<TResult>> f) => f(_data);

    public IOption<TResult> Map<TResult>(Func<T, TResult> f) => new Some<TResult>(f(_data));

    public T Or(T _) => _data;
}

public class None<T> : IOption<T>
{
    public TResult Match<TResult>(Func<T, TResult> _, Func<TResult> onNone) =>
        onNone();
    public void Match(Action<T> _, Action onNone) =>
        onNone();

    public IOption<TResult> Bind<TResult>(Func<T, IOption<TResult>> f) => new None<TResult>();

    public IOption<TResult> Map<TResult>(Func<T, TResult> f) => new None<TResult>();

    public T Or(T aDefault) => aDefault;
}

public abstract record Finity<T> : IComparable where T : INumber<T>
{
    public record Infinite : Finity<T>;

    public record Finite(T Value) : Finity<T>;

    public int CompareTo(object? obj)
    {
        return obj switch
        {
            null => 1,
            Finity<T> otherFinity when this is Finite thisFinite && otherFinity is Finite otherFinite => thisFinite
                .Value.CompareTo(otherFinite.Value),
            Finity<T> otherFinity => (this is Infinite ? 1 : 0) - (otherFinity is Infinite ? 1 : 0),
            _ => throw new ArgumentException("Object is not a Finity")
        };
    }

    public Finity<T> GetGreater(Finity<T> other)
    {
        return CompareTo(other) > 0 ? this : other;
    }
}