namespace MoreStructures.RecImmTrees.Conversions;

/// <summary>
/// A converter from <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> structures to string.
/// </summary>
/// <remarks>
///     <para>
///     Requires <typeparamref name="TEdge"/> to implement <see cref="IComparable{T}"/>, so that output lines are 
///     sorted.
///     </para>
///     <para id="complexity">
///     Time Complexity = O(n) and Space Complexity = O(n) where n = number of nodes in the 
///     <typeparamref name="TNode"/> structure. Each node and its incoming edge is visited once.
///     </para>
/// </remarks>
public interface IStringifier<TEdge, TNode>
    where TEdge : IRecImmDictIndexedTreeEdge<TEdge, TNode>, IComparable<TEdge>
    where TNode : IRecImmDictIndexedTreeNode<TEdge, TNode>
{
    /// <summary>
    /// The character or string used to separate lines of the output.
    /// </summary>
    string NewLine { get; init; }

    /// <summary>
    /// The character or string used to indent output, to visually express tree levels.
    /// </summary>
    /// <example>
    /// 2 or 4 spaces, a tab, ...
    /// </example>
    string Indent { get; init; }

    /// <summary>
    /// A function mapping the top-level node to a string. Used for the first line of the output.
    /// </summary>
    Func<TNode, string> RootStringifier { get; init; }

    /// <summary>
    /// A function mapping the provided edge and node to a string. Used for all lines of the output but the first.
    /// </summary>
    Func<TEdge, TNode, string> EdgeAndNodeStringifier { get; init; }

    /// <summary>
    /// Converts the provided <see cref="IRecImmDictIndexedTreeNode{TEdge, TNode}"/> into a string.
    /// </summary>
    /// <param name="node">The root of the tree to stringify.</param>
    /// <returns>A string version of the provided structure.</returns>
    /// <example>
    /// Using 4 spaces as indent, RootStringifier = n => "R", an EdgeAndNodeStringifier = (e, n) => $"{e} -> N":
    /// R
    ///     e1 -> N
    ///         e3 -> N
    ///         e4 -> N
    ///             e5 -> N
    ///     e2 -> N
    ///         e6 -> N
    /// </example>
    string Stringify(TNode node);
}
