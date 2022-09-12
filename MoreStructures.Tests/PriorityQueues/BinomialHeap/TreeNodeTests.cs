using MoreStructures.PriorityQueues;
using MoreStructures.PriorityQueues.BinomialHeap;

namespace MoreStructures.Tests.PriorityQueues.BinomialHeap;

[TestClass]
public class TreeNodeTests
{
    protected class TreeNodeMock : TreeNode<string>
    {
    }

    [TestMethod]
    public void IsInAHeap_IsTrueWhenARoot()
    {
        var treeNode = new TreeNodeMock() { PrioritizedItem = new("3", 2, 0) };
        Assert.IsFalse(treeNode.IsInAHeap);

        var roots = new LinkedList<TreeNode<string>>();
        treeNode.RootsListNode = roots.AddLast(treeNode);
        Assert.IsTrue(treeNode.IsInAHeap);
    }

    [TestMethod]
    public void IsInAHeap_IsTrueWhenAChild()
    {
        var treeNode = new TreeNodeMock() { PrioritizedItem = new("3", 2, 0) };
        Assert.IsFalse(treeNode.IsInAHeap);

        var parentTreeNode = new TreeNodeMock() { PrioritizedItem = new("4", 2, 1) };
        parentTreeNode.AddChild(treeNode);

        Assert.IsTrue(treeNode.IsInAHeap);
        Assert.IsFalse(parentTreeNode.IsInAHeap);
    }

    [TestMethod]
    public void AddChild_ThrowsExceptionIfNodeIsARoot()
    {
        var treeNode = new TreeNodeMock() { PrioritizedItem = new("3", 2, 0) };
        var roots = new LinkedList<TreeNode<string>>();
        treeNode.RootsListNode = roots.AddLast(treeNode);

        var parentTreeNode = new TreeNodeMock() { PrioritizedItem = new("4", 2, 1) };
        Assert.ThrowsException<InvalidOperationException>(() => parentTreeNode.AddChild(treeNode));
    }

    [TestMethod]
    public void AddChild_ThrowsExceptionIfNodeIsChildOfAnotherNode()
    {
        var treeNode = new TreeNodeMock() { PrioritizedItem = new("3", 2, 0) };
        var parentTreeNode1 = new TreeNodeMock() { PrioritizedItem = new("4", 2, 1) };
        parentTreeNode1.AddChild(treeNode);

        var parentTreeNode2 = new TreeNodeMock() { PrioritizedItem = new("2", 4, 2) };
        Assert.ThrowsException<InvalidOperationException>(() => parentTreeNode2.AddChild(treeNode));
    }

    [TestMethod]
    public void AddChild_ThrowsExceptionIfNodeIsAlreadyChildOfTheParent()
    {
        var treeNode = new TreeNodeMock() { PrioritizedItem = new("3", 2, 0) };
        var parentTreeNode = new TreeNodeMock() { PrioritizedItem = new("4", 2, 1) };
        parentTreeNode.AddChild(treeNode);

        Assert.ThrowsException<InvalidOperationException>(() => parentTreeNode.AddChild(treeNode));
    }

    [TestMethod]
    public void AddChild_ThrowsExceptionIfNodeParentPropertiesAreIncoherent()
    {
        var treeNode = new TreeNodeMock() { PrioritizedItem = new("3", 2, 0) };
        var parentTreeNode = new TreeNodeMock() { PrioritizedItem = new("4", 2, 1) };
        parentTreeNode.AddChild(treeNode);
        treeNode.Parent = null;

        Assert.ThrowsException<InvalidOperationException>(() => parentTreeNode.AddChild(treeNode));
    }

    [TestMethod]
    public void AddChild_AddsToTheChildrenOfTheParentAndSetBackreferences()
    {
        var treeNode = new TreeNodeMock() { PrioritizedItem = new("3", 2, 0) };
        var parentTreeNode = new TreeNodeMock() { PrioritizedItem = new("4", 2, 1) };
        parentTreeNode.AddChild(treeNode);

        Assert.IsTrue(parentTreeNode.Children.Contains(treeNode));
        Assert.AreSame(parentTreeNode, treeNode.Parent);
        Assert.AreSame(parentTreeNode.Children.Last, treeNode.ParentListNode);
    }

