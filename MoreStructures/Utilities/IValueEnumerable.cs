namespace MoreStructures.Utilities;

/// <inheritdoc path="//*[not(self::summary)]"/>
/// <summary>
/// An <see cref="IEnumerable{T}"/> which is compared by value, by using 
/// <see cref="Enumerable.SequenceEqual{TSource}(IEnumerable{TSource}, IEnumerable{TSource})"/>. 
/// To be used to be used as interface for enumerables in records and other value structures.
/// </summary>
public interface IValueEnumerable<out T> : IEnumerable<T>
{
}
