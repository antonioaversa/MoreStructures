namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// The possible adjacency order relationships between two edges by the parts they refer to in the text: 
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
