namespace StringAlgorithms.SuffixStructures;

/// <summary>
/// Extension methods for all <see cref="ISuffixStructureBuilder{TEdge, TNode, TPath, TBuilder}"/> concretions.
/// </summary>
public static class SuffixStructureBuilderExtensions
{
    /// <summary>
    /// <inheritdoc cref="ISuffixStructureBuilder{TEdge, TNode, TPath, TBuilder}.BuildTree(TextWithTerminator)"/>
    /// </summary>
    /// <param name="text">
    /// The text to build the Suffix Structure of, without any terminator (automatically added).
    /// </param>
    /// <returns>
    /// <inheritdoc cref="ISuffixStructureBuilder{TEdge, TNode, TPath, TBuilder}.BuildTree(TextWithTerminator)"/>
    /// </returns>
    public static TNode BuildTree<TEdge, TNode, TPath, TBuilder>(
        this ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder> builder,
        string text)
        where TEdge : ISuffixStructureEdge<TEdge, TNode, TPath, TBuilder>
        where TNode : ISuffixStructureNode<TEdge, TNode, TPath, TBuilder>
        where TPath : ISuffixStructurePath<TEdge, TNode, TPath, TBuilder>
        where TBuilder : ISuffixStructureBuilder<TEdge, TNode, TPath, TBuilder> =>
        builder.BuildTree(new TextWithTerminator(text));
}
