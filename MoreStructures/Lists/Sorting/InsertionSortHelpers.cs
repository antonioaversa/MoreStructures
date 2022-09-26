namespace MoreStructures.Lists.Sorting;

internal static class InsertionSortHelpers
{
    public static void InsertionSortOnHthIndexes<T>(IList<T> list, IComparer<T> comparer, int gap)
    {
        for (var i = gap; i < list.Count; i++)
        {
            var ithItem = list[i];

            int j;
            for (j = i; j >= gap && comparer.Compare(ithItem, list[j - gap]) < 0; j -= gap)
                list[j] = list[j - gap];

            list[j] = ithItem;
        }
    }
}
