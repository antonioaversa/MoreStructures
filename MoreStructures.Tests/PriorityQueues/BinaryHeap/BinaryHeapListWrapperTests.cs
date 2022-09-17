using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.PriorityQueues.BinaryHeap;

namespace MoreStructures.Tests.PriorityQueues.BinaryHeap;

public abstract class BinaryHeapListWrapperTests
{
    private readonly IComparer<int> DC = Comparer<int>.Default;

    private bool StoreHeapAtTheEnd { get; }

    protected BinaryHeapListWrapperTests(bool storeHeapAtTheEnd)
    {
        StoreHeapAtTheEnd = storeHeapAtTheEnd;
    }

    private class AbsIntComparer : IComparer<int>
    {
        public int Compare(int x, int y) => Math.Abs(x) - Math.Abs(y);
    }

    private class InverseIntComparer : IComparer<int>
    {
        public int Compare(int x, int y) => y - x;
    }

    [TestMethod]
    public void Ctor_EnforcesHeapCountToBeAtMostListCount()
    {
        var list1 = new List<int> { 1, 2, 3 };
        Assert.ThrowsException<ArgumentException>(
            () => new BinaryHeapListWrapper<int>(list1, DC, -1, StoreHeapAtTheEnd));
        Assert.ThrowsException<ArgumentException>(
            () => new BinaryHeapListWrapper<int>(list1, DC, 4, StoreHeapAtTheEnd));
        Assert.ThrowsException<ArgumentException>(
            () => new BinaryHeapListWrapper<int>(list1, DC, 10, StoreHeapAtTheEnd));
        var list2 = Array.Empty<int>();
        Assert.ThrowsException<ArgumentException>(
            () => new BinaryHeapListWrapper<int>(list2, DC, 1, StoreHeapAtTheEnd));
    }

    [TestMethod]
    public void Ctor_RestoresHeapPropertyWithSpecifiedComparer()
    {
        var list1 = new List<int> { 1, 3, 5, -2, -4, -6 };
        var heap1 = new BinaryHeapListWrapper<int>(list1, new AbsIntComparer(), 6, StoreHeapAtTheEnd);
        Assert.IsTrue(heap1.PopAll().SequenceEqual(new[] { -6, 5, -4, 3, -2, 1 }));
        var heap2 = new BinaryHeapListWrapper<int>(list1, Comparer<int>.Default, 6, StoreHeapAtTheEnd);
        Assert.IsTrue(heap2.PopAll().SequenceEqual(new[] { 5, 3, 1, -2, -4, -6 }));
        var heap3 = new BinaryHeapListWrapper<int>(list1, new InverseIntComparer(), 6, StoreHeapAtTheEnd);
        Assert.IsTrue(heap3.PopAll().SequenceEqual(new[] { -6, -4, -2, 1, 3, 5 }));
    }

    [TestMethod]
    public void Ctor_WorksOnEmptyHeap()
    {
        var list1 = new List<int> { 1, 2, 3 };
        var heap1 = new BinaryHeapListWrapper<int>(list1, Comparer<int>.Default, 0, StoreHeapAtTheEnd);
        Assert.AreEqual(0, heap1.HeapCount);
        Assert.AreEqual(3, heap1.ListCount);
    }

    [TestMethod]
    public void Ctor_DoesntTouchItemsInBufferArea()
    {
        for (var numberOfItems = 0; numberOfItems < 10; numberOfItems++)
        {
            for (var numberOfHeapItems = 0; numberOfHeapItems <= numberOfItems; numberOfHeapItems++)
            {
                var numberOfBufferItems = numberOfItems - numberOfHeapItems;
                var list1 = Enumerable.Range(0, numberOfItems).ToList();
                if (StoreHeapAtTheEnd)
                {
                    var bufferItems = Enumerable.Range(0, numberOfBufferItems);
                    _ = new BinaryHeapListWrapper<int>(
                    list1, new InverseIntComparer(), numberOfHeapItems, StoreHeapAtTheEnd);
                    Assert.IsTrue(list1.Take(numberOfBufferItems).SequenceEqual(bufferItems));
                }
                else
                {
                    var bufferItems = Enumerable.Range(numberOfHeapItems, numberOfBufferItems);
                    _ = new BinaryHeapListWrapper<int>(
                    list1, Comparer<int>.Default, numberOfHeapItems, StoreHeapAtTheEnd);
                    Assert.IsTrue(list1.Skip(numberOfHeapItems).SequenceEqual(bufferItems));
                }
            }
        }
    }

