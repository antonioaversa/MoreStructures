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
                    var (coalescedChildEdge, coalescedChildNode) = ConverterHelpers.Coalesce(trieChildEdge, trieChildNode);
                    stack.Push(new StackFrame(null, treeNodeChildren, coalescedChildEdge, coalescedChildNode));
                }
            }

            return;
        }

        throw new NotSupportedException($"{trieNode} of type {trieNode.GetType().Name} not supported");
    }
}