    [TestMethod]
    public void DetachFromParent_ThrowsExceptionOnOrphan()
    {
        var treeNode = new TreeNodeMock() { PrioritizedItem = new("3", 2, 0) };
        Assert.ThrowsException<InvalidOperationException>(() => treeNode.DetachFromParent());
    }

    [TestMethod]
    public void DetachFromParent_ThrowsExceptionWhenParentPropertiesAreIncoherent()
    {
        var treeNode = new TreeNodeMock() { PrioritizedItem = new("3", 2, 0) };
        var parentTreeNode = new TreeNodeMock() { PrioritizedItem = new("4", 2, 1) };
        parentTreeNode.AddChild(treeNode);
        treeNode.Parent = null;
        Assert.ThrowsException<InvalidOperationException>(() => treeNode.DetachFromParent());
    }

    [TestMethod]
    public void DetachFromParent_ThrowsExceptionIfNodeIsARoot()
    {
        var treeNode = new TreeNodeMock() { PrioritizedItem = new("3", 2, 0) };
        var parentTreeNode = new TreeNodeMock() { PrioritizedItem = new("4", 2, 1) };
        parentTreeNode.AddChild(treeNode);

        var roots = new LinkedList<TreeNode<string>>();
        treeNode.RootsListNode = roots.AddLast(treeNode);

        Assert.ThrowsException<InvalidOperationException>(() => treeNode.DetachFromParent());
    }

    [TestMethod]
    public void DetachFromParent_UpdatesParentChildren()
    {
        var treeNode = new TreeNodeMock() { PrioritizedItem = new("3", 2, 0) };
        var parentTreeNode = new TreeNodeMock() { PrioritizedItem = new("4", 2, 1) };
        parentTreeNode.AddChild(treeNode);

        treeNode.DetachFromParent();
        Assert.IsFalse(treeNode.Children.Contains(treeNode));
    }

    [TestMethod]
    public void DetachFromParent_UpdateParentPropertiesOnChild()
    {
        var treeNode = new TreeNodeMock() { PrioritizedItem = new("3", 2, 0) };
        var parentTreeNode = new TreeNodeMock() { PrioritizedItem = new("4", 2, 1) };
        parentTreeNode.AddChild(treeNode);

        Assert.IsNotNull(treeNode.Parent);
        Assert.IsNotNull(treeNode.ParentListNode);

        treeNode.DetachFromParent();
        Assert.IsNull(treeNode.Parent);
        Assert.IsNull(treeNode.ParentListNode);
    }

    [TestMethod]
    public void DeepCopy_PointsToSamePrioritizedItem()
    {
        var prioritizedItem = new PrioritizedItem<string>("3", 2, 0);
        var treeNode = new TreeNodeMock() { PrioritizedItem = prioritizedItem };
        var treeNodeCopy = treeNode.DeepCopy();
        Assert.AreEqual(treeNode.PrioritizedItem, treeNodeCopy.PrioritizedItem);
    }

    [TestMethod]
    public void DeepCopy_WorksWithLeaves()
    {
        var node = new TreeNodeMock() { PrioritizedItem = new("3", 2, 0) };
        var nodeCopy = node.DeepCopy();

        Assert.AreEqual(node.PrioritizedItem, nodeCopy.PrioritizedItem);
        Assert.AreEqual(0, node.Children.Count);
        Assert.AreEqual(0, nodeCopy.Children.Count);
        Assert.IsNull(nodeCopy.Parent);
        Assert.IsNull(nodeCopy.ParentListNode);
    }

