using MoreStructures.Utilities;

namespace MoreStructures.Lists.Searching;

internal class SearchHelperMethods
{
    public static int ValidateIndexesAndGetLength<T>(IEnumerable<T> source, int? fromIndex, int? toIndex)
    {
        var length = source.CountO1();
        if (fromIndex != null && (fromIndex < 0 || fromIndex > length - 1))
            throw new ArgumentOutOfRangeException(
                nameof(fromIndex), $"Must be within the range of valid indexes for {source}.");
        if (toIndex != null && (toIndex < 0 || toIndex > length - 1))
            throw new ArgumentOutOfRangeException(
                nameof(toIndex), $"Must be within the range of valid indexes for {source}.");
        return length;
    }

}
