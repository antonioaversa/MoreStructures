using StringAlgorithms.SuffixTrees;
using StringAlgorithms.SuffixTries;

namespace StringAlgorithms.SuffixStructures.Conversions;

/// <summary>
/// <inheritdoc cref="IConverter"/>
/// </summary>
/// <remarks>
/// Conversion is iteratively both for branching and no-branching paths (i.e. on nodes having a single child) of the 
/// input <see cref="SuffixTrieNode"/>, with occasional mutation of internal state of the conversion and the use of a 
/// stack to store nodes to process.
/// Not limited by call stack depth. Convenient with deep trees (i.e. trees having a height > ~1K nodes).
/// </remarks>
public class FullyIterativeConverter : IConverter
{
    /// <inheritdoc/>
    public SuffixTreeNode TrieToTree(SuffixTrieNode trieNode)
    {
        var stack = new Stack<StackFrame>();
        var dictionaryToRoot = new Dictionary<SuffixTreeEdge, SuffixTreeNode>() { };
        var edgeToRoot = new SuffixTreeEdge(0, 1);
        stack.Push(new StackFrame(null, dictionaryToRoot, edgeToRoot, trieNode));

        while (stack.Count > 0)
            ProcessStack(stack);

        return dictionaryToRoot[edgeToRoot];
    }

    private record struct StackFrame(
        IDictionary<SuffixTreeEdge, SuffixTreeNode>? TreeNodeChildrenMaybe, // Whether children have been processed already
        IDictionary<SuffixTreeEdge, SuffixTreeNode> TreeNodeParentChildren,
        SuffixTreeEdge TreeEdge,
        SuffixTrieNode TrieNode);

    private static void ProcessStack(Stack<StackFrame> stack)
    {
        var (treeNodeChildrenMaybe, treeNodeParentChildren, treeEdge, trieNode) = stack.Pop();

        if (trieNode is SuffixTrieNode.Leaf(var leafStart))
        {
            treeNodeParentChildren[treeEdge] = new SuffixTreeNode.Leaf(leafStart);
            return;
        }

        if (trieNode is SuffixTrieNode.Intermediate(var trieNodeChildren))
        {
            if (treeNodeChildrenMaybe is IDictionary<SuffixTreeEdge, SuffixTreeNode> treeNodeChildren)
            {
                treeNodeParentChildren[treeEdge] = new SuffixTreeNode.Intermediate(treeNodeChildren);
            }
            else
            {
                treeNodeChildren = new Dictionary<SuffixTreeEdge, SuffixTreeNode> { };

                stack.Push(new StackFrame(treeNodeChildren, treeNodeParentChildren, treeEdge, trieNode));

                foreach (var (trieChildEdge, trieChildNode) in trieNodeChildren)
                {
                    var treeChildEdge1 = new SuffixTreeEdge(trieChildEdge.Start, trieChildEdge.Length);
                    var trieChildNode1 = trieChildNode;

                    while (trieChildNode1.Children.Count == 1)
                    {
                        if (trieChildNode1 is not SuffixTrieNode.Intermediate)
                            throw new NotSupportedException(
                                $"{trieChildNode1} of type {trieChildNode1.GetType().Name} not supported");

                        var trieChild = trieChildNode1.Children.Single();
                        treeChildEdge1 = new SuffixTreeEdge(
                            treeChildEdge1.Start, treeChildEdge1.Length + trieChild.Key.Length);
                        trieChildNode1 = trieChildNode1.Children.Single().Value;
                    }

                    stack.Push(new StackFrame(null, treeNodeChildren, treeChildEdge1, trieChildNode1));
                }
            }

            return;
        }

        throw new NotSupportedException($"{trieNode} of type {trieNode.GetType().Name} not supported");
    }
}
