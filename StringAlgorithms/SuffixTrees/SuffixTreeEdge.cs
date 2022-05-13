namespace StringAlgorithms.SuffixTrees;

/// <summary>
/// The index key of the collection of children of a Suffix Tree node, which identifies a non-empty substring in text, 
/// used as a selector to navigate the Suffix Tree.
/// </summary>
/// <param name="Start">The index of the first character of the edge string in the text.</param>
/// <param name="Length">The length of the edge string.</param>
public record SuffixTreeEdge(int Start, int Length)
{
    /// <summary>
    /// <inheritdoc cref="SuffixTreeEdge(int, int)" path="/param[@name='Start']"/>
    /// </summary>
    public int Start { get; init; } = Start >= 0 
        ? Start 
        : throw new ArgumentOutOfRangeException(nameof(Start), "Must be non-negative.");

    /// <summary>
    /// <inheritdoc cref="SuffixTreeEdge(int, int)" path="/param[@name='Length']"/>
    /// </summary>
    public int Length { get; init; } = Length >= 1
        ? Length
        : throw new ArgumentOutOfRangeException(nameof(Length), "Must be positive.");

    /// <summary>
    /// The possible adjacency order relationships between two edges by the strings they refer to in the text: 
    /// non-adjacent (overlapping on more than an extreme or not at all), adjacent with a specific order, or adjacent 
    /// with any order.
    /// </summary>
    [Flags]
    public enum AdjacencyOrder
    {
        None = 0,
        Before = 1,
        After = 2,
        BeforeOrAfter = 3
    }

    /// <summary>
    /// Whether this edge is in adjacency order w.r.t. the provided edge.
    /// </summary>
    /// <param name="other">The edge, to compare this edge against.</param>
    /// <param name="order">The adjacency relationship order to use for comparison.</param>
    /// <returns>True if the specified adjacency relationship is respected.</returns>
    public bool IsAdjacentTo(SuffixTreeEdge other, AdjacencyOrder order = AdjacencyOrder.BeforeOrAfter) =>
        (order.HasFlag(AdjacencyOrder.Before) && Start + Length == other.Start) || 
        (order.HasFlag(AdjacencyOrder.After) && other.Start + other.Length == Start);

    /// <summary>
    /// Returns the substring of the provided text with terminator, identified by this edge.
    /// </summary>
    /// <param name="text">The text, to apply the edge to.</param>
    /// <returns>The substring of text, as a plain string. Always non-empty.</returns>
    public string Of(TextWithTerminator text) => text.AsString.Substring(Start, Length);
}