    [TestMethod]
    public void DeepCopy_PreservesParentChildStructure()
    {
        var itemParent = new PrioritizedItem<string>("3", 2, 0);
        var nodeParent = new TreeNodeMock() { PrioritizedItem = itemParent };

        var itemChild1 = new PrioritizedItem<string>("4", 2, 1);
        var nodeChild1 = new TreeNodeMock() { PrioritizedItem = itemChild1 };
        nodeParent.AddChild(nodeChild1);

        var itemChild2 = new PrioritizedItem<string>("5", 3, 2);
        var nodeChild2 = new TreeNodeMock() { PrioritizedItem = itemChild2 };
        nodeParent.AddChild(nodeChild2);

        var nodeParentCopy = nodeParent.DeepCopy();

        AssertParentChildrenStructure(nodeParent, nodeParentCopy);
    }

    [TestMethod]
    public void DeepCopy_PreservesParentChildStructureAtMultipleLevels()
    {
        var nodeParent = new TreeNodeMock() { PrioritizedItem = new("3", 2, 0) };

        var nodeChild1 = new TreeNodeMock() { PrioritizedItem = new("4", 2, 1) };
        nodeParent.AddChild(nodeChild1);

        var nodeChild2 = new TreeNodeMock() { PrioritizedItem = new("5", 3, 2) };
        nodeParent.AddChild(nodeChild2);

        var nodeGrandChild11 = new TreeNodeMock() { PrioritizedItem = new("6", 1, 3) };
        nodeChild1.AddChild(nodeGrandChild11);

        var nodeGrandChild12 = new TreeNodeMock() { PrioritizedItem = new("7", 4, 4) };
        nodeChild1.AddChild(nodeGrandChild12);

        var nodeGrandChild21 = new TreeNodeMock() { PrioritizedItem = new("6", 4, 5) };
        nodeChild2.AddChild(nodeGrandChild21);

        var nodeGrandChild22 = new TreeNodeMock() { PrioritizedItem = new("7", 1, 6) };
        nodeChild2.AddChild(nodeGrandChild22);

        var nodeParentCopy = nodeParent.DeepCopy();

        AssertParentChildrenStructure(nodeParent, nodeParentCopy);

        var nodeChild1Copy = nodeParentCopy.Children.First!.Value;
        var nodeChild2Copy = nodeParentCopy.Children.First!.Next!.Value;
        AssertParentChildrenStructure(nodeChild1, nodeChild1Copy);
        AssertParentChildrenStructure(nodeChild2, nodeChild2Copy);
    }

    private static void AssertParentChildrenStructure<T>(TreeNode<T> nodeParent, TreeNode<T> nodeParentCopy)
    {
        Assert.AreNotSame(nodeParent, nodeParentCopy);
        Assert.AreEqual(nodeParent.PrioritizedItem, nodeParentCopy.PrioritizedItem);

        Assert.AreEqual(nodeParent.Children.Count, nodeParentCopy.Children.Count);

        var nodeChild1 = nodeParent.Children.First!.Value;
        var nodeChild1Copy = nodeParentCopy.Children.First!.Value;
        Assert.AreNotSame(nodeChild1, nodeChild1Copy);
        Assert.AreEqual(nodeChild1.PrioritizedItem, nodeChild1Copy.PrioritizedItem);
        Assert.AreSame(nodeParentCopy, nodeChild1Copy.Parent);
        Assert.AreSame(nodeParentCopy.Children.First, nodeChild1Copy.ParentListNode);

        var nodeChild2 = nodeParent.Children.First!.Next!.Value;
        var nodeChild2Copy = nodeParentCopy.Children.First!.Next!.Value;
        Assert.AreNotSame(nodeChild2, nodeChild2Copy);
        Assert.AreEqual(nodeChild2.PrioritizedItem, nodeChild2Copy.PrioritizedItem);
        Assert.AreSame(nodeParentCopy, nodeChild2Copy.Parent);
        Assert.AreSame(nodeParentCopy.Children.First!.Next, nodeChild2Copy.ParentListNode);
    }
}



