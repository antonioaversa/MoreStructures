using System.Text;

namespace MoreStructures.RecImmTrees.Conversions;

/// <summary>
/// <inheritdoc/>
/// Iterative implementation.
/// </summary>
/// <remarks>
///     <inheritdoc cref="IStringifier{TEdge, TNode}" path="/remarks"/>
///     <para>
///     Implemented fully iteratively via a <see cref="Stack{T}"/>, so not limited by call stack depth but rather by 
///     the maximum size of the stack stored in the heap. Convenient with deep trees (i.e. trees having a height > 
///     ~1K nodes).
///     </para>
/// </remarks>
public class FullyIterativeStringifier<TEdge, TNode> 
    : StringifierBase<TEdge, TNode>, IStringifier<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>, IComparable<TEdge>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    /// <summary>
    /// The maximum level at which indentation should not be done anymore. Default is <see cref="int.MaxValue"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     When trying to render a very deep structure to string, the resulting string can become extremely big due to 
    ///     indentation. This can easily happen with structures like <see cref="SuffixTries.SuffixTrieNode"/>. Less 
    ///     with <see cref="SuffixTrees.SuffixTreeNode"/>, due to their coalescing of paths of nodes with single child.
    ///     </para>
    ///     <para>
    ///     For example if the structure is a linear chain of n in depth, 4 chars of indentation per line would yield a 
    ///     string of 2n(n-1) chars = O(n^2). For n = 10000 nodes the produced string would be ~ 200M.
    ///     To avoid that <see cref="StopIndentingLevel"/> can be set to a constant c, limiting the size of the 
    ///     resulting string by an upper bound of cn = O(n). For n = 10000 nodes and c = 10 levels the produced string 
    ///     would be 100K.
    ///     </para>
    /// </remarks>
    public int StopIndentingLevel { get; set; } = int.MaxValue;

    /// <summary>
    /// Whether the actual level should be prepended to the line, once the maximum level of indentation defined at 
    /// <see cref="StopIndentingLevel"/> has been reached. Default is <see langword="true"/>.
    /// </summary>
    public bool PrependLevelAfterStopIndenting { get; set; } = true;

    /// <inheritdoc/>
    /// <inheritdoc cref="FullyIterativeStringifier{TEdge, TNode}" path="/remarks"/>
    public FullyIterativeStringifier(
        Func<TNode, string> rootStringifier, Func<TEdge, TNode, string> edgeAndNodeStringifier) 
        : base(rootStringifier, edgeAndNodeStringifier)
    {
    }

    /// <inheritdoc/>
    /// <inheritdoc cref="FullyIterativeStringifier{TEdge, TNode}" path="/remarks"/>
    public override string Stringify(TNode node)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(RootStringifier(node));

        var stack = new Stack<(TEdge edge, TNode node, int level)> { };
        foreach (var (childEdge, childNode) in node.Children.OrderByDescending(c => c.Key))
            stack.Push((childEdge, childNode, 1));

        while (stack.Count > 0)
            Stringify(stringBuilder, stack);
        return stringBuilder.ToString();
    }

    private void Stringify(StringBuilder stringBuilder, Stack<(TEdge edge, TNode node, int level)> stack)
    {
        var (edge, node, level) = stack.Pop();

        stringBuilder.Append(NewLine);
        var indentationDepth = Math.Min(StopIndentingLevel, level);
        for (int i = 0; i < indentationDepth; i++)
            stringBuilder.Append(Indent);
        if (PrependLevelAfterStopIndenting && level != indentationDepth)
            stringBuilder.Append($"[level {level}]");

        stringBuilder.Append(EdgeAndNodeStringifier(edge, node));

        foreach (var (childEdge, childNode) in node.Children.OrderByDescending(c => c.Key))
            stack.Push((childEdge, childNode, level + 1));
    }
}