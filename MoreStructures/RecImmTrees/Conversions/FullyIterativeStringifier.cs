namespace MoreStructures.RecImmTrees.Conversions;

/// <summary>
/// <inheritdoc/>
/// Iterative implementation.
/// </summary>
/// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
/// <remarks>
///     <inheritdoc cref="IStringifier{TEdge, TNode}" path="/remarks"/>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     TODO
///     </para>
/// </remarks>
public class FullyIterativeStringifier<TEdge, TNode> 
    : StringifierBase<TEdge, TNode>, IStringifier<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    /// <summary>
    /// The maximum level at which indentation should not be done anymore. Default is <see cref="int.MaxValue"/>.
    /// </summary>
    /// <remarks>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - When trying to render a very deep structure to string, the resulting string can become extremely big due 
    ///       to indentation. 
    ///       <br/>
    ///     - This can easily happen with structures like <see cref="SuffixTries.SuffixTrieNode"/>. Less with 
    ///       <see cref="SuffixTrees.SuffixTreeNode"/>, due to their coalescing of paths of nodes with single child.
    ///       <br/>
    ///     - For example if the structure is a linear chain of n in depth, 4 chars of indentation per line would yield 
    ///       a string of 2n(n-1) chars = O(n^2). 
    ///       <br/>
    ///     - For n = 10000 nodes the produced string would be ~ 200M.
    ///       <br/>
    ///     - To avoid that <see cref="StopIndentingLevel"/> can be set to a constant c, limiting the size of the 
    ///       resulting string by an upper bound of cn = O(n). 
    ///       <br/>
    ///     - For n = 10000 nodes and c = 10 levels the produced string would be 100K.
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

    /// <inheritdoc/>
    /// <inheritdoc cref="FullyIterativeStringifier{TEdge, TNode}" path="/remarks"/>
    public override string Stringify(TreePath<TEdge, TNode> path)
    {
        if (!path.PathNodes.Any())
            return string.Empty;

        var stringBuilder = new StringBuilder();

        var (firstEdge, firstNode) = path.PathNodes.First();
        stringBuilder.Append(EdgeAndNodeStringifier(firstEdge, firstNode));

        foreach (var (edge, node) in path.PathNodes.Skip(1))
        {
            stringBuilder.Append(PathSeparator);
            stringBuilder.Append(EdgeAndNodeStringifier(edge, node));
        }

        return stringBuilder.ToString();
    }
}