using StringAlgorithms.SuffixStructures;

namespace StringAlgorithms.SuffixTrees;

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
public record SuffixTreeEdge(int Start, int Length)
    : ISuffixStructureEdge<SuffixTreeEdge, SuffixTreeNode>
{
    /// <inheritdoc/>
    public int Start { get; init; } = Start >= 0 
        ? Start 
        : throw new ArgumentOutOfRangeException(nameof(Start), "Must be non-negative.");

    /// <inheritdoc/>
    public int Length { get; init; } = Length >= 0
        ? Length
        : throw new ArgumentOutOfRangeException(nameof(Length), "Must be non-negative.");

    /// <inheritdoc/>
    public virtual string Of(TextWithTerminator text) => text[Start..(Start + Length)];
}