namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// Extension methods for all <see cref="ISuffixStructureBuilder{TEdge, TNode, TBuilder}"/> concretions.
/// </summary>
public static class SuffixStructureBuilderExtensions
{
    /// <summary>
    /// <inheritdoc cref="ISuffixStructureBuilder{TEdge, TNode, TBuilder}.BuildTree(TextWithTerminator)"/>
    /// Text is provided as a plain string.
    /// </summary>
    /// <param name="builder">
    /// The builder to be used, to build the structure.
    /// </param>
    /// <param name="text">
    /// The text to build the structure of, without any terminator (automatically added).
    /// </param>
    /// <returns>
    /// <inheritdoc cref="ISuffixStructureBuilder{TEdge, TNode, TBuilder}.BuildTree(TextWithTerminator)"/>
    /// </returns>
    public static TNode BuildTree<TEdge, TNode, TBuilder>(
        this ISuffixStructureBuilder<TEdge, TNode, TBuilder> builder,
        string text)
        where TEdge : ISuffixStructureEdge<TEdge, TNode, TBuilder>
        where TNode : ISuffixStructureNode<TEdge, TNode, TBuilder>
        where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TBuilder> =>
        builder.BuildTree(new TextWithTerminator(text));
}
