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

public interface IFinity<T> : IComparable<IFinity<T>> where T : INumber<T>
{
    TResult Match<TResult>(Func<T, TResult> onFinite, Func<TResult> onInfinite);
    void Match(Action<T> onFinite, Action onInfinite);
    IOption<TResult> MapFinite<TResult>(Func<T, TResult> f);
    IOption<TResult> MapInfinite<TResult>(Func<TResult> f);
    IFinity<T> GetGreater(IFinity<T> other) => CompareTo(other) > 0 ? this : other;
}

public class Finite<T>:IFinity<T> where T : INumber<T>
{
    private T _data;

    private Finite(T data)
    {
        _data = data;
    }

    public static IFinity<T> Of(T data) => new Finite<T>(data);

    public TResult Match<TResult>(Func<T, TResult> onFinite, Func<TResult> onInfinite) => onFinite(_data);

    public void Match(Action<T> onFinite, Action onInfinite) => onFinite(_data);

    public IOption<TResult> MapFinite<TResult>(Func<T, TResult> f) => Some<TResult>.Of(f(_data));

    public IOption<TResult> MapInfinite<TResult>(Func<TResult> f) => new None<TResult>();
    public int CompareTo(IFinity<T>? other) => other?.Match(
        onInfinite: () => -1,
        onFinite: (otherData)=>_data.CompareTo(otherData)
    ) ?? 1;
}

public class Infinite<T>:IFinity<T> where T: INumber<T>
{
    public int CompareTo(IFinity<T>? other) => other?.Match(
        onInfinite: () => 0,
        onFinite: _ => 1
    ) ?? 1;

    public TResult Match<TResult>(Func<T, TResult> onFinite, Func<TResult> onInfinite) => onInfinite();

    public void Match(Action<T> onFinite, Action onInfinite) => onInfinite();

    public IOption<TResult> MapFinite<TResult>(Func<T, TResult> f) => new None<TResult>();

    public IOption<TResult> MapInfinite<TResult>(Func<TResult> f) => Some<TResult>.Of(f());
}