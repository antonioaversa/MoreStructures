using MoreStructures.RecImmTrees.Conversions;

namespace MoreStructures.Tests.RecImmTrees.Conversions;

using static TreeMock;

[TestClass]
public class FullyRecursiveStringifierTests : StringifierBaseTests
{
    public FullyRecursiveStringifierTests() : base(
        new FullyRecursiveStringifier<Edge, Node>(
            DefaultRootStringifier, DefaultEdgeAndNodeStringifier)
        {
            NewLine = DefaultNewLine,
            Indent = DefaultIndent,
            PathSeparator = DefaultPathSeparator,
        })
    {
    }

    [TestMethod]
    public void Stringify_DoesntStackOverflowWithNotSoDeepStructures()
    {
        var numberOfNodes = 100;
        var root = new Node(0);
        var edgesAndNodes = new List<(Edge edge, Node node)> { };
        for (int i = numberOfNodes - 1; i >= 1; i--)
        {
            var edge = new Edge(i - 1);
            edgesAndNodes.Add((edge, root));

            root = new Node(i, new Dictionary<Edge, Node> { [edge] = root });
        }

        var rootStr = Stringifier.Stringify(root);
        Assert.IsTrue(edgesAndNodes.All(
            edgeAndNode => rootStr.Contains(DefaultEdgeAndNodeStringifier(edgeAndNode.edge, edgeAndNode.node))));
    }
}
