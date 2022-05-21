using StringAlgorithms.RecImmTrees;

namespace StringAlgorithms.SuffixStructures.Builders;

/// <summary>
/// Builds objects, such as edges and nodes, for the <see cref="ISuffixStructureNode{TEdge, TNode}"/> 
/// concretion it is the builder of.
/// </summary>
/// <typeparam name="TEdge">
///     <inheritdoc cref="IRecImmDictIndexedTreeEdge{TEdge, TNode}" path="/typeparam[@name='TEdge']"/>
/// </typeparam>
/// <typeparam name="TNode">
///     <inheritdoc cref="IRecImmDictIndexedTreeNode{TEdge, TNode}" path="/typeparam[@name='TNode']"/>
/// </typeparam>
/// <remarks>
/// This interface allows to have a shared construction interface for objects among all structures.
/// It is a workaround to the limitation of not having constructor signatures in interfaces.
/// See https://codeblog.jonskeet.uk/2008/08/29/lessons-learned-from-protocol-buffers-part-4-static-interfaces/
/// </remarks>
public interface IBuilder<TEdge, TNode>
    where TEdge : ISuffixStructureEdge<TEdge, TNode>
    where TNode : ISuffixStructureNode<TEdge, TNode>
{
    /// <summary>
    /// Build a <see cref="ISuffixStructureEdge{TEdge, TNode}"/> of the provided text, which is a 
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
}
