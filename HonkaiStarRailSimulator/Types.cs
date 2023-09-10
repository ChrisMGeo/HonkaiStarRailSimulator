using System.Numerics;
namespace HonkaiStarRailSimulator;

public abstract record Option<T>
{
    public record None : Option<T>;

    public record Some(T Value) : Option<T>;

    public static explicit operator Option<T>(T obj) => new Some(obj);
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