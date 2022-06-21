using MoreStructures.RecImmTrees;

namespace MoreStructures.SuffixStructures.Builders;

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
/// Object construction is externalized into a Builder object as a workaround to the limitation of not having 
/// constructor signatures directly in interfaces.
/// See https://codeblog.jonskeet.uk/2008/08/29/lessons-learned-from-protocol-buffers-part-4-static-interfaces/
/// </remarks>
public interface IBuilder<TEdge, TNode>
    where TEdge : ISuffixStructureEdge<TEdge, TNode>
    where TNode : ISuffixStructureNode<TEdge, TNode>
{
    /// <summary>
    /// Build a <see cref="ISuffixStructureEdge{TEdge, TNode}"/> of the provided text(s), which is a 
    /// n-ary search tree in which edges coming out of a node are substrings of at least one of the texts and nodes are
    /// not directly labeled instead.
    /// </summary>
    /// <param name="texts">
    /// The text(s) to build the Suffix Structure of, each one with its own unique terminator (required for traversal).
    /// </param>
    /// <returns>The root node of the Suffix Structure.</returns>
    /// <remarks>
    /// <para id="info-terminators">
    ///     TERMINATORS
    ///     <br/>
    ///     Terminators have to be unique characters, not just on the single <see cref="TextWithTerminator"/> they are 
    ///     appended to, but to all <see cref="TextWithTerminator"/> items in <paramref name="texts"/>.
    ///     <br/>
    ///     That means that if <paramref name="texts"/> is (T1, ..., Tn), where each <see cref="TextWithTerminator"/>
    ///     Ti has ti as <see cref="TextWithTerminator.Terminator"/>, ti should not be in any of the
    ///     <see cref="TextWithTerminator.Text"/> of Tj, for any j.
    /// </para>
    /// <para id="info-generalizedsuffixstructures">
    ///     GENERALIZED SUFFIX STRUCTURES
    ///     <br/>
    ///     When multiple <see cref="TextWithTerminator"/> instances are passed into <paramref name="texts"/>, the 
    ///     Suffix Structure built is known as Generalized Suffix Structure (e.g. Generalized Suffix Tree or Trie). 
    ///     <br/>
    ///     It differs from a normal Suffix Structure built for the concatenation of items in <paramref name="texts"/>
    ///     by the fact that it doesn't contain any root-to-leaf path identifying a suffix which spans multiple texts.
    ///     <br/>
    ///     When a Suffix Structure is built for a single <see cref="TextWithTerminator"/> which a concatenation of
    ///     multiple texts, each with its own terminator <c>T1 || t1 || ... || Tn || tn</c>, the resulting structure 
    ///     has branches which span over multiple texts, such as <c>suffixOf[Ti] || ti || prefixOf[T(i+1)]</c> or 
    ///     <c>suffixOf[Ti] || ti || T(i+1) || t(i+1) || prefixOf[T(i+2)]</c>.
    ///     <br/>
    ///     A Generalized Suffix Structure trims those branches in construction, so that each node-to-leaf branch ends
    ///     with any of the terminator t1, ..., tn (not always tn) and doesn't contain any other terminator except the 
    ///     ending one.
    /// </para>
    /// <br/>
    /// <para id="info-edges">
    ///     EDGES
    ///     <br/>
    ///     Substrings of a text are identified by their start position in text and their length, rather than by a copy 
    ///     of the substring itself. 
    ///     <br/>
    ///     The technique, known as <b>Edge Compression</b> allows to store edges information in constant space, and 
    ///     the entire tree in linear space w.r.t. the number of nodes in the tree (which can be linear or not in the 
    ///     input, depending on the type of suffix structure).
    ///     <br/>
    ///     The sequence of edges in root-to-node paths in the tree identify prefixes in common to multiple suffixes of 
    ///     the text(s).
    /// </para>
    /// </remarks>
    TNode BuildTree(params TextWithTerminator[] texts);
}
