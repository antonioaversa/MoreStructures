using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform.Matching.Comparers;

/// <summary>
/// An <see cref="IComparer{T}"/> of <see cref="int"/>, which compares a suffix <see cref="string"/> of 
/// <see cref="Text"/> against the provided <see cref="string"/> pattern, and ignores the second <see cref="int"/> 
/// value.
/// </summary>
public abstract class SuffixAgainstPatternComparer : IComparer<int>
{
    /// <summary>
    /// The text, to extract suffixes from.
    /// </summary>
    public TextWithTerminator Text { get; }

    /// <summary>
    /// The pattern, to compare against each suffix of <see cref="Text"/>.
    /// </summary>
    public IEnumerable<char> Pattern { get; }

    /// <summary>
    /// The <see cref="IComparer{T}"/> of <see cref="char"/>, to be used to compare single chars of the strings to 
    /// compare (suffixes and pattern).
    /// </summary>
    public IComparer<char> CharComparer { get; }

    /// <summary>
    /// The value of the first term of comparison, which resulted in <see cref="LongestMatch"/> chars matched,
    /// when comparing the suffix starting at <see cref="LongestMatchFirstValue"/> against the pattern.
    /// </summary>
    /// <remarks>
    /// If multiple values of the first term resulted in the same amount of chars matched, the first value
    /// encountered is kept.
    /// </remarks>
    public int LongestMatchFirstValue { get; protected set; } = -1;

    /// <summary>
    /// The maximum amount of chars of the pattern matched, since the instantiation of this comparer.
    /// </summary>
    /// <remarks>
    /// It is never reset. To start over, a new instance of this comparer has to be created.
    /// </remarks>
    public int LongestMatch { get; protected set; } = 0;

    /// <summary>
    ///     <inheritdoc cref="SuffixAgainstPatternComparer"/>
    /// </summary>
    /// <param name="text"><inheritdoc cref="Text"/></param>
    /// <param name="pattern"><inheritdoc cref="Pattern"/></param>
    public SuffixAgainstPatternComparer(TextWithTerminator text, IEnumerable<char> pattern)
    {
        Text = text;
        Pattern = pattern;
        CharComparer = CharOrTerminatorComparer.Build(text.Terminator);
    }

    /// <summary>
    /// Compares the suffix of text identified by <paramref name="x"/> against the pattern.
    /// </summary>
    /// <param name="x">
    /// The index, in the complete Suffix Array, of the suffix which is first term of comparison.
    /// </param>
    /// <param name="y">Ignored.</param>
    /// <returns>
    /// A positive value if there is mismatch and the suffix is bigger than the pattern lexicographically.
    /// <br/>
    /// A negative value if there is mismatch and the suffix is smaller than the pattern lexicographically.
    /// <br/>
    /// The value 0 if there is full match and pattern and text are of the same length or pattern is shorter.
    /// <br/>
    /// The value -1 if there is full match but the pattern is longer than the suffix.
    /// </returns>
    public abstract int Compare(int x, int y);

    /// <summary>
    /// Compares the suffix of <see cref="Text"/> starting at index <paramref name="suffixStartIndex"/> against
    /// <see cref="Pattern"/>, returning the result of the comparison as a positive, null or negative 
    /// <see cref="int"/>.
    /// </summary>
    /// <param name="bwtIndex">
    /// The index, in the Burrows-Wheeler Transform, of the first char of the suffix starting at 
    /// <paramref name="suffixStartIndex"/>.
    /// </param>
    /// <param name="suffixStartIndex">
    /// The index of <see cref="Text"/> where <paramref name="suffixStartIndex"/> starts.
    /// </param>
    /// <returns>
    /// A positive <see cref="int"/>, if the suffix is lexicographically bigger than <see cref="Pattern"/>.
    /// <br/>
    /// A negative <see cref="int"/>, if the suffix is lexicographically smaller than <see cref="Pattern"/>.
    /// <br/>
    /// Zero, when there is a match, and the suffix is either of longer or of the same length as <see cref="Pattern"/>.
    /// </returns>
    protected virtual int CompareSuffixAgainstPattern(int bwtIndex, int suffixStartIndex)
    {
        using var firstEnumerator = Text[suffixStartIndex..].GetEnumerator();
        using var secondEnumerator = Pattern.GetEnumerator();

        int numberOfItemsMatched = 0;
        bool firstHasValue, secondHasValue;
        while (true)
        {
            firstHasValue = firstEnumerator.MoveNext();
            secondHasValue = secondEnumerator.MoveNext();

            if (!firstHasValue || !secondHasValue)
                break;

            var itemsComparison = CharComparer.Compare(firstEnumerator.Current, secondEnumerator.Current);
            if (itemsComparison != 0)
                return itemsComparison;

            if (++numberOfItemsMatched > LongestMatch)
            {
                LongestMatch = numberOfItemsMatched;
                LongestMatchFirstValue = bwtIndex;
            }
        }

        if (secondHasValue)
            return -1; // The pattern is longer than the suffix

        // Either the pattern is shorter than the suffix, or pattern and suffix have equal length.
        // In either case, the match is successful.
        return 0;
    }
}
