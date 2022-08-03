namespace MoreStructures.SuffixTrees.Builders;

/// <summary>
/// An internal representation of a mutable recursive tree structure, used by algorithms building a tree iteratively
/// by performing mutations. 
/// </summary>
/// <remarks>
/// - Allows mutation of each of its field and properties. 
///   <br/>
/// - Its sub-collections, such as <see cref="Node.Children"/> are mutable.
///   <br/>
/// - Used by <see cref="SuffixAndLcpArraysBasedSuffixTreeBuilder"/> to build its working copy of the tree, before
///   producing the final <see cref="SuffixTreeNode"/> structure, which is immutable.
/// </remarks>
internal static class MutableTree
{
    /// <summary>
    /// The edge of a <see cref="MutableTree"/>, indentified by the <see cref="Start"/> index in the text and the 
    /// <see cref="Length"/> of the label associated with the edge (a technique to efficiently store labels in a
    /// Suffix Tree also known as <b>Edge Compression</b>).
    /// </summary>
    public struct Edge
    {
        /// <summary>
        /// The index in the text of the char by which the label of this edge starts. 
        /// </summary>
        /// <remarks>
        /// Should be non-negative and smaller than text length (where length includes the terminator, if any).
        /// </remarks>
        public int Start;

        /// <summary>
        /// The length of the label of this edge.
        /// </summary>
        /// <remarks>
        /// Should be positive (empty edges not allowed), with <see cref="Start"/> + <see cref="Length"/> &lt;= text 
        /// length.
        /// </remarks>
        public int Length;

        private Edge(int start, int length)
        {
            Start = start;
            Length = length;
        }

        /// <summary>
        /// Builds a <see cref="Edge"/> with the provided <paramref name="start"/> and <paramref name="length"/>.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Edge Build(int start, int length) =>
            new(start, length);
    }

    /// <summary>
    /// The node of a <see cref="MutableTree"/>, with a back-reference to its <see cref="ParentNode"/>, a 
    /// back-reference to its <see cref="IncomingEdge"/>, a collection of <see cref="Children"/> (non-empty for 
    /// intermediate nodes only) and a <see cref="LeafStart"/> index (non-null for leaves only).
    /// </summary>
    public sealed class Node
    {
        /// <summary>
        /// A back-reference to the <see cref="Node"/> above this one in the tree.
        /// </summary>
        /// <remarks>
        /// It's the node itself for the root of the tree, built via <see cref="BuildRoot"/>.
        /// </remarks>
        public Node ParentNode;

        /// <summary>
        /// A back-reference to the <see cref="Edge"/> pointing to this node from the <see cref="ParentNode"/>.
        /// </summary>
        /// <remarks>
        /// It's a dummy edge (0, 0) for the root of the tree, built via <see cref="BuildRoot"/>.
        /// </remarks>
        public Edge IncomingEdge;

        /// <summary>
        /// A mutable collection of <see cref="Node"/> instances, indexed by <see cref="Edge"/> instances.
        /// </summary>
        public Dictionary<Edge, Node> Children;

        /// <summary>
        /// The leaf start index in the text, of this node. Only applicable to leaves.
        /// </summary>
        public int? LeafStart;

        private Node(Node? parent, Edge incomingEdge, int? leafStart)
        {
            ParentNode = parent ?? this;
            IncomingEdge = incomingEdge;
            Children = new Dictionary<Edge, Node>();
            LeafStart = leafStart;
        }

        /// <summary>
        /// Builds a <see cref="Node"/> instance, representing the root node of a tree, with <see cref="ParentNode"/>
        /// equal to the node itself (circular reference), a dummy edge (0, 0) as <see cref="IncomingEdge"/> and null
        /// <see cref="LeafStart"/>.
        /// </summary>
        /// <returns>A <see cref="Node"/> instance.</returns>
        public static Node BuildRoot() => 
            new(null, Edge.Build(0, 0), null);

        /// <summary>
        /// Builds a <see cref="Node"/> instance, representing a non-root node of a tree (intermediate or leaf), with 
        /// the provided <paramref name="parentNode"/>, <paramref name="incomingEdge"/> and 
        /// <paramref name="leafStart"/> (only applicable for leaves).
        /// </summary>
        /// <param name="parentNode">
        ///     <inheritdoc cref="ParentNode" path="/summary"/>
        /// </param>
        /// <param name="incomingEdge">
        ///     <inheritdoc cref="IncomingEdge" path="/summary"/>
        /// </param>
        /// <param name="leafStart">
        ///     <inheritdoc cref="LeafStart" path="/summary"/>
        /// </param>
        /// <returns>A <see cref="Node"/> instance.</returns>
        public static Node Build(Node parentNode, Edge incomingEdge, int? leafStart) => 
            new(parentNode, incomingEdge, leafStart);
    }
}