    [TestMethod]
    public void RaiseEvents_AreInvoked()
    {
        var list1 = new List<int> { 1, 2, 3 };
        var raiseItemPushedInvoked = false;
        var raiseItemPoppingInvoked = false;
        var raiseItemsSwappedInvoked = false;
        var heap1 = new BinaryHeapListWrapper<int>(list1, Comparer<int>.Default, 0, StoreHeapAtTheEnd)
        {
            RaiseItemPushed = i => raiseItemPushedInvoked = true,
            RaiseItemPopping = (i, j) => raiseItemPoppingInvoked = true,
            RaiseItemsSwapped = (i, j) => raiseItemsSwappedInvoked = true,
        };

        Assert.IsFalse(raiseItemPushedInvoked);
        Assert.IsFalse(raiseItemPoppingInvoked);
        Assert.IsFalse(raiseItemsSwappedInvoked);
        heap1.Push(1);
        Assert.IsTrue(raiseItemPushedInvoked);
        Assert.IsFalse(raiseItemPoppingInvoked);
        Assert.IsFalse(raiseItemsSwappedInvoked);
        heap1.Pop();
        Assert.IsTrue(raiseItemPoppingInvoked);
        Assert.IsFalse(raiseItemsSwappedInvoked);
        heap1.Push(2);
        heap1.Push(1);
        Assert.IsFalse(raiseItemsSwappedInvoked);
        heap1.Push(3);
        Assert.IsTrue(raiseItemsSwappedInvoked);
    }

    [TestMethod]
    public void HeapCount_IsInstantiatedToTheSpecifiedValue()
    {
        var list1 = new List<int> { 1, 2, 3 };
        var heap1 = new BinaryHeapListWrapper<int>(list1, Comparer<int>.Default, 2, StoreHeapAtTheEnd);
        Assert.AreEqual(2, heap1.HeapCount);
    }

    [TestMethod]
    public void HeapCount_IsUpdatedOnPushAndPop()
    {
        var heap = new BinaryHeapListWrapper<int>(
            Enumerable.Repeat(0, 10).ToList(), Comparer<int>.Default, 0, StoreHeapAtTheEnd);
        Assert.AreEqual(0, heap.HeapCount);
        heap.Push(0);
        Assert.AreEqual(1, heap.HeapCount);
        for (var i = 0; i < 9; i++) heap.Push(i + 1);
        Assert.AreEqual(10, heap.HeapCount);
        heap.Pop();
        Assert.AreEqual(9, heap.HeapCount);
        for (var i = 0; i < 9; i++) heap.Pop();
        Assert.AreEqual(0, heap.HeapCount);
    }

    [TestMethod]
    public void HeapCount_IsIncreasedWhenPushMakesRoomInTheList()
    {
        var heap = new BinaryHeapListWrapper<int>(new List<int>(), Comparer<int>.Default, 0, StoreHeapAtTheEnd);
        Assert.AreEqual(0, heap.HeapCount);

        if (!StoreHeapAtTheEnd)
        {
            heap.Push(0);
            Assert.AreEqual(1, heap.HeapCount);
            for (var i = 0; i < 9; i++) heap.Push(i + 1);
            Assert.AreEqual(10, heap.HeapCount);
            heap.Pop();
            Assert.AreEqual(9, heap.HeapCount);
            for (var i = 0; i < 9; i++) heap.Pop();
            Assert.AreEqual(0, heap.HeapCount);
        }
        else
        {
            Assert.ThrowsException<InvalidOperationException>(() => heap.Push(0));
        }
    }

    [TestMethod]
    public void HeapCount_IsNotChangedByPeekOrPeekKth()
    {
        var list = Enumerable.Range(0, 10).ToArray();
        var listCount = list.Length;
        var listItems = list.ToHashSet();
        var heap = new BinaryHeapListWrapper<int>(list, Comparer<int>.Default, list.Length, StoreHeapAtTheEnd);
        for (var i = 0; i < list.Length; i++)
            Assert.AreEqual(list.Length - 1 - i, heap.PeekKth(i).result);
        Assert.AreEqual(heap.Peek(), heap.PeekKth(0).result);
        Assert.IsTrue(listItems.SetEquals(list));
        Assert.AreEqual(listCount, list.Length);
    }

