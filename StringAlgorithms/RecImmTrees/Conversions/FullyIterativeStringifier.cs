using System.Text;

namespace StringAlgorithms.RecImmTrees.Conversions;

/// <summary>
/// <inheritdoc/>
/// </summary>
/// <remarks>
/// Implemented fully iteratively, so not limited by stack depth but rather by the maximum size of the stack
/// stored in the heap. Convenient with deep trees (i.e. trees having a much bigger height).
/// </remarks>
public class FullyIterativeStringifier<TEdge, TNode> 
    : StringifierBase<TEdge, TNode>, IStringifier<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
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

        var stack = new List<(TEdge edge, TNode node, int level)> { };
        foreach (var (childEdge, childNode) in node.Children)
            stack.Add((childEdge, childNode, 1));

        while (stack.Count > 0)
            Stringify(stringBuilder, stack);
        return stringBuilder.ToString();
    }

    private void Stringify(StringBuilder stringBuilder, IList<(TEdge edge, TNode node, int level)> stack)
    {
        var (edge, node, level) = stack[^1];
        stack.RemoveAt(stack.Count - 1);

        stringBuilder.Append(NewLine);
        for (int i = 0; i < level; i++)
            stringBuilder.Append(Indent);
        stringBuilder.Append(EdgeAndNodeStringifier(edge, node));

        foreach (var (childEdge, childNode) in node.Children)
            stack.Add((childEdge, childNode, level + 1));
    }
}