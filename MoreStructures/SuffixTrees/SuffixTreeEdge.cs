using MoreStructures.SuffixStructures;

namespace MoreStructures.SuffixTrees;

/// <summary>
/// The index key of the collection of children of a <see cref="SuffixTreeNode"/>, which identifies a non-empty 
/// substring in text used as a selector to navigate the <see cref="SuffixTreeNode"/> in text pattern matching.
/// </summary>
/// <param name="Start">
///     <inheritdoc cref="ISuffixStructureEdge{TEdge, TNode}.Start" path="/summary"/>
/// </param>
/// <param name="Length">
///     <inheritdoc cref="ISuffixStructureEdge{TEdge, TNode}.Length" path="/summary"/>
/// </param>
/// Supports <see cref="IComparable{T}"/>, by <see cref="Start"/> and <see cref="Length"/>, in this order.
public record SuffixTreeEdge(int Start, int Length)
    : ISuffixStructureEdge<SuffixTreeEdge, SuffixTreeNode>, IComparable<SuffixTreeEdge>
{
    /// <inheritdoc/>
    /// <inheritdoc/>
    public int Start { get; init; } = Start >= 0 
        ? Start 
        : throw new ArgumentOutOfRangeException(nameof(Start), "Must be non-negative.");

    /// <inheritdoc/>
    public int Length { get; init; } = Length >= 0
        ? Length
        : throw new ArgumentOutOfRangeException(nameof(Length), "Must be non-negative.");

    /// <summary>
    /// <inheritdoc/>
    /// Comparison is done by <see cref="Start"/> first, then <see cref="Length"/>: lower is smaller, higher is bigger.
    /// </summary>
    /// <param name="other"><inheritdoc/></param>
    /// <returns><inheritdoc/></returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="other"/> is not a <see cref="SuffixTreeEdge"/>.
    /// </exception>
    public int CompareTo(SuffixTreeEdge? other)
    {
        if (other == null)
            throw new ArgumentException($"Invalid comparison: cannot compare to null.");

        var startComparison = Start - other.Start;
        if (startComparison != 0)
            return startComparison;
        return Length - other.Length;
    }

    /// <inheritdoc/>
    public virtual string Of(TextWithTerminator text) => 
        string.Concat(text[Start..(Start + Length)])[0..Length];

    /// <inheritdoc/>
    public virtual string OfRotated(RotatedTextWithTerminator text) => 
        string.Concat(text[Start..(Start + Length)])[0..Length];
}