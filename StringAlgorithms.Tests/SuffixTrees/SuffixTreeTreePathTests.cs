using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.RecImmTrees;
using StringAlgorithms.SuffixTrees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StringAlgorithms.Tests.SuffixTrees;

[TestClass]
public class SuffixTreeTreePathTests
{
    [TestMethod]
    public void PathNodes_ImmutabilityOnGet()
    {
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode>();
        if (path.PathNodes is IList<KeyValuePair<SuffixTreeEdge, SuffixTreeNode>> pathNodesAsList)
        {
            Assert.ThrowsException<NotSupportedException>(() => pathNodesAsList.Add(
                KeyValuePair.Create(new SuffixTreeEdge(0, 1), new SuffixTreeNode.Leaf(0) as SuffixTreeNode)));
        }
    }

    [TestMethod]
    public void PathNodes_ImmutabilityOnCtorParam()
    {
        var pathNodes = new Dictionary<SuffixTreeEdge, SuffixTreeNode> { };
        var path = new TreePath<SuffixTreeEdge, SuffixTreeNode>(pathNodes);
        Assert.AreEqual(0, path.PathNodes.Count());
        pathNodes[new(0, 1)] = new SuffixTreeNode.Leaf(0);
        Assert.AreEqual(0, path.PathNodes.Count());
    }

    /// <remarks>
    /// The example is the second "root to leaf"" path of the tree built from <see cref="ExampleText"/>: 
    /// a -> ba -> a$.
    /// </remarks>
    internal static TreePath<SuffixTreeEdge, SuffixTreeNode> BuildSuffixTreePathExample()
    {
        var leaf = new SuffixTreeNode.Leaf(2);
        var leafEdge = new SuffixTreeEdge(5, 2);

        var intermediate1 = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [new(3, 4)] = new SuffixTreeNode.Leaf(0),
            [leafEdge] = leaf,
        });
        var intermediate1Edge = new SuffixTreeEdge(1, 2);
        
        var intermediate2 = new SuffixTreeNode.Intermediate(new Dictionary<SuffixTreeEdge, SuffixTreeNode>
        {
            [intermediate1Edge] = intermediate1,
        });
        var intermediate2Edge = new SuffixTreeEdge(0, 1);

        return new(new List<KeyValuePair<SuffixTreeEdge, SuffixTreeNode>>
        {
            new(intermediate2Edge, intermediate2),
            new(intermediate1Edge, intermediate1),
            new(leafEdge, leaf),            
        });
    }
}
