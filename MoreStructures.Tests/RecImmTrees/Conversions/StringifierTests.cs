using MoreStructures.RecImmTrees;
using MoreStructures.RecImmTrees.Conversions;

namespace MoreStructures.Tests.RecImmTrees.Conversions;

using static TreeMock;
using static StringifierTestsHelpers;

public abstract class StringifierTests
{
    protected IStringifier<Edge, Node> Stringifier { get; init; }
    protected string NL => Stringifier.NewLine;
    protected string I => Stringifier.Indent;

    protected static readonly string DefaultNewLine =
        Environment.NewLine;
    protected static readonly string DefaultIndent = 
        new(' ', 1);
    protected static readonly string DefaultPathSeparator = 
        " -> ";
    protected static readonly Func<Node, string> DefaultRootStringifier =
        n => $"R({n.Id})";
    protected static readonly Func<Edge, Node, string> DefaultEdgeAndNodeStringifier =
        (e, n) => $"e({e.Id}):N({n.Id})";

    public StringifierTests(IStringifier<Edge, Node> stringifier)
    {
        Stringifier = stringifier;
    }

    [TestMethod]
    public void Stringify_OfLeaf()
    {
        var root = new Node(1);
        Assert.AreEqual("R(1)", Stringifier.Stringify(root));
    }

    [TestMethod]
    public void Stringify_OfTwoLevelsTree()
    {
        var root = new Node(1, new Dictionary<Edge, Node> 
        {
            [new(1)] = new(2),
            [new(2)] = new(3),
        });
        var rootStr = Stringifier.Stringify(root);
        var validResults = new string[]
        {
            $"R(1){NL}{I}e(1):N(2){NL}{I}e(2):N(3)",
            $"R(1){NL}{I}e(2):N(3){NL}{I}e(1):N(2)",
        };
        Assert.IsTrue(validResults.Contains(rootStr));
    }

    [TestMethod]
    public void Stringify_OfThreeLevelsTree()
    {
        var root = new Node(1, new Dictionary<Edge, Node>
        {
            [new(1)] = new(2, new Dictionary<Edge, Node>
            {
                [new(3)] = new(4),
            }),
            [new(2)] = new(3),
        });
        var rootStr = Stringifier.Stringify(root);
        var validResults = new string[]
        {
            $"R(1){NL}{I}e(1):N(2){NL}{I}{I}e(3):N(4){NL}{I}e(2):N(3)",
            $"R(1){NL}{I}e(2):N(3){NL}{I}e(1):N(2){NL}{I}{I}e(3):N(4)",
        };
        Assert.IsTrue(validResults.Contains(rootStr));
    }

    [TestMethod]
    public void Stringify_OfFourLevelsTree()
    {
        var root = new Node(1, new Dictionary<Edge, Node>
        {
            [new(1)] = new(2, new Dictionary<Edge, Node>
            {
                [new(3)] = new(4),
                [new(4)] = new(5),
                [new(5)] = new(6, new Dictionary<Edge, Node>
                {
                    [new(6)] = new(7),
                }),
                [new(7)] = new(8),
            }),
            [new(2)] = new(3),
            [new(8)] = new(9, new Dictionary<Edge, Node>
            {
                [new(9)] = new(10),
            }),
        });
        AssertAreEqualBySetOfLines(
            Stringifier, 
            root,
            $"R(1)",
            $"{I}e(1):N(2)",
            $"{I}{I}e(3):N(4)",
            $"{I}{I}e(4):N(5)",
            $"{I}{I}e(5):N(6)",
            $"{I}{I}{I}e(6):N(7)",
            $"{I}{I}e(7):N(8)",
            $"{I}e(2):N(3)",
            $"{I}e(8):N(9)",
            $"{I}{I}e(9):N(10)"
        );
    }

    [TestMethod]
    public void Stringify_OfEmptyPath()
    {
        var path = new TreePath<Edge, Node>();
        var pathStr = Stringifier.Stringify(path);
        Assert.AreEqual(string.Empty, pathStr);
    }

    [TestMethod]
    public void Stringify_OfSingletonPath()
    {
        var path = new TreePath<Edge, Node>((new(0), new(10)));
        var pathStr = Stringifier.Stringify(path);
        foreach (var pathNode in path.PathNodes)
        {
            Assert.IsTrue(pathStr.Contains(pathNode.Key.Id.ToString()));
            Assert.IsTrue(pathStr.Contains(pathNode.Value.Id.ToString()));
        }

        Assert.IsFalse(pathStr.Contains(DefaultPathSeparator));
    }

    [TestMethod]
    public void Stringify_OfMultistepPath()
    {
        var path = new TreePath<Edge, Node>(
            (new(0), new(10)), (new(1), new(11)), (new(2), new(12)));
        var pathStr = Stringifier.Stringify(path);
        foreach (var pathNode in path.PathNodes)
        {
            Assert.IsTrue(pathStr.Contains(pathNode.Key.Id.ToString()));
            Assert.IsTrue(pathStr.Contains(pathNode.Value.Id.ToString()));
        }

        var pathSeparatorOccurrences = Enumerable
            .Range(0, pathStr.Length - DefaultPathSeparator.Length)
            .Count(i => pathStr[i..(i + DefaultPathSeparator.Length)] == DefaultPathSeparator);

        Assert.AreEqual(path.PathNodes.Count() - 1, pathSeparatorOccurrences);
    }
}
