using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StringAlgorithms.Tests;

[TestClass]
public class PrefixPathTests
{
    [TestMethod]
    public void IsAdjacentTo_Correctness()
    {
        Assert.IsTrue(new PrefixPath(0, 1).IsAdjacentTo(new (1, 2), PrefixPath.AdjacencyOrder.Before));
        Assert.IsFalse(new PrefixPath(0, 1).IsAdjacentTo(new(1, 2), PrefixPath.AdjacencyOrder.After));
        Assert.IsTrue(new PrefixPath(0, 1).IsAdjacentTo(new(1, 2), PrefixPath.AdjacencyOrder.BeforeOrAfter));

        Assert.IsFalse(new PrefixPath(1, 2).IsAdjacentTo(new(0, 1), PrefixPath.AdjacencyOrder.Before));
        Assert.IsTrue(new PrefixPath(1, 2).IsAdjacentTo(new(0, 1), PrefixPath.AdjacencyOrder.After));
        Assert.IsTrue(new PrefixPath(1, 2).IsAdjacentTo(new(0, 1), PrefixPath.AdjacencyOrder.BeforeOrAfter));
    }

    [TestMethod]
    public void Of()
    {
        Assert.IsTrue(new PrefixPath(0, 0).Of(new("", '$')) == "");
        Assert.IsTrue(new PrefixPath(0, 1).Of(new("", '$')) == "$");
        Assert.IsTrue(new PrefixPath(0, 0).Of(new("a", '$')) == "");
        Assert.IsTrue(new PrefixPath(0, 1).Of(new("a", '$')) == "a");
        Assert.IsTrue(new PrefixPath(0, 2).Of(new("a", '$')) == "a$");
    }

}
