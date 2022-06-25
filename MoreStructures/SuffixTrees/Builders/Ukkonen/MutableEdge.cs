namespace MoreStructures.SuffixTrees.Builders.Ukkonen;

/// <summary>
/// An edge of the tree structure built by <see cref="UkkonenSuffixTreeBuilder"/>, expressed with label compression. 
/// </summary>
/// <remarks>
/// Mutable and using a <see cref="MovingEnd"/> to have leaves updated by Rule 1 Extension in constant time.
/// </remarks>
internal class MutableEdge
{
    /// <summary>
    /// The index (in the text) of the first char of the label associated with this edge.
    /// </summary>
    public int Start { get; }

    /// <summary>
    /// The index (in the text) of the last char of the label associated with this edge.
    /// </summary>
    public MovingEnd End { get; set; }

    /// <summary>
    /// <inheritdoc cref="MutableEdge"/>
    /// </summary>
    /// <param name="start"><inheritdoc cref="Start" path="/summary"/></param>
    /// <param name="end"><inheritdoc cref="End" path="/summary"/></param>
    public MutableEdge(int start, MovingEnd end)
    {
        Start = start;
        End = end;
    }

    /// <summary>
    /// The length of this edge.
    /// </summary>
    /// <value>
    /// A positive value (at least 1).
    /// </value>
    public int Length => End.Value - Start + 1;

    /// <inheritdoc path="//*[not(self::summary)]"/>
    /// <summary>
    ///     <inheritdoc/>. 
    ///     <br/>
    ///     To assess equality, <see cref="Start"/> and <see cref="MovingEnd.Value"/> of <see cref="End"/> are 
    ///     compared.
    /// </summary>
    public override bool Equals(object? obj) =>
        obj is MutableEdge edge && Equals(Start, edge.Start) && Equals(End.Value, edge.End.Value);

    /// <inheritdoc path="//*[not(self::summary)]"/>
    /// <summary>
    ///     <inheritdoc/>. 
    ///     <br/>
    ///     Calculated based on <see cref="Start"/> only, since <see cref="End"/> is subject to mutation.
    /// </summary>
    public override int GetHashCode() =>
        Start;

    /// <summary>
    /// In the form [<see cref="Start"/>, <see cref="End"/>] or [<see cref="Start"/>, <see cref="End"/>*], if the
    /// <see cref="End"/> of this <see cref="MutableEdge"/> is the provided <paramref name="globalEnd"/>.
    /// </summary>
    /// <remarks>
    /// <inheritdoc/>
    /// </remarks>
    public string ToString(MovingEnd globalEnd) => 
        $"[{Start},{End}{(ReferenceEquals(End, globalEnd) ? "*" : string.Empty)}]";
}
