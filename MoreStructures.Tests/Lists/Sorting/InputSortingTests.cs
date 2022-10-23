using MoreStructures.Lists.Sorting;

namespace MoreStructures.Tests.Lists.Sorting;

public abstract class InputSortingTests
{
    protected Func<IInputMutatingSort> Sorter { get; }

    protected InputSortingTests(Func<IInputMutatingSort> sorter)
    {
        Sorter = sorter;
    }

    [TestMethod]
    public void Sort_PlaceItemsInOrder()
    {
        var sorter = Sorter();
        var numbers = Enumerable.Range(0, 8).ToList();
        foreach (var permutation in TestUtilities.GeneratePermutations(numbers))
        {
            sorter.Sort(permutation);
            Assert.AreEqual(permutation.Count, numbers.Count);
            Assert.IsTrue(permutation.IsSorted(), $"Not sorted: [{string.Join(", ", permutation)}]");
        }
    }

    [TestMethod]
    public void Sort_WorksWithDuplicates()
    {
        var sorter = Sorter();
        var numbers = new[] { 1, 2, 2, 3, 3, 3, 4 };
        foreach (var permutation in TestUtilities.GeneratePermutations(numbers))
        {
            sorter.Sort(permutation);
            Assert.AreEqual(permutation.Count, numbers.Length);
            Assert.IsTrue(permutation.IsSorted());
        }
    }

    [TestMethod]
    public void Sort_WorksWithEmptyList()
    {
        var sorter = Sorter();
        var list = new List<int> { };
        sorter.Sort(list);
        Assert.AreEqual(0, list.Count);
    }

    [TestMethod]
    public void Sort_WorksWithSingletonList()
    {
        var sorter = Sorter();
        var list = new List<int> { 496 };
        sorter.Sort(list);
        Assert.AreEqual(1, list.Count);
        Assert.AreEqual(496, list[0]);
    }

    [TestMethod]
    public void Sort_WorksWithReferenceTypes()
    {
        var s1 = "s1";
        var s2 = "s2";
        var list = new RefType1[] { new(3, s1), new(2, s1), new(4, s2), new(5, s1), new(2, s1) };
        var sorter = Sorter();
        sorter.Sort(list);
        Assert.IsTrue(list.IsSorted());
    }

    [TestMethod]
    public void Sort_WorksWithValueTypes()
    {
        var s1 = "s1";
        var s2 = "s2";
        var list = new ValType1[] { new(3, s1), new(2, s1), new(4, s2), new(5, s1), new(2, s1) };
        var sorter = Sorter();
        sorter.Sort(list);
        Assert.IsTrue(list.IsSorted());
    }

    [TestMethod]
    public void Sort_TakesIntoAccountComparerWithReferenceTypes()
    {
        var s1 = "o1";
        var s2 = "o2";
        var list = new RefType1[] { new(3, s1), new(2, s1), new(4, s2), new(5, s1), new(2, s1) };
        var sorter = Sorter();

        var comparer1 = new Field1BasedRefType1Comparer();
        sorter.Sort(list, comparer1);
        Assert.IsTrue(list.IsSorted(comparer1));

        var comparer2 = new Field2BasedRefType1Comparer();
        sorter.Sort(list, comparer2);
        Assert.IsTrue(list.IsSorted(comparer2));
    }

    [TestMethod]
    public void Sort_TakesIntoAccountComparerWithValueTypes()
    {
        var list = new ValType1[] { new(3, "a"), new(2, "b"), new(4, "a"), new(5, "c"), new(2, "d") };
        var sorter = Sorter();

        var comparer1 = new Field1BasedValType1Comparer();
        sorter.Sort(list, comparer1);
        Assert.IsTrue(list.IsSorted(comparer1));

        var comparer2 = new Field2BasedValType1Comparer();
        sorter.Sort(list, comparer2);
        Assert.IsTrue(list.IsSorted(comparer2));
    }

    private sealed record RefType1(int Field1, string Field2) : IComparable<RefType1>
    {
        public int CompareTo(RefType1? other) => other != null ? Field1 - other.Field1 : 1;
    }

    private record struct ValType1(int Field1, string Field2) : IComparable<ValType1>
    {
        public int CompareTo(ValType1 other) => Field1 - other.Field1;
    }

    private sealed class Field1BasedRefType1Comparer : IComparer<RefType1>
    {
        public int Compare(RefType1? x, RefType1? y)
        {
            if (x == null) return y == null ? 0 : -1;
            if (y == null) return 1;
            return x.Field1 - y.Field1;
        }
    }

    private sealed class Field2BasedRefType1Comparer : IComparer<RefType1>
    {
        public int Compare(RefType1? x, RefType1? y) => StringComparer.InvariantCulture.Compare(x?.Field2, y?.Field2);
    }

    private sealed class Field1BasedValType1Comparer : IComparer<ValType1>
    {
        public int Compare(ValType1 x, ValType1 y) => x.Field1 - y.Field1;
    }

    private sealed class Field2BasedValType1Comparer : IComparer<ValType1>
    {
        public int Compare(ValType1 x, ValType1 y) => StringComparer.InvariantCulture.Compare(x.Field2, x.Field2);
    }
}
