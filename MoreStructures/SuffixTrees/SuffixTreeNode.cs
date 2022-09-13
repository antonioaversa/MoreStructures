using MoreStructures.RecImmTrees;
using MoreStructures.RecImmTrees.Conversions;
using MoreStructures.SuffixStructures;
using MoreStructures.Utilities;

namespace MoreStructures.SuffixTrees;

/// <summary>
/// An immutable node of an immutable Suffix Tree, recursively pointing to its children nodes via 
/// <see cref="SuffixTreeEdge"/> instances, associated with selector strings.
/// </summary>
/// <param name="Children">The collection of children for the node, indexed by string edges.</param>
/// <param name="Start">
/// <inheritdoc cref="ISuffixStructureNode{TEdge, TNode}.Start" path="/summary"/>
/// </param>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     Suffix Trees are more space-efficient than Suffix Tries due to the reduced number of 
///     <see cref="SuffixTreeEdge"/> and their <see cref="SuffixTreeNode"/>, compare to the corresponding
///     <see cref="SuffixTries.SuffixTrieEdge"/> and their <see cref="SuffixTries.SuffixTrieNode"/>, due to entire 
///     paths of single chains of chars in Suffix Tries being coalesced into a single selector string, which is stored
///     on the edge with path label compression, i.e. using two fixed sized numbers (<see cref="SuffixTreeEdge.Start"/> 
///     and <see cref="SuffixTreeEdge.Length"/>) instead of a variable-length string of characters.
///     <br/>
///     Furthermore, suffix trees, unlike suffix tries, can be constructed in linear time, for example via the 
///     <see cref="Builders.UkkonenSuffixTreeBuilder"/>.
///     </para>
///     <para id="immutability">
///     IMMUTABILITY
///     <br/>
///     Immutability is guaranteed by using <see cref="ValueReadOnlyCollection{T}"/>.
///     </para>
/// </remarks>
public abstract record SuffixTreeNode(IDictionary<SuffixTreeEdge, SuffixTreeNode> Children, int? Start)
    : ISuffixStructureNode<SuffixTreeEdge, SuffixTreeNode>
{
    /// <summary>
    /// Builds an intermediate node, i.e. a node with children and their corresponding incoming edges.
    /// </summary>
    public record Intermediate(IDictionary<SuffixTreeEdge, SuffixTreeNode> Children)
        : SuffixTreeNode(Children, null)
    {
        /// <inheritdoc/>
        public override bool IsEquivalentTo(SuffixTreeNode other, params TextWithTerminator[] texts) =>
            other is Intermediate(var otherChildren) &&
            SuffixTreeNode.HaveEquivalentChildren(texts.GenerateFullText().fullText, Children, otherChildren);
    }

    /// <summary>
    /// Builds a leaf, i.e. a node with no children and the start index of the suffix in the text.
    /// </summary>
    public record Leaf(int LeafStart)
        : SuffixTreeNode(new Dictionary<SuffixTreeEdge, SuffixTreeNode> { }, LeafStart)
    {
        /// <inheritdoc/>
        public override bool IsEquivalentTo(SuffixTreeNode other, params TextWithTerminator[] texts) =>
            other is Leaf(var otherLeafStart) && 
            LeafStart == otherLeafStart;
    }

    /// <inheritdoc/>
    public IDictionary<SuffixTreeEdge, SuffixTreeNode> Children { get; } 
        = (Children.Any() == (Start == null)) && Start.GetValueOrDefault(0) >= 0
        ? Children.ToValueReadOnlyDictionary()
        : throw new ArgumentException($"Leafs needs to specificy a non-negative {nameof(Start)}.", nameof(Children));

    /// <inheritdoc/>
    public int? Start { get; } = Start;

    /// <summary>
    /// Indexes into the children of this node, by edge.
    /// </summary>
    public SuffixTreeNode this[SuffixTreeEdge edge] => Children[edge];

    private static readonly IStringifier<SuffixTreeEdge, SuffixTreeNode> Stringifier =
        new FullyIterativeStringifier<SuffixTreeEdge, SuffixTreeNode>(
            r => r.IsLeaf() ? $"R from {r.Start}" : "R",
            (e, n) => $"{e} -> {(n.IsLeaf() ? $"L from {n.Start}" : "I")}")
            {
                StopIndentingLevel = 10,
            };

    /// <inheritdoc path="//*[not(self::summary or self::remarks)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Uses a <see cref="IStringifier{TEdge, TNode}"/> to generate the string which show the node and its 
    ///     underlying structure.
    /// </summary>
    /// <remarks>
    /// Sealed to prevent compiler from superceding <see cref="ToString()"/> in derived record.
    /// </remarks>
    public override sealed string ToString() => Stringifier.Stringify(this);

    /// <summary>
    /// Determines whether this <see cref="SuffixTreeNode"/> structure is equivalent to the provided 
    /// <paramref name="other"/> structure, w.r.t. the provided <paramref name="texts"/>.
    /// </summary>
    /// <param name="other">The other tree, to compare this tree against.</param>
    /// <param name="texts">The text, based on which to evaluate the tree equivalence.</param>
    /// <returns>True if the two trees are equivalent for the provided text, false otherwise.</returns>
    /// <remarks>
    ///     <para id="definition">
    ///     DEFINITION
    ///     <br/>
    ///     The definition of <see cref="SuffixTreeNode"/> structures equivalence depends on (a list of)
    ///     <see cref="TextWithTerminator"/>: two trees can be equivalent for some texts, and not equivalent
    ///     for others. This is because of the <b>Edge Compression</b> technique used by
    ///     <see cref="SuffixTreeEdge"/>, to store edges in O(1) space, instead of O(n).
    ///     <br/>
    ///     Moreover, equivalence is based on the equivalence of edges and of the specific type of node.
    ///     <br/>
    ///     - Two <see cref="SuffixTreeEdge"/> E1 and E2 are equivalent w.r.t. a <see cref="TextWithTerminator"/> T, 
    ///       if the string identified by each edge is the same: <c>T[E1] == T[E2]</c>.
    ///       <br/>
    ///     - Two <see cref="Leaf"/> L1 and L2 are equivalent if their <see cref="Leaf.LeafStart"/> is the same:
    ///       <c>L1.LeafStart == L2.LeafStart</c>.
    ///       <br/>
    ///     - Two <see cref="Intermediate"/> I1 and I2 are equivalent if there is a 1-to-1 correspondence between 
    ///       children edges and child nodes pointed by such edges are equivalent: 
    ///       <c>for each edge e1 of I1.Children there exists exactly one equivalent edge e2 of I2.Children and
    ///       I1.Children[e1] is equivalent to I2.Children[e2] w.r.t. T</c>.
    ///     </para>
    ///     <para id="complexity">
    ///     COMPLEXITY
    ///     <br/>
    ///     - Checking for edges equivalence is a O(n) operation, where n is the number of nodes in the tree, since it 
    ///       requires decompressing the labels of each of the edges being compare and compare for equality the two 
    ///       resulting O(n) strings.
    ///       <br/>
    ///     - Checking for leaves equivalence is a O(1) operation, since it only requires comparing 
    ///       <see cref="Leaf.LeafStart"/>, which has constant size.
    ///       <br/>
    ///     - Checking for intermediate nodes equivalence is a O(n) operation, amortized cost over the total number of 
    ///       edges in the tree, since there are O(n) edges in total in the tree, an edge is visited multiple times
    ///       but always in the context of a single node, and the node equivalence check requires looking for the 
    ///       equivalent edge.
    ///       <br/>
    ///     - In conclusion Time Complexity is O(n^2) and Space Complexity is O(n).
    ///     </para>
    /// </remarks>
    public abstract bool IsEquivalentTo(SuffixTreeNode other, params TextWithTerminator[] texts);

    private static bool HaveEquivalentChildren(
        TextWithTerminator fullText,
        IDictionary<SuffixTreeEdge, SuffixTreeNode> thisChildren,
        IDictionary<SuffixTreeEdge, SuffixTreeNode> otherChildren)
    {
        foreach (var (childEdge, childNode) in thisChildren)
        {
            var correspondingEdgeAndNode = otherChildren
                .Where(edgeAndNode => fullText[childEdge] == fullText[edgeAndNode.Key])
                .ToList();
            if (correspondingEdgeAndNode.Count != 1)
                return false;
            if (!childNode.IsEquivalentTo(correspondingEdgeAndNode[0].Value, fullText))
                return false;
        }

        return true;
    }
}
