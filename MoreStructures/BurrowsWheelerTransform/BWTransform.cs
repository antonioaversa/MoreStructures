namespace MoreStructures.BurrowsWheelerTransform;

/// <summary>
/// The Burrows-Wheeler Transform (BWT) of a <see cref="TextWithTerminator"/> <paramref name="Text"/> is a permutation
/// of the chars of <paramref name="Text"/> which corresponds to the <see cref="BWMatrix.LastColumn"/> of the 
/// <see cref="BWMatrix"/> of <paramref name="Text"/>.
/// </summary>
/// <param name="Text">The text to calculate the BWT of.</param>
/// <param name="Content">The string which corresponds to the transform of the text.</param>
/// <remarks>
/// This <see langword="record"/> is a typed wrapped of the underlying <see langword="string"/> representing the BWT.
/// It guarantes immutability and strong typing, and also keeps together the <see cref="Text"/> and its transform
/// <see cref="Content"/>.
/// </remarks>
public record BWTransform(TextWithTerminator Text, string Content)
{
    /// <summary>
    /// The length of this transform, which corresponds to the length of <see cref="Content"/>.
    /// </summary>
    public int Length => Content.Length;

    /// <summary>
    /// Returns the <see cref="ToString"/> of the underlying <see cref="Content"/> of this transform.
    /// </summary>
    /// <returns><inheritdoc/></returns>
    public override string ToString() => Content;
}
