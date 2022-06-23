namespace MoreStructures.SuffixTrees.Builders.Ukkonen;

/// <summary>
/// An edge of the tree structure built by <see cref="UkkonenSuffixTreeBuilder"/>, expressed with label compression. 
/// </summary>
/// <param name="Start">The index (in the text) of the first char of the label associated with this edge.</param>
/// <param name="End">The index (in the text) of the last char of the label associated with this edge.</param>
/// <remarks>
/// Mutable and using a <see cref="MovingEnd"/> to have leaves updated by Rule 1 Extension in constant time.
/// </remarks>
internal record MutableEdge(int Start, MovingEnd End)
{
    /// <summary>
    /// The length of this edge.
    /// </summary>
    /// <value>
    /// A positive value (at least 1).
    /// </value>
    public int Length => End.Value - Start + 1;

    /// <summary>
    /// In the form (<see cref="Start"/>, <see cref="End"/>).
    /// </summary>
    /// <remarks>
    /// <inheritdoc/>
    /// </remarks>
    public override string ToString() => $"[{Start},{End}]";
}
