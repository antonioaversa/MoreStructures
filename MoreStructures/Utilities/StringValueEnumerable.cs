namespace MoreStructures.Utilities;

/// <summary>
/// A <see cref="IValueEnumerable{T}"/> implementation, wrapping a <see cref="string"/>.
/// </summary>
/// <remarks>
/// Useful when a <see cref="string"/> property/field should be equatable to an <see cref="IEnumerable{T}"/> by value.
/// <br/>
/// For example, when a property or field is declared as <see cref="IEnumerable{T}"/> of <see cref="char"/>, and can be
/// assigned either a <see cref="string"/> or a generic <see cref="IEnumerable{T}"/>, and it has to be solely compared 
/// by value (i.e. on the actual chars in the <see cref="string"/> and <see cref="IEnumerable{T}"/>).
/// </remarks>
public class StringValueEnumerable : ValueEnumerable<char>
{
    /// <summary>
    /// Builds a <see cref="ValueEnumerable{T}"/> around the provided <paramref name="stringValue"/>.
    /// </summary>
    /// <param name="stringValue">The string to wrap.</param>
    /// <remarks>
    /// Time and Space Complexity are O(1), as this constructor doesn't iterate over <paramref name="stringValue"/>.
    /// </remarks>
    public StringValueEnumerable(string stringValue) : base(stringValue)
    {
        StringValue = stringValue;
    }

    /// <summary>
    /// The <see cref="string"/> value underlying this <see cref="IValueEnumerable{T}"/>.
    /// </summary>
    public string StringValue { get; }
}