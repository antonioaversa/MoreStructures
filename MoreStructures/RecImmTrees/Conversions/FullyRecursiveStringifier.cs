using System.Text;

namespace MoreStructures.RecImmTrees.Conversions;

/// <summary>
/// <inheritdoc/>
/// </summary>
/// <remarks>
/// Implemented fully recursively, so limited by stack depth and usable with tree of a "reasonable" height.
/// </remarks>
public class FullyRecursiveStringifier<TEdge, TNode> 
    : StringifierBase<TEdge, TNode>, IStringifier<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    /// <inheritdoc/>
    /// <inheritdoc cref="FullyRecursiveStringifier{TEdge, TNode}" path="/remarks"/>
    public FullyRecursiveStringifier(
        Func<TNode, string> rootStringifier, Func<TEdge, TNode, string> edgeAndNodeStringifier) 
        : base(rootStringifier, edgeAndNodeStringifier)
    {
    }

    /// <inheritdoc/>
    /// <inheritdoc cref="FullyRecursiveStringifier{TEdge, TNode}" path="/remarks"/>
    public override string Stringify(TNode node)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(RootStringifier(node));
        Stringify(stringBuilder, node, 0);
        return stringBuilder.ToString();
    }

    private void Stringify(StringBuilder stringBuilder, TNode node, int level)
    {
        foreach (var (childEdge, childNode) in node.Children)
        {
            stringBuilder.Append(NewLine);
            for (int i = 0; i < level; i++)
                stringBuilder.Append(Indent);
            stringBuilder.Append($"{Indent}{EdgeAndNodeStringifier(childEdge, childNode)}");
            Stringify(stringBuilder, childNode, level + 1);
        }
    }
}