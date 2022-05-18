namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// The possible adjacency order relationships between two edges by the parts they refer to in the text.
/// </summary>
[Flags]
public enum AdjacencyOrder
{
    /// <summary>
    /// Non-adjacent (overlapping on more than an extreme or not at all).
    /// </summary>
    None = 0,

    /// <summary>
    /// Adjacent in a specific order: the first comes before the second.
    /// </summary>
    Before = 1,

    /// <summary>
    /// Adjacent in a specific order: the first comes after the second.
    /// </summary>
    After = 2,

    /// <summary>
    /// Adjacent in any order.
    /// </summary>
    BeforeOrAfter = 3
}