    [TestMethod]
    public void Push_BeyondListCountUpdatesListCount_WhenHeapAtTheBeginning()
    {
        if (!StoreHeapAtTheEnd)
        { 
            var heap = new BinaryHeapListWrapper<int>(
                new List<int> { 1, 2, 3 }, Comparer<int>.Default, 2, StoreHeapAtTheEnd);
            Assert.AreEqual(2, heap.HeapCount);
            Assert.AreEqual(3, heap.ListCount);
            heap.Push(4);
            Assert.AreEqual(3, heap.HeapCount);
            Assert.AreEqual(3, heap.ListCount);
            heap.Push(5);
            Assert.AreEqual(4, heap.HeapCount);
            Assert.AreEqual(4, heap.ListCount);
        }
    }

    [TestMethod]
    public void Push_BeyondListCountRaisesException_WhenHeapAtTheEnd()
    {
        if (StoreHeapAtTheEnd)
        {
            var heap = new BinaryHeapListWrapper<int>(
                new List<int> { 1, 2, 3 }, Comparer<int>.Default, 2, StoreHeapAtTheEnd);
            Assert.AreEqual(2, heap.HeapCount);
            Assert.AreEqual(3, heap.ListCount);
            heap.Push(4);
            Assert.AreEqual(3, heap.HeapCount);
            Assert.AreEqual(3, heap.ListCount);
            Assert.ThrowsException<InvalidOperationException>(() => heap.Push(5));
        }
    }

    [TestMethod]
    public void Pop_DoesntLoseItems()
    {
        var numbers = Enumerable.Range(0, 5).Select(i => i - 3).ToList();
        var numbersSet = numbers.ToHashSet();
        foreach (var permutation in TestUtilities.GeneratePermutations(numbers))
        {
            var heap = new BinaryHeapListWrapper<int>(
                permutation, new AbsIntComparer(), permutation.Count, StoreHeapAtTheEnd);
            for (var i = 0; i < permutation.Count / 2; i++)
                heap.Pop();
            Assert.IsTrue(numbersSet.SetEquals(permutation));
        }
    }

    [TestMethod]
    public void Clear_WipesAllItemsOutWithNonReadOnlyLists()
    {
        var list = new List<int> { 1, 2, 3 };
        var heap = new BinaryHeapListWrapper<int>(list, Comparer<int>.Default, 2, StoreHeapAtTheEnd);
        heap.Clear();
        Assert.AreEqual(0, heap.HeapCount);
        Assert.AreEqual(0, heap.ListCount);
    }

    [TestMethod]
    public void Clear_JustResetsHeapCountWithReadOnlyLists()
    {
        var list = new[] { 1, 2, 3 };
        var heap = new BinaryHeapListWrapper<int>(list, Comparer<int>.Default, 2, StoreHeapAtTheEnd);
        heap.Clear();
        Assert.AreEqual(0, heap.HeapCount);
        Assert.AreEqual(3, heap.ListCount);
    }

    [TestMethod]
    public void Index_AccessItemsFromHeapAndBuffer()
    {
        var list = new[] { 1, 2 };
        var heap = new BinaryHeapListWrapper<int>(list, Comparer<int>.Default, 1, StoreHeapAtTheEnd);
        Assert.AreEqual(1, heap[0]);
        Assert.AreEqual(2, heap[1]);
    }

    [TestMethod]
    public void GetEnumerator_EnumeratesItemsFromHeapAndBuffer()
    {
        var list = new[] { 1, 2 };
        var heap = new BinaryHeapListWrapper<int>(list, Comparer<int>.Default, 1, StoreHeapAtTheEnd);
        var enumeratedItems = new List<int>();
        foreach(var item in heap)
            enumeratedItems.Add(item);

        Assert.IsTrue(new[] { 1, 2 }.SequenceEqual(enumeratedItems));
    }
}

[TestClass]
public class BinaryHeapListWrapperTests_HeapAtTheBeginning : BinaryHeapListWrapperTests
{
    public BinaryHeapListWrapperTests_HeapAtTheBeginning() : base(false)
    {
    }
}

[TestClass]
public class BinaryHeapListWrapperTests_HeapAtTheEnd : BinaryHeapListWrapperTests
{
    public BinaryHeapListWrapperTests_HeapAtTheEnd() : base(true)
    {
    }
}