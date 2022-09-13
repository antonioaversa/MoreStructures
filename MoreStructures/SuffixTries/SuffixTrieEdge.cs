using MoreStructures.SuffixStructures;
using MoreStructures.SuffixTrees;

namespace MoreStructures.SuffixTries;

/// <summary>
/// The index key of the collection of children of a <see cref="SuffixTrieNode"/>, which identifies a single char in 
/// text, used as a selector to navigate the <see cref="SuffixTrieNode"/> in text pattern matching.
/// </summary>
/// <param name="Index">The index of the character in the text.</param>
/// <remarks>
/// Supports <see cref="IComparable{T}"/>, by <see cref="Index"/>.
/// </remarks>
public record SuffixTrieEdge(int Index)
    : SuffixTreeEdge(Index, 1), ISuffixStructureEdge<SuffixTrieEdge, SuffixTrieNode>, IComparable<SuffixTrieEdge>
{
    /// <inheritdoc path="//*[not(self::summary)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Comparison is done by <see cref="Index"/>: lower is smaller, higher is bigger.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="other"/> is not a <see cref="SuffixTrieEdge"/>.
    /// </exception>
    public int CompareTo(SuffixTrieEdge? other) => 
        other != null
        ? Index - other.Index 
        : throw new ArgumentException($"Invalid comparison: cannot compare to null.");

    /// <inheritdoc path="//*[not(self::summary)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Generates a string in the form "(<see cref="SuffixTreeEdge.Start"/>)". <see cref="SuffixTreeEdge.Length"/>
    ///     is not included as in this context it is always 1.
    /// </summary>
    public override string ToString() => $"({Start})";
}