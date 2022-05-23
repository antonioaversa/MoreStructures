namespace StringAlgorithms.RecImmTrees.Conversions
{
    /// <summary>
    /// <inheritdoc cref="IStringifier{TEdge, TNode}"/>
    /// Provides concrete properties and defaults for new line, identation and stringifiers.
    /// </summary>
    public abstract class StringifierBase<TEdge, TNode>
    {
        /// <summary>
        /// <inheritdoc cref="IStringifier{TEdge, TNode}.NewLine"/>
        /// By default is <see cref="Environment.NewLine"/>.
        /// </summary>
        public string NewLine { get; init; } = Environment.NewLine;

        /// <summary>
        /// <inheritdoc cref="IStringifier{TEdge, TNode}.Indent"/>
        /// By default is 4 spaces.
        /// </summary>
        public string Indent { get; init; } = new string(' ', 4);

        /// <inheritdoc cref="IStringifier{TEdge, TNode}.RootStringifier"/>
        public Func<TNode, string> RootStringifier { get; init; }

        /// <inheritdoc cref="IStringifier{TEdge, TNode}.EdgeAndNodeStringifier"/>
        public Func<TEdge, TNode, string> EdgeAndNodeStringifier { get; init; }

        /// <summary>
        /// Builds an instance of an <see cref="IStringifier{TEdge, TNode}"/> with the provided stringifiers, 
        /// for the root and for all other nodes in the three, and with default new line and indent.
        /// </summary>
        /// <param name="rootStringifier"><inheritdoc cref="RootStringifier" path="/summary"/></param>
        /// <param name="edgeAndNodeStringifier"><inheritdoc cref="EdgeAndNodeStringifier" path="/summary"/></param>
        protected StringifierBase(
            Func<TNode, string> rootStringifier, Func<TEdge, TNode, string> edgeAndNodeStringifier)
        {
            RootStringifier = rootStringifier;
            EdgeAndNodeStringifier = edgeAndNodeStringifier;
        }

        /// <inheritdoc cref="IStringifier{TEdge, TNode}.Stringify(TNode)"/>
        public abstract string Stringify(TNode node);
    }
}