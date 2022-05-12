namespace StringAlgorithms;

/// <summary>
/// The index key of the collection of children of a Suffix Tree node, which identifies a substring in text, used 
/// as a selector to navigate the Suffix Tree.
/// </summary>
/// <param name="Start">The index of the first character of the prefix path in the text.</param>
/// <param name="Length">The length of the prefix path.</param>
public record PrefixPath(int Start, int Length)
{
    /// <summary>
    /// The possible adjacency order relationships between two prefix paths: non-adjacent (overlapping on more than
    /// an extreme or not at all), adjacent with a specific order, or adjacent with any order.
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
    /// Whether this prefix path is in adjacency order w.r.t. the provided prefix path.
    /// </summary>
    /// <param name="prefixPath2">The prefix path, to compare this prefix path against.</param>
    /// <param name="order">The adjacency relationship order to use for comparison.</param>
    /// <returns>True if the specified adjacency relationship is respected.</returns>
    public bool IsAdjacentTo(PrefixPath prefixPath2, AdjacencyOrder order = AdjacencyOrder.BeforeOrAfter) =>
        (order.HasFlag(AdjacencyOrder.Before) && Start + Length == prefixPath2.Start) || 
        (order.HasFlag(AdjacencyOrder.After) && prefixPath2.Start + prefixPath2.Length == Start);

    /// <summary>
    /// Returns the substring of the provided text with terminator, identified by this prefix path.
    /// </summary>
    /// <param name="text">The text, to apply the prefix path to.</param>
    /// <returns>The substring of text, as a plain string.</returns>
    public string Of(TextWithTerminator text) => text.AsString.Substring(Start, Length);
}