using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.PriorityQueues.BinomialHeap;

namespace MoreStructures.Tests.PriorityQueues.BinomialHeap;

[TestClass]
public class UpdatableBinomialHeapPriorityQueueTests_AsBasicQueue : PriorityQueueTests
{
    public UpdatableBinomialHeapPriorityQueueTests_AsBasicQueue() : base(
        () => new UpdatableBinomialHeapPriorityQueue<int>(),
        () => new UpdatableBinomialHeapPriorityQueue<RefType>(),
        () => new UpdatableBinomialHeapPriorityQueue<ValType>())
    {
    }
}

[TestClass]
public class UpdatableBinomialHeapPriorityQueueTests_AsUpdatableQueue : UpdatablePriorityQueueTests
{
    public UpdatableBinomialHeapPriorityQueueTests_AsUpdatableQueue() : base(
        () => new UpdatableBinomialHeapPriorityQueue<int>(),
        () => new UpdatableBinomialHeapPriorityQueue<RefType>(),
        () => new UpdatableBinomialHeapPriorityQueue<ValType>())
    {
    }
}

[TestClass]
public class UpdatableBinomialHeapPriorityQueueTests_AsMergeableAndUpdatableQueue 
    : MergeableAndUpdatablePriorityQueueTests<UpdatableBinomialHeapPriorityQueue<int>>
{
    public UpdatableBinomialHeapPriorityQueueTests_AsMergeableAndUpdatableQueue() : base(
        () => new UpdatableBinomialHeapPriorityQueue<int>())
    {
    }

    [TestMethod]
    public void Merge_SourceOfAMergeCannotBeUsedAsTargetOfAnotherMerge()
    {
        var source = IntBuilder();
        var target = IntBuilder();
        source.MergeFrom(target);
        
        var newSource = IntBuilder();
        Assert.ThrowsException<NotSupportedException>(() => newSource.MergeFrom(source));
    }
}

