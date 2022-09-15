using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.PriorityQueues;
using MoreStructures.PriorityQueues.Extensions;

namespace MoreStructures.Tests.PriorityQueues;

public abstract class MergeablePriorityQueueTests<TIntPriorityQueue>
    where TIntPriorityQueue : IMergeablePriorityQueue<int, TIntPriorityQueue>
{
    protected Func<TIntPriorityQueue> IntBuilder { get; }

    protected MergeablePriorityQueueTests(Func<TIntPriorityQueue> intBuilder)
    {
        IntBuilder = intBuilder;
    }

    [TestMethod]
    public void Merge_WithEmptySource()
    {
        var source = IntBuilder();
        var target = IntBuilder();
        target.Push(1, 2);
        target.Push(3, 3);
        var countBeforeMerge = target.Count;
        source.MergeFrom(target);
        var countAfterMerge = source.Count;
        Assert.AreEqual(countBeforeMerge, countAfterMerge);
    }

    [TestMethod]
    public void Merge_WithEmptyTarget()
    {
        var source = IntBuilder();
        var target = IntBuilder();
        source.Push(1, 2);
        source.Push(3, 3);
        var countBeforeMerge = source.Count;
        source.MergeFrom(target);
        var countAfterMerge = source.Count;
        Assert.AreEqual(countBeforeMerge, countAfterMerge);
    }

    [TestMethod]
    public void Merge_ContainsElementsFromBothQueues()
    {
        var sourceList = Enumerable.Range(0, 5).Select(i => 2 * i).ToArray();
        var targetList = Enumerable.Range(0, 5).Select(i => 2 * i + 1).ToArray();
        foreach (var sourcePermutation in TestUtilities.GeneratePermutations(sourceList))
        {
            foreach (var targetPermutation in TestUtilities.GeneratePermutations(targetList))
            {
                var source = IntBuilder();
                foreach (var sourceItem in sourcePermutation)
                    source.Push(sourceItem, targetPermutation[0] + sourceItem);

                var target = IntBuilder();
                foreach (var targetItem in targetPermutation)
                    target.Push(targetItem, sourcePermutation[0] + targetItem);

                var itemsBeforeMerge = source.Concat(target).OrderByDescending(i => i).ToList();
                var countBeforeMerge = source.Count + target.Count;
                source.MergeFrom(target);
                var countAfterMerge = source.Count;
                var itemsAfterMerge = source.OrderByDescending(i => i).ToList();
                Assert.AreEqual(countBeforeMerge, countAfterMerge);
                Assert.IsTrue(itemsAfterMerge.SequenceEqual(itemsBeforeMerge));
            }
        }
    }

    [TestMethod]
    public void Merge_WorksWithManyItems()
    {
        var numberOfItems = 2048;
        var source = IntBuilder();
        for (var i = 0; i < numberOfItems; i += 2)
            source.Push(i, i % 7);
        var target = IntBuilder();
        for (var i = 1; i < numberOfItems; i += 2)
            target.Push(i, i % 5);
        source.MergeFrom(target);
        Assert.AreEqual(numberOfItems, source.Count);
        Assert.IsTrue(Enumerable.Range(0, numberOfItems).SequenceEqual(source.OrderBy(i => i)));
    }

    [TestMethod]
    public void Merge_WorksWithDuplicates()
    {
        var source = IntBuilder();
        source.Push(1, 2);
        source.Push(2, 4);
        source.Push(3, 3);
        source.Push(4, 1);

        var target = IntBuilder();
        target.Push(1, 1);
        target.Push(2, 6);
        target.Push(3, 3);
        target.Push(5, 0);

        var countBeforeMerge = source.Count + target.Count;
        source.MergeFrom(target);
        var countAfterMerge = source.Count;
        Assert.AreEqual(countBeforeMerge, countAfterMerge);
        Assert.AreEqual(2, source.Peek().Item);
        Assert.AreEqual(6, source.Peek().Priority);
        source.Pop();
        Assert.AreEqual(2, source.Peek().Item);
        Assert.AreEqual(4, source.Peek().Priority);
        source.Pop();
        Assert.AreEqual(3, source.Peek().Item);
        Assert.AreEqual(3, source.Peek().Priority);
        source.Pop();
        Assert.AreEqual(3, source.Peek().Item);
        Assert.AreEqual(3, source.Peek().Priority);
    }

    [TestMethod]
    public void Merge_CanBeRepeatedWithMultipleTargets()
    {
        var source = IntBuilder();
        source.Push(1, 2);
        source.Push(2, 4);
        source.Push(3, 3);
        source.Push(4, 1);

        var target1 = IntBuilder();
        target1.Push(1, 1);
        target1.Push(2, 6);
        target1.Push(3, 3);
        target1.Push(5, 0);

        var target2 = IntBuilder();
        target2.Push(2, 1);
        target2.Push(4, 6);
        target2.Push(5, 3);
        target2.Push(1, 0);

        var countBeforeMerge = source.Count + target1.Count + target2.Count;
        var itemsBeforeMerge = source.Concat(target1).Concat(target2).OrderByDescending(i => i).ToList();
        source.MergeFrom(target1);
        source.MergeFrom(target2);
        var countAfterMerge = source.Count;
        var itemsAfterMerge = source.OrderByDescending(i => i).ToList();
        Assert.AreEqual(countBeforeMerge, countAfterMerge);
        Assert.IsTrue(itemsAfterMerge.SequenceEqual(itemsBeforeMerge));
    }

    [TestMethod]
    public void Merge_RemoveAllItemsFromTheTarget()
    {
        var source = IntBuilder();
        source.Push(1, 2);
        source.Push(2, -4);

        var target = IntBuilder();
        target.Push(1, 1);
        target.Push(5, 2);
        target.Push(3, -1);

        source.MergeFrom(target);
        Assert.AreEqual(0, target.Count);
    }

    [TestMethod]
    public void Merge_TargetIsUsableAfterMerge()
    {
        var source = IntBuilder();
        source.Push(1, 2);
        source.Push(2, -4);

        var target = IntBuilder();
        target.Push(1, 1);
        target.Push(5, 2);
        target.Push(3, -1);

        source.MergeFrom(target);

        Assert.ThrowsException<InvalidOperationException>(() => target.Peek());
        Assert.ThrowsException<InvalidOperationException>(() => target.Pop());
        target.Push(4, 2);
        Assert.AreEqual(1, target.Count);
        Assert.AreEqual(4, target.Peek().Item);
        Assert.AreEqual(2, target.Peek().Priority);
        target.Push(2, 3);
        Assert.AreEqual(2, target.Count);
        Assert.AreEqual(2, target.Peek().Item);
        Assert.AreEqual(3, target.Peek().Priority);
        target.Push(4, 4);
        Assert.AreEqual(3, target.Count);
        Assert.AreEqual(4, target.Peek().Item);
        Assert.AreEqual(4, target.Peek().Priority);
        target.Pop();
        Assert.AreEqual(2, target.Count);
        Assert.AreEqual(2, target.Peek().Item);
        Assert.AreEqual(3, target.Peek().Priority);
        target.Pop();
        target.Pop();
        Assert.AreEqual(0, target.Count);
    }

    [TestMethod]
    public void Merge_TargetChangesDontInterfereWithSource()
    {
        var source = IntBuilder();
        source.Push(1, 2);
        source.Push(2, -4);

        var target = IntBuilder();
        target.Push(1, 1);
        target.Push(5, 2);
        target.Push(3, -1);

        source.MergeFrom(target);

        var sourceCountBeforeTargetPush = source.Count;
        target.Push(3, 4);
        var sourceCountAfterTargetPush = source.Count;
        Assert.AreEqual(sourceCountBeforeTargetPush, sourceCountAfterTargetPush);
        target.Pop();
        var sourceCountAfterTargetPop = source.Count;
        Assert.AreEqual(sourceCountBeforeTargetPush, sourceCountAfterTargetPop);
    }

    [TestMethod]
    public void Merge_KeepsPushTimestampsUniqueWithinEras()
    {
        var source = IntBuilder();
        source.Push(1, 1);
        source.Push(1, 1);

        var target = IntBuilder();
        target.Push(1, 1);
        target.Push(1, 1);
        target.Push(1, 1);

        source.MergeFrom(target);
        var allPrioritizedItems = new List<PrioritizedItem<int>>();
        while (source.Count > 0) allPrioritizedItems.Add(source.Pop());

        var allPushTimestamps = new HashSet<(int, int)>();
        Assert.IsTrue(allPrioritizedItems.All(
            pi => allPushTimestamps.Add((pi.PushTimestamp, pi.PushTimestampEra.Era))));
    }

    [TestMethod]
    public void Merge_IfSamePriorityTakeItemsComingFromTargetAfterSource()
    {
        var target = IntBuilder();
        target.Push(1, 1);
        target.Push(2, 1);
        target.Push(3, 1);

        var source = IntBuilder();
        source.Push(4, 1);
        source.Push(5, 1);

        source.MergeFrom(target);

        var items = source.PopAll().ToList();
        Assert.IsTrue(
            new[] { (4, 1), (5, 1), (1, 1), (2, 1), (3, 1) }.SequenceEqual(
                items.Select(pi => (pi.Item, pi.Priority))));
    }

    [TestMethod]
    public void Merge_IfDifferentPrioritySortByItNoMatterIfFromSourceOrTarget()
    {
        var target = IntBuilder();
        target.Push(1, 2);
        target.Push(2, 1);
        target.Push(3, 4);

        var source = IntBuilder();
        source.Push(4, 3);
        source.Push(5, 1);

        source.MergeFrom(target);

        var items = source.PopAll().ToList();
        Assert.IsTrue(
            new[] { (3, 4), (4, 3), (1, 2), (5, 1), (2, 1) }.SequenceEqual(
                items.Select(pi => (pi.Item, pi.Priority))));
    }

    [TestMethod]
    public void Merge_IfSamePriorityTakeItemsAfterMergeAfterTarget()
    {
        var source = IntBuilder();
        var target = IntBuilder();
        target.Push(1, 2);
        target.Push(2, 1);
        target.Push(3, 4);

        source.MergeFrom(target);
        source.Push(4, 2);

        var items = source.PopAll().ToList();
        Assert.IsTrue(
            new[] { (3, 4), (1, 2), (4, 2), (2, 1) }.SequenceEqual(
                items.Select(pi => (pi.Item, pi.Priority))));
    }

    [TestMethod]
    public void MergeAndClear_ComplexScenario()
    {
        var numbers1 = Enumerable.Range(0, 5).ToArray();
        var numberOfValues1 = numbers1.Length;
        var numbers2 = Enumerable.Range(3, 5).Select(i => i * 2).ToArray();
        var numberOfValues2 = numbers2.Length;
        foreach (var permutation1 in TestUtilities.GeneratePermutations(numbers1))
        {
            foreach (var permutation2 in TestUtilities.GeneratePermutations(numbers2))
            {
                var source = IntBuilder();
                foreach (var value in numbers1)
                    source.Push(value, value);

                var target = IntBuilder();
                foreach (var value in numbers2)
                {
                    if (value % 3 == 0)
                    {
                        source.MergeFrom(target);
                        target.Clear();
                    }
                    target.Push(value, value);
                }

                source.MergeFrom(target);

                Assert.IsTrue(new[] { 14, 12, 10, 8, 6, 4, 3, 2, 1, 0 }.SequenceEqual(source));
            }
        }
    }

    [TestMethod]
    public void Clear_WipesAllItemsOut()
    {
        var queue = IntBuilder();
        queue.Push(1, 2);
        queue.Push(2, -4);
        queue.Clear();
        Assert.AreEqual(0, queue.Count);

        queue.Clear();
        Assert.AreEqual(0, queue.Count);
    }

    [TestMethod]
    public void Clear_QueueKeepsWorkingAfter()
    {
        var queue = IntBuilder();
        queue.Push(1, 2);
        queue.Push(2, -4);
        queue.Clear();

        queue.Push(1, 1);
        Assert.AreEqual(1, queue.Count);
        Assert.AreEqual(1, queue.Peek().Item);
        Assert.AreEqual(1, queue.Peek().Priority);
        queue.Push(2, 2);
        Assert.AreEqual(2, queue.Count);
        Assert.AreEqual(2, queue.Peek().Item);
        Assert.AreEqual(2, queue.Peek().Priority);
        var poppedItem1 = queue.Pop();
        Assert.AreEqual(1, queue.Count);
        Assert.AreEqual(2, poppedItem1.Item);
        Assert.AreEqual(2, poppedItem1.Priority);
        Assert.AreEqual(1, queue.Peek().Item);
        Assert.AreEqual(1, queue.Peek().Priority);
        var poppedItem2 = queue.Pop();
        Assert.AreEqual(0, queue.Count);
        Assert.AreEqual(1, poppedItem2.Item);
        Assert.AreEqual(1, poppedItem2.Priority);
    }

    [TestMethod]
    public void Clear_DoesntResetPushTimestamps()
    {
        var queue = IntBuilder();
        queue.Push(1, 1);
        queue.Push(1, 1);
        queue.Push(1, 1);
        var poppedItems1 = new List<PrioritizedItem<int>>();
        while (queue.Count > 1) // Leaves one item in the queue
            poppedItems1.Add(queue.Pop());

        queue.Clear();
        queue.Push(1, 1);
        queue.Push(1, 1);
        queue.Push(1, 1);

        var poppedItems2 = new List<PrioritizedItem<int>>();
        while (queue.Count > 0)
            poppedItems2.Add(queue.Pop());

        Assert.IsFalse(poppedItems1.Intersect(poppedItems2).Any());
        var uniquePushTimestamps = new HashSet<int>();
        Assert.IsTrue(poppedItems1.Concat(poppedItems2).All(pi => uniquePushTimestamps.Add(pi.PushTimestamp)));
    }

    
}

