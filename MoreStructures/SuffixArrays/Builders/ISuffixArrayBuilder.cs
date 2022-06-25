using MoreStructures.SuffixStructures;

namespace MoreStructures.SuffixArrays.Builders;

/// <summary>
/// An algorithm for building Suffix Arrays.
/// </summary>
/// <remarks>
/// The Suffix Array of a terminator-terminated text T is an array where the i-th element is the index in T of the 1st 
/// char of a suffix Si of T, and Si &lt; Sj for each i &lt; j.
/// </remarks>
/// <typeparam name="TEdge">The type of edges of the specific structure.</typeparam>
/// <typeparam name="TNode">The type of nodes of the specific structure.</typeparam>
public interface ISuffixArrayBuilder<TEdge, TNode>
    where TEdge : ISuffixStructureEdge<TEdge, TNode>
    where TNode : ISuffixStructureNode<TEdge, TNode>
{
    /// <summary>
    /// Builds the Suffix Array.
    /// </summary>
    /// <returns>A <see cref="IEnumerable{T}"/> of <see cref="int"/>.</returns>
    IEnumerable<int> Build();
}
