namespace MoreStructures.BurrowsWheelerTransform.Builders;

/// <summary>
/// Builds Burrows-Wheeler objects, such as <see cref="BWMatrix"/> and <see cref="BWTransform"/> of the provided
/// <see cref="TextWithTerminator"/>.
/// </summary>
public interface IBuilder
{
    /// <summary>
    /// Build a <see cref="BWMatrix"/> of the provided text, which is a 
    /// n-ary search tree in which edges coming out of a node are substrings of text which identify edges shared by all 
    /// paths to leaves, starting from the node.
    /// </summary>
    /// <param name="text">
    /// The text to build the BWM, with its terminator (required).
    /// </param>
    /// <returns>The matrix, wrapped into a <see cref="BWMatrix"/> object.</returns>
    /// <remarks>
    /// <inheritdoc cref="BWMatrix" path="/summary"/>
    /// </remarks>
    /// <example>
    ///     Code: 
    ///     <code>
    ///     var builder = ...
    ///     builder.BuildMatrix(new("mississippi")).Content
    ///     </code>
    ///     <br/>
    ///     
    ///     Result:
    ///     <code>
    ///     {
    ///         "$mississippi",
    ///         "i$mississipp",
    ///         "ippi$mississ",
    ///         "issippi$miss",
    ///         "ississippi$m",
    ///         "mississippi$",
    ///         "pi$mississip",
    ///         "ppi$mississi",
    ///         "sippi$missis",
    ///         "sissippi$mis",
    ///         "ssippi$missi",
    ///         "ssissippi$mi",
    ///     }
    ///     </code>
    /// </example>
    BWMatrix BuildMatrix(TextWithTerminator text);

    /// <summary>
    /// Rebuilds the original <see cref="BWMatrix"/> from a <see cref="BWTransform"/> 
    /// representing the last column of the Burrows-Wheeler Matrix (which is also the Burrows-Wheeler Transform).
    /// </summary>
    /// <param name="lastBWMColumn">The last column of the Burrows-Wheeler Matrix.</param>
    /// <returns>The matrix, wrapped into a <see cref="BWMatrix"/> object.</returns>
    /// <remarks>
    /// Because the entire Burrows-Wheeler Matrix is built from the text with an invertible function, and the same
    /// happens for the Burrows-Wheeler Transform of the text, it's possible to get back the entire matrix from its
    /// last column.
    /// </remarks>
    BWMatrix BuildMatrix(BWTransform lastBWMColumn);

    /// <summary>
    /// Builds the Burrows-Wheeler Transform from the provided <see cref="BWMatrix"/>.
    /// </summary>
    /// <param name="matrix">The matrix, whose BWT has to be calculated.</param>
    /// <returns>The transform, wrapped into a <see cref="BWTransform"/> object.</returns>
    /// <remarks>
    /// <inheritdoc cref="BWTransform" path="/summary"/>
    /// </remarks>
    BWTransform BuildTransform(BWMatrix matrix);

    /// <summary>
    /// Builds the Burrows-Wheeler Transform from the provided <see cref="TextWithTerminator"/>.
    /// </summary>
    /// <param name="text">The text, whose BWT has to be calculated.</param>
    /// <returns>The transform, wrapped into a <see cref="BWTransform"/> object.</returns>
    /// <remarks>
    /// <inheritdoc cref="BWTransform" path="/summary"/>
    /// </remarks>
    BWTransform BuildTransform(TextWithTerminator text);

    /// <summary>
    /// Rebuilds the original <see cref="TextWithTerminator"/> from the <see cref="BWMatrix"/>.
    /// </summary>
    /// <param name="matrix">The matrix, whose original text has to be calculated.</param>
    /// <returns>The text which corresponds to the provided matrix.</returns>
    TextWithTerminator InvertMatrix(BWMatrix matrix);

    /// <summary>
    /// Rebuilds the original <see cref="TextWithTerminator"/> from a <see cref="RotatedTextWithTerminator"/> 
    /// representing the last column of the Burrows-Wheeler Matrix (which is also the Burrows-Wheeler Transform).
    /// </summary>
    /// <param name="lastBWMColumn">The last column of the Burrows-Wheeler Matrix.</param>
    /// <returns>
    /// The text which corresponds to the provided text which produced a BWM whose last column is the one provided.
    /// </returns>
    /// <remarks>
    ///     <para id="terminator-required">
    ///     <paramref name="lastBWMColumn"/> requires a terminator to be specified in order to correctly compare 
    ///     strings, since the terminator should always be considered smaller than any other char.
    ///     </para>
    ///     <para id="possible-strategies">
    ///     Multiple strategies for inversion are possible: via n-mers construction, via last-first property, ...
    ///     </para>
    /// </remarks>
    TextWithTerminator InvertTransform(RotatedTextWithTerminator lastBWMColumn);
}

