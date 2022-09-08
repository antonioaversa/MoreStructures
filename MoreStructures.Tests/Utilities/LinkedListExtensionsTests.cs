using MoreStructures.Utilities;

namespace MoreStructures.Tests.Utilities;

[TestClass]
public class LinkedListExtensionsTests
{
    [TestMethod]
    public void AsNodes_ReturnsItemsInOrder()
    {
        var list = new LinkedList<int>();
        var items = new[] { 5, 4, 9, 2, 6 };
        foreach (var item in items)
            list.AddLast(item);

        Assert.IsTrue(items.SequenceEqual(list.AsNodes().Select(n => n.Value)));
    }

    [TestMethod]
    public void AsNodes_IsEmptyOnEmptyList()
    {
        var list = new LinkedList<int>();
        Assert.IsFalse(list.AsNodes().Any());
        list.AddLast(3);
        Assert.IsTrue(list.AsNodes().Any());
    }

    [TestMethod]
    public void AsNodes_FirstCoincide()
    {
        var list = new LinkedList<int>();
        list.AddLast(3);
        Assert.AreSame(list.First, list.AsNodes().First());
    }

    [TestMethod]
    public void AsNode_IntermediateCoincide()
    {
        var list = new LinkedList<int>();
        list.AddLast(3);
        list.AddLast(4);
        list.AddLast(5);
        Assert.AreSame(list.First!.Next, list.AsNodes().ElementAt(1));
    }

    [TestMethod]
    public void AsNodes_LastCoincide()
    {
        var list = new LinkedList<int>();
        list.AddLast(3);
        list.AddLast(4);
        list.AddLast(5);
        Assert.AreSame(list.Last, list.AsNodes().Last());
    }
}
