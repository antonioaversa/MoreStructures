namespace MoreStructures.Lists.Sorting.QuickSort;

/// <summary>
/// A <see cref="IShuffleStrategy"/> which does nothing to its input, and returns it as it is.
/// </summary>
/// <remarks>
/// Useful when used in a <see cref="IThreeWayPartitionStrategy"/> of a <see cref="RecursiveQuickSort"/> which should
/// act deterministically.
/// </remarks>
public class IdentityShuffleStrategy : IShuffleStrategy
{
    /// <inheritdoc/>
    /// <remarks>
    /// Does nothing to the <paramref name="list"/>: it returns it as it is.
    /// </remarks>
    public void Shuffle<T>(IList<T> list, int start, int end)
    {
    }
}
