using System.Text;

namespace StringAlgorithms.RecImmTrees.Stringifiable;

/// <summary>
/// <inheritdoc cref="IStringifier"/>
/// </summary>
/// <remarks>
/// Implemented fully recursively, so limited by stack depth and usable with tree a "reasonable" height.
/// </remarks>
public class FullyRecursiveStringifier<TEdge, TNode, TPath, TBuilder> 
    : IStringifier<TEdge, TNode, TPath, TBuilder>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode, TPath, TBuilder>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode, TPath, TBuilder>
    where TPath : IRecImmDictIndexedTreePath<TEdge, TNode, TPath, TBuilder>
    where TBuilder : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TPath, TBuilder>, new()
{
    /// <summary>
    /// <inheritdoc/>
    /// By default is <see cref="Environment.NewLine"/>.
    /// </summary>
    public string NewLine { get; init; } = Environment.NewLine;

    /// <summary>
    /// <inheritdoc/>
    /// By default is 4 spaces.
    /// </summary>
    public string Indent { get; init; } = new string(' ', 4);

    /// <inheritdoc/>
    public Func<TNode, string> RootStringifier { get; init; }

    /// <inheritdoc/>
    public Func<TEdge, TNode, string> EdgeAndNodeStringifier { get; init; }

    /// <summary>
    /// Builds an instance of <see cref="FullyRecursiveStringifier{TEdge, TNode, TPath, TBuilder}"/> with the 
    /// provided stringifiers, for the root and for all other nodes in the three, and with default new line and indent.
    /// </summary>
    /// <param name="rootStringifier"><inheritdoc cref="RootStringifier" path="/summary"/></param>
    /// <param name="edgeAndNodeStringifier"><inheritdoc cref="EdgeAndNodeStringifier" path="/summary"/></param>
    public FullyRecursiveStringifier(
        Func<TNode, string> rootStringifier, Func<TEdge, TNode, string> edgeAndNodeStringifier)
    {
        RootStringifier = rootStringifier;
        EdgeAndNodeStringifier = edgeAndNodeStringifier;
    }

    /// <inheritdoc/>
    /// <inheritdoc cref="FullyRecursiveStringifier{TEdge, TNode, TPath, TBuilder}" path="/remarks"/>
    public string Stringify(TNode node)
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