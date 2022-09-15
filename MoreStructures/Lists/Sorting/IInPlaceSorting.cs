namespace MoreStructures.Lists.Sorting;

/// <summary>
/// An algorithm sorting the given list in place, i.e. directly modifying the list given in input and using such list
/// as "workspace", rather than generating a brand new list sorted.
/// </summary>
public interface IInPlaceSorting
{
    /// <inheritdoc cref="Sort{T}(IList{T}, IComparer{T})" path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    /// Sorts the provided <see cref="IList{T}"/> in place.
    /// <br/>
    /// Uses the default comparer of instances of type <typeparamref name="T"/> to compare items.
    /// </summary>
    void Sort<T>(IList<T> list) where T: IComparable<T>;

    /// <summary>
    /// Sorts the provided <see cref="IList{T}"/> in place. 
    /// <br/>
    /// Uses the provided <paramref name="comparer"/> of instances of type <typeparamref name="T"/> to compare items.
    /// </summary>
    /// <typeparam name="T">
    /// The type of items of the <paramref name="list"/>. Unlike in <see cref="Sort{T}(IList{T})"/>, it 
    /// doesn't necessarily have to support <see cref="IComparable{T}"/>, as an external <see cref="IComparer{T}"/> is 
    /// provided.
    /// </typeparam>
    /// <param name="list">The list to be sorted.</param>
    /// <param name="comparer">
    /// The comparer of instances of <typeparamref name="T"/>, to be used for comparisons (if necessary).
    /// </param>
    /// <remarks>
    /// Alternative to <see cref="Sort{T}(IList{T})"/> when <typeparamref name="T"/> doesn't support 
    /// <see cref="IComparable{T}"/> or when the sorting strategy to be used is not the default one.
    /// </remarks>
    void Sort<T>(IList<T> list, IComparer<T> comparer);
}
