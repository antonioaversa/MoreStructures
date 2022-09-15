using MoreStructures.PriorityQueues.ArrayList;
using MoreStructures.PriorityQueues.BinomialHeap;

namespace MoreStructures.Tests.PriorityQueues.BinomialHeap;

[TestClass]
public class DuplicatedItemsResolutionTests
{
    [TestMethod]
    public void Clear_WipesContentOut()
    {
        var dir = new DuplicatedItemsResolution<string, ArrayListPriorityQueue<int>>();

        var treeNodeInHeap1 = new TreeNode<string> { PrioritizedItem = new("a", 1, 0) };
        var treeNodeInHeap2 = new TreeNode<string> { PrioritizedItem = new("b", 1, 1) };
        var parentTreeNode = new TreeNode<string> { PrioritizedItem = new("b", 2, 2) };
        parentTreeNode.AddChild(treeNodeInHeap1);
        parentTreeNode.AddChild(treeNodeInHeap2);

        dir.RaiseItemPushed(treeNodeInHeap1);
        dir.RaiseItemPushed(treeNodeInHeap2);
        Assert.IsTrue(dir.GetPrioritiesOf("a").Any());
        Assert.IsTrue(dir.GetPrioritiesOf("b").Any());
        dir.Clear();
        Assert.IsFalse(dir.GetPrioritiesOf("a").Any());
        Assert.IsFalse(dir.GetPrioritiesOf("b").Any());
    }
}
