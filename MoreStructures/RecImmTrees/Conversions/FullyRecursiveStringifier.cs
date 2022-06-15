using System.Text;

namespace MoreStructures.RecImmTrees.Conversions;

/// <summary>
/// <inheritdoc/>
/// Recursive implementation.
/// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
/// </summary>
/// <remarks>
///     <inheritdoc cref="IStringifier{TEdge, TNode}" path="/remarks"/>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     Implemented fully recursively, so limited by stack depth and usable with tree of a "reasonable" height.
///     </para>
/// </remarks>
public class FullyRecursiveStringifier<TEdge, TNode> 
    : StringifierBase<TEdge, TNode>, IStringifier<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>, IComparable<TEdge>
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
        foreach (var (childEdge, childNode) in node.Children.OrderBy(c => c.Key))
        {
            stringBuilder.Append(NewLine);
            for (int i = 0; i < level; i++)
                stringBuilder.Append(Indent);
            stringBuilder.Append($"{Indent}{EdgeAndNodeStringifier(childEdge, childNode)}");
            Stringify(stringBuilder, childNode, level + 1);
        }
    }


    /// <inheritdoc/>
    /// <inheritdoc cref="FullyRecursiveStringifier{TEdge, TNode}" path="/remarks"/>
    public override string Stringify(TreePath<TNode, TEdge> path) => 
        string.Join(PathSeparator, Stringify(path.PathNodes));

    private IEnumerable<string> Stringify(IEnumerable<KeyValuePair<TNode, TEdge>> pathNodes)
    {
        if (!pathNodes.Any())
            yield break;

        var nextPathNode = pathNodes.First();
        yield return EdgeAndNodeStringifier(nextPathNode.Value, nextPathNode.Key);

        foreach (var fragment in Stringify(pathNodes.Skip(1)))
            yield return fragment;
    }
}