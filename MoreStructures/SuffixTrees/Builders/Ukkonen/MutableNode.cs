namespace MoreStructures.SuffixTrees.Builders.Ukkonen;

/// <summary>
/// A node (root, internal or leaf) of the tree structure built by <see cref="UkkonenSuffixTreeBuilder"/>. 
/// </summary>
/// <param name="Id">
/// A unique identifier of the node in the tree. Useful for debugging and tracing.
/// </param>
/// <param name="LeafStart">
/// The index of the character, the path from the root leading to this leaf starts with. Non-null for leaves only.
/// </param>
/// <param name="SuffixLink">
/// The Suffix Link associated to this node. Defined for internal nodes only.
/// </param>
/// <remarks>
/// Mutable and having a <see cref="SuffixLink"/> to have Rule 2 Extension applied in constant time.
/// </remarks>
internal record MutableNode(int Id, int? LeafStart, MutableNode? SuffixLink)
{
    /// <summary>
    /// The children of this node. Empty for leaves, non-empty for root and internal nodes.
    /// </summary>
    public Dictionary<MutableEdge, MutableNode> Children { get; } = new();

    /// <summary>
    /// <inheritdoc cref="MutableNode" path="/param[@name='SuffixLink']"/>
    /// </summary>
    public MutableNode? SuffixLink { get; set; } = SuffixLink;

    /// <inheritdoc/>
    public override string ToString() => $"{Id}";

    /// <summary>
    /// Settings for the <see cref="Dump(DumpParams)"/> method.
    /// </summary>
    /// <param name="Text">The text, on which the Ukkonen algorithm is being run.</param>
    /// <param name="EdgeNodeSeparator">The string separator between pointing edge and pointed node.</param>
    /// <param name="SuffixLinkSeparator">The string separator between node indicator and its suffix link.</param>
    /// <param name="IndentationChar">The char to be used for indentation.</param>
    public record DumpParams(
        TextWithTerminator Text,
        string EdgeNodeSeparator = "->",
        string SuffixLinkSeparator = "~>",
        char IndentationChar = '\t');

    /// <summary>
    /// Dump the state of this node (with its entire <see cref="Children"/> structure.
    /// </summary>
    /// <param name="dumpParams">The parameters to be used to generate the output.</param>
    /// <returns>A string showing the state of the structure under this node.</returns>
    public string Dump(DumpParams dumpParams) =>
        DumpR(dumpParams, null, 0);

    private string DumpR(DumpParams dumpParams, string? prefix, int level) =>
        string.Join(
            Environment.NewLine,
            Enumerable
                .Repeat(
                    (prefix == null ? "" : $"{prefix} {dumpParams.EdgeNodeSeparator} ") +
                    ToString() +
                    (SuffixLink == null ? "" : $" {dumpParams.SuffixLinkSeparator} {SuffixLink}"),
                    1)
                .Concat(
                    from c in Children
                    let indentation = new string(dumpParams.IndentationChar, level + 1)
                    let edgeText = $"'{dumpParams.Text[c.Key.Start..(c.Key.End.Value + 1)]}'"
                    let edgeCompressed = $"{c.Key}"
                    select c.Value.DumpR(dumpParams, $"{indentation}{edgeText}{edgeCompressed}", level + 1)));
}
