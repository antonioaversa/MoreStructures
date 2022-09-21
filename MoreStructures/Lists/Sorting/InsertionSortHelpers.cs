namespace MoreStructures.Lists.Sorting;

internal static class InsertionSortHelpers
{
    public static void InsertionSortOnHthIndexes<T>(IList<T> list, IComparer<T> comparer, int gap)
    {
        for (var i = gap; i < list.Count; i += gap)
            for (var j = i; j > 0 && comparer.Compare(list[j], list[j - gap]) < 0; j -= gap)
                (list[j], list[j - gap]) = (list[j - gap], list[j]);
    }
}
