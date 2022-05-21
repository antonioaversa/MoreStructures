using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.CountTrees;
using static StringAlgorithms.Tests.CountTrees.TreeMock;

namespace StringAlgorithms.Tests.CountTrees;

[TestClass]
public class CountTreeBuilderTests
{
    [TestMethod]
    public void Ctor_IsConcrete()
    {
        _ = new CountTreeBuilder<Edge, Node, Builder>();
    }
}
