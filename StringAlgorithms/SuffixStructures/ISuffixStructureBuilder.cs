using StringAlgorithms.RecImmTrees;

namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// Builds objects, such as edges, nodes and paths, for the <see cref="ISuffixStructureNode{TEdge, TNode, TPath, TBuilder}"/> 
/// concretion it is the builder of.
/// </summary>
/// <typeparam name="TEdge">
///     <inheritdoc cref="IRecImmDictIndexedTreeBuilder{TEdge, TNode, TPath, TBuilder}" path="/typeparam[@name='TEdge']"/>
/// </typeparam>
/// <typeparam name="TNode">
///     <inheritdoc cref="IRecImmDictIndexedTreeBuilder{TEdge, TNode, TPath, TBuilder}" path="/typeparam[@name='TNode']"/>
/// </typeparam>
/// <typeparam name="TPath">
///     <inheritdoc cref="IRecImmDictIndexedTreeBuilder{TEdge, TNode, TPath, TBuilder}" path="/typeparam[@name='TPath']"/>
/// </typeparam>
/// <typeparam name="TBuilder">
///     <inheritdoc cref="IRecImmDictIndexedTreeBuilder{TEdge, TNode, TPath, TBuilder}" path="/typeparam[@name='TBuilder']"/>
/// </typeparam>
public interface ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>
    : IRecImmDictIndexedTreeBuilder<TEdge, TNode, TPath, TBuilder>
    where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
    where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
    where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
    where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder>
{
    #region TNode building

    /// <summary>
    /// Build a <see cref="ISuffixStructureEdge{TEdge, TNode, TPath, TBuilder}"/> of the provided text, which is a 
    /// n-ary search tree in which edges coming out of a node are substrings of text which identify edges shared by all 
    /// paths to leaves, starting from the node.
    /// </summary>
    /// <param name="text">
    /// The text to build the Suffix Structure of, with its terminator (required for traversal).
    /// </param>
    /// <returns>The root node of the Suffix Structure.</returns>
    /// <remarks>
    /// Substrings of text are identified by their start position in text and their length.
    /// </remarks>
    TNode BuildTree(TextWithTerminator text);

    #endregion
}
