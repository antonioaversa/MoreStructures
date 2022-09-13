namespace MoreStructures.SuffixTrees.Builders.Ukkonen;

/// <summary>
/// An object wrapping an integer value, using as a auto-updating end for all leaves of the tree beeing built by
/// <see cref="UkkonenSuffixTreeBuilder"/>. Makes Rule 1 Extension a constant-time operation.
/// </summary>
internal class MovingEnd
{
    /// <summary>
    /// The current value of the end.
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// <inheritdoc cref="MovingEnd"/>
    /// </summary>
    /// <param name="value"><inheritdoc cref="Value"/></param>
    public MovingEnd(int value)
    {
        Value = value;
    }

    /// <inheritdoc path="//*[not(self::summary)]"/>
    /// <summary>
    ///     <inheritdoc/>
    ///     <br/>
    ///     Shows the undelying <see cref="Value"/>.
    /// </summary>
    public override string ToString() => Value.ToString();
}
