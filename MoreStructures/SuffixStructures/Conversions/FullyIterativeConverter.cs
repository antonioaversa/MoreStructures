using MoreStructures.SuffixTrees;
using MoreStructures.SuffixTries;
using static MoreStructures.SuffixStructures.Conversions.ConverterHelpers;

namespace MoreStructures.SuffixStructures.Conversions;

/// <summary>
/// <inheritdoc cref="IConverter"/>
/// </summary>
/// <remarks>
/// Conversion is iteratively both for branching and no-branching paths (i.e. on nodes having a single child) of the 
/// input <see cref="SuffixTrieNode"/>, with occasional mutation of internal state of the conversion and the use of a 
/// stack to store nodes to process.
/// Not limited by call stack depth. Convenient with deep trees (i.e. trees having a height &gt; ~1K nodes).
/// </remarks>
public class FullyIterativeConverter : IConverter
{
    /// <inheritdoc/>
    /// <inheritdoc cref="FullyIterativeConverter" path="/remarks"/>
    public SuffixTreeNode TrieToTree(SuffixTrieNode trieNode)
    {
        var stack = new Stack<StackFrameTrieToTree>();
        var dictionaryToRoot = new Dictionary<SuffixTreeEdge, SuffixTreeNode>() { };
        var edgeToRoot = new SuffixTreeEdge(0, 1);
        stack.Push(new StackFrameTrieToTree(null, dictionaryToRoot, edgeToRoot, trieNode));

        while (stack.Count > 0)
            ProcessStack(stack);

        return dictionaryToRoot[edgeToRoot];
    }

    private record struct StackFrameTrieToTree(
        // Non-null on second pass only, when children have already been processed (done only for intermediate nodes)
        IDictionary<SuffixTreeEdge, SuffixTreeNode>? TreeNodeChildrenMaybe,
        IDictionary<SuffixTreeEdge, SuffixTreeNode> TreeNodeParentChildren,
        SuffixTreeEdge TreeEdge,
        SuffixTrieNode TrieNode);

    private static void ProcessStack(Stack<StackFrameTrieToTree> stack)
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

                stack.Push(new StackFrameTrieToTree(treeNodeChildren, treeNodeParentChildren, treeEdge, trieNode));

                foreach (var (trieChildEdge, trieChildNode) in trieNodeChildren)
                {
                    var (coalescedChildEdge, coalescedChildNode) = IterativeCoalesce(trieChildEdge, trieChildNode);
                    stack.Push(new StackFrameTrieToTree(null, treeNodeChildren, coalescedChildEdge, coalescedChildNode));
                }
            }

            return;
        }

        throw new NotSupportedException($"{trieNode} of type {trieNode.GetType().Name} not supported");
    }

    private record struct StackFrameTreeToTrie(
        // Non-null on second pass only, when children have already been processed (done only for intermediate nodes)
        IDictionary<SuffixTrieEdge, SuffixTrieNode>? TrieNodeChildrenMaybe,
        SuffixTreeEdge TreeEdge, 
        SuffixTreeNode TreeNode, 
        IDictionary<SuffixTrieEdge, SuffixTrieNode> ParentChildren);

    /// <inheritdoc/>
    /// <inheritdoc cref="FullyIterativeConverter" path="/remarks"/>
    public SuffixTrieNode TreeToTrie(SuffixTreeNode treeNode)
    {
        var stack = new Stack<StackFrameTreeToTrie>();
        var edgeToRoot = new SuffixTrieEdge(0);
        var parentChildren = new Dictionary<SuffixTrieEdge, SuffixTrieNode> { };
        stack.Push(new(null, edgeToRoot, treeNode, parentChildren));
        while (stack.Count > 0)
            ProcessStack(stack);

        return parentChildren[edgeToRoot];
    }

    private static void ProcessStack(Stack<StackFrameTreeToTrie> stack)
    {
        var (trieNodeChildrenMaybe, treeEdge, treeNode, parentChildren) = stack.Pop();

        if (treeNode is SuffixTreeNode.Leaf(var leafStart))
        {
            var (expandedTrieEdge, expandedTrieNode) = IterativeExpand(treeEdge, new SuffixTrieNode.Leaf(leafStart));
            parentChildren[expandedTrieEdge] = expandedTrieNode;
            return;
        }

        if (treeNode is SuffixTreeNode.Intermediate(var treeNodeChildren))
        {
            if (trieNodeChildrenMaybe is IDictionary<SuffixTrieEdge, SuffixTrieNode> trieNodeChildren)
            {
                var (expandedTrieEdge, expandedTrieNode) = IterativeExpand(
                   treeEdge, new SuffixTrieNode.Intermediate(trieNodeChildren));
                parentChildren[expandedTrieEdge] = expandedTrieNode;
            }
            else
            {
                trieNodeChildren = new Dictionary<SuffixTrieEdge, SuffixTrieNode> { };
                stack.Push(new(trieNodeChildren, treeEdge, treeNode, parentChildren));
                foreach (var child in treeNodeChildren)
                    stack.Push(new(null, child.Key, child.Value, trieNodeChildren));
            }

            return;
        }

        throw new NotSupportedException($"{treeNode} of type {treeNode.GetType().Name} not supported");
    }
}
