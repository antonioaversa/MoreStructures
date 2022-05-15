﻿using StringAlgorithms.SuffixStructures;

namespace StringAlgorithms.SuffixTrees;

/// <summary>
/// The index key of the collection of children of a Suffix Tree node, which identifies a non-empty substring in text, 
/// used as a selector to navigate the Suffix Tree in text pattern matching.
/// </summary>
/// <param name="Start"><inheritdoc cref="ISuffixStructureEdge{TEdge, TNode, TPath}.Start" path="/summary"/></param>
/// <param name="Length"><inheritdoc cref="ISuffixStructureEdge{TEdge, TNode, TPath}.Length" path="/summary"/></param>
public record SuffixTreeEdge(int Start, int Length)
    : ISuffixStructureEdge<SuffixTreeEdge, SuffixTreeNode, SuffixTreePath, SuffixTreeBuilder>
{
    public int Start { get; init; } = Start >= 0 
        ? Start 
        : throw new ArgumentOutOfRangeException(nameof(Start), "Must be non-negative.");

    public int Length { get; init; } = Length >= 1
        ? Length
        : throw new ArgumentOutOfRangeException(nameof(Length), "Must be positive.");

    public virtual string Of(TextWithTerminator text) => text[Start..(Start + Length)];
}