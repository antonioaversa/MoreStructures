using MoreStructures.RecImmTrees;

namespace MoreStructures.SuffixStructures;

/// <summary>
/// An edge of a <see cref="ISuffixStructureEdge{TEdge, TNode}"/>, directionally linking two nodes and 
/// forming a path hop. Represents prefix matching.
/// </summary>
/// <typeparam name="TEdge">
///     <inheritdoc cref="IRecImmDictIndexedTreeEdge{TEdge, TNode}" path="/typeparam[@name='TEdge']"/>
/// </typeparam>
/// <typeparam name="TNode">
///     <inheritdoc cref="IRecImmDictIndexedTreeEdge{TEdge, TNode}" path="/typeparam[@name='TNode']"/>
/// </typeparam>
public interface ISuffixStructureEdge<TEdge, TNode> 
    : TextWithTerminator.ISelector, IRecImmDictIndexedTreeEdge<TEdge, TNode>
    where TEdge : ISuffixStructureEdge<TEdge, TNode>
    where TNode : ISuffixStructureNode<TEdge, TNode>
{
    /// <summary>
    /// The index of the first character of the edge string in the text.
    /// </summary>
    int Start { get; }

    /// <summary>
    /// The length of the edge string.
    /// </summary>
    int Length { get; }
}
