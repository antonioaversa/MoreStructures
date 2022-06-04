using MoreLinq;
using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform;

/// <summary>
/// The Burrows-Wheeler Matrix (BWM) of a <see cref="TextWithTerminator"/> is the square matrix all cyclic rotations of
/// the provided <see cref="TextWithTerminator"/>, with rows sorted in ascending order and taking into account that 
/// <see cref="TextWithTerminator.Terminator"/> is to be considered smaller than any other char in the text.
/// </summary>
/// <param name="Text">The text, corresponding to the provided BWM.</param>
/// <param name="Content">The content of the Burrows-Wheeler Matrix (BWM) of <see cref="Text"/>.</param>
/// <remarks>
/// This <see langword="record"/> is a typed wrapped of the underlying <see langword="IList{string}"/> representing 
/// the BWM. It guarantes immutability and strong typing, and also keeps together the <see cref="Text"/> and its 
/// matrix <see cref="Content"/>, providing BWM-specific functionalities.
/// </remarks>
public record BWMatrix(TextWithTerminator Text, IList<string> Content)
{
    // CharOrTerminatorComparer is a record, so compared by value
    private readonly IComparer<char> _charComparer = CharOrTerminatorComparer.Build(Text.Terminator);

    /// <summary>
    /// <inheritdoc cref="BWMatrix(TextWithTerminator, IList{string})" path="/param[@name='Content']"/>
    /// </summary>
    /// <returns>
    /// A readonly immutable list of strings, each one containing a row of the matrix, i.e. a string containing a 
    /// cyclic rotation of <see cref="Text"/>.
    /// </returns>
    /// <example>
    /// Code: 
    /// <code>
    /// new BWTMatrix(new("ab"), new string[] { "$ab", "ab$", "b$a" }).Content
    /// </code>
    /// 
    /// Result:
    /// <code>
    /// {
    ///     "$ab",
    ///     "ab$",
    ///     "b$a",
    /// }
    /// </code>
    /// </example>
    public IList<string> Content { get; init; } = Content.ToValueReadOnlyCollection();

    /// <summary>
    /// Builds the Burrows-Wheeler Transform from this <see cref="BWMatrix"/>, which corresponds to the last column
    /// of the matrix, stored in <see cref="Content"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="BWTransform"/> object wrapping the string containing the Burrows-Wheeler transform.
    /// </returns>
    /// <example>
    ///     Code: 
    ///     <code>
    ///     new BWTMatrix(new("mississippi")).Transform;
    ///     </code>
    ///     
    ///     Result:
    ///     <code>
    ///     "ipssm$pissii"
    ///     </code>
    /// </example>
    /// <remarks>
    /// Requires <see cref="Content"/> calculation.
    /// <inheritdoc cref="BWMatrix" path="/remarks/para[@id='lazy-initialization']"/>
    /// </remarks>
    public BWTransform Transform =>
        new(Text, new(Content.Select(r => r[^1]), Text.Terminator));

    /// <summary>
    /// Returns the first column of this <see cref="BWMatrix"/>. Corresponds to the sorted <see cref="Text"/> and 
    /// also to the sorted <see cref="Transform"/> of this <see cref="BWMatrix"/>.
    /// </summary>
    /// <example>
    ///     Code: 
    ///     <code>
    ///     new BWTMatrix(new("mississippi")).FirstColumn
    ///     </code>
    ///     
    ///     Result:
    ///     <code>
    ///     "$iiiimppssss"
    ///     </code>
    /// </example>
    /// <remarks>
    /// Unlike <see cref="LastColumn"/> and <see cref="Transform"/>, <see cref="FirstColumn"/> wouldn't require 
    /// computation of the <see cref="Content"/> of this <see cref="BWMatrix"/>, since the <see cref="FirstColumn"/>
    /// can easily be calculated by sorting the input <see cref="Text"/>.
    /// </remarks>
    public string FirstColumn =>
        string.Concat(Text.OrderBy(c => c, _charComparer));

    /// <summary>
    /// Returns the last column of this <see cref="BWMatrix"/>. Corresponds to the <see cref="BWTransform.Content"/> 
    /// of the <see cref="Transform"/> of this <see cref="BWMatrix"/>.
    /// </summary>
    /// <example>
    ///     Code: 
    ///     <code>
    ///     new BWTMatrix(new("mississippi")).LastColumn
    ///     </code>
    ///     
    ///     Result:
    ///     <code>
    ///     "ipssm$pissii"
    ///     </code>
    /// </example>
    /// <remarks>
    /// Requires <see cref="Content"/> calculation.
    /// <inheritdoc cref="BWMatrix" path="/remarks/para[@id='lazy-initialization']"/>
    /// </remarks>
    public string LastColumn => 
        string.Concat(Transform.Content.RotatedText);
}
