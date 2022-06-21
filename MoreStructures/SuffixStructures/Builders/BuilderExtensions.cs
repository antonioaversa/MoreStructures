namespace MoreStructures.SuffixStructures.Builders;

/// <summary>
/// Extension methods for all <see cref="IBuilder{TEdge, TNode}"/> concretions.
/// </summary>
public static class BuilderExtensions
{
    /// <summary>
    /// <inheritdoc cref="IBuilder{TEdge, TNode}.BuildTree(TextWithTerminator[])"/>
    /// Text is provided as a plain string.
    /// </summary>
    /// <param name="builder">
    /// The builder to be used, to build the structure.
    /// </param>
    /// <param name="text">
    /// The text to build the structure of, without any terminator (automatically added).
    /// </param>
    /// <returns>
    /// <inheritdoc cref="IBuilder{TEdge, TNode}.BuildTree(TextWithTerminator[])"/>
    /// </returns>
    public static TNode BuildTree<TEdge, TNode>(
        this IBuilder<TEdge, TNode> builder,
        string text)
        where TEdge : ISuffixStructureEdge<TEdge, TNode>
        where TNode : ISuffixStructureNode<TEdge, TNode> =>
        builder.BuildTree(new TextWithTerminator(text));
}
