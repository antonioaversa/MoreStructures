using StringAlgorithms.RecImmTrees;

namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// An edge of a <see cref="ISuffixStructureEdge{TEdge, TNode, TBuilder}"/>, directionally linking two nodes and 
/// forming a path hop. Represents prefix matching.
/// </summary>
/// <typeparam name="TEdge">
///     <inheritdoc cref="IRecImmDictIndexedTreeEdge{TEdge, TNode, TBuilder}" path="/typeparam[@name='TEdge']"/>
/// </typeparam>
/// <typeparam name="TNode">
///     <inheritdoc cref="IRecImmDictIndexedTreeEdge{TEdge, TNode, TBuilder}" path="/typeparam[@name='TNode']"/>
/// </typeparam>
/// <typeparam name="TBuilder">
///     <inheritdoc cref="IRecImmDictIndexedTreeEdge{TEdge, TNode, TBuilder}" path="/typeparam[@name='TBuilder']"/>
/// </typeparam>
public interface ISuffixStructureEdge<TEdge, TNode, TBuilder> 
    : TextWithTerminator.ISelector, IRecImmDictIndexedTreeEdge<TEdge, TNode, TBuilder>
    where TEdge : ISuffixStructureEdge<TEdge, TNode, TBuilder>
    where TNode : ISuffixStructureNode<TEdge, TNode, TBuilder>
    where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TBuilder>
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
