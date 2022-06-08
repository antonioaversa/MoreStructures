using MoreStructures.Utilities;

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
public record BWTransform(TextWithTerminator Text, RotatedTextWithTerminator Content)
{
    /// <summary>
    /// Any strategy to sort the <see cref="char"/> of a <see cref="RotatedTextWithTerminator"/>, for example to turn
    /// a BWT into its sorted version.
    /// </summary>
    /// <param name="text">The text to be sorted.</param>
    /// <param name="comparer">
    /// The <see cref="IComparer{T}"/> of <see cref="char"/> to be used for comparison.
    /// If not specified, a <see cref="CharOrTerminatorComparer"/> using the 
    /// <see cref="RotatedTextWithTerminator.Terminator"/> of <paramref name="text" /> is used instead.
    /// </param>
    /// <returns>
    /// A new <see cref="RotatedTextWithTerminator"/>, sorted according to the provided <paramref name="comparer"/>, 
    /// together with a <see cref="IEnumerable{T}"/> of <see cref="int"/>, defining the mapping of the index of each 
    /// char of the input text into the index of that char in the sorted version of the text.
    /// </returns>
    public delegate (RotatedTextWithTerminator sortedText, IEnumerable<int> indexesMapping) SortStrategy(
        RotatedTextWithTerminator text, IComparer<char>? comparer = null);

    /// <summary>
    /// A strategy to sort a <see cref="RotatedTextWithTerminator"/> using
    /// <see cref="Enumerable.OrderBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey}, IComparer{TKey}?)"/>,
    /// which in turn uses a QuickSort with Time Complexity = O(n * log(n)) in average and O(n^2) in the worst case.
    /// </summary>
    /// <remarks>
    /// Tipically used to sort the Burrows-Wheeler Transform.
    /// </remarks>
    public static readonly SortStrategy QuickSort =
        (text, charComparer) =>
        {
            charComparer ??= CharOrTerminatorComparer.Build(text.Terminator);

            var textWithIndexes = text.Select((c, i) => (c, i));
            var sortedTextWithIndexes = textWithIndexes.OrderBy(charAndIndex => charAndIndex.c, charComparer);
            var sortedText = new RotatedTextWithTerminator(
                sortedTextWithIndexes.Select(charAndIndex => charAndIndex.c), 
                text.Terminator);
            var indexesMapping = sortedTextWithIndexes.Select(charAndIndex => charAndIndex.i);
            return (sortedText, indexesMapping);
        };

    /// <summary>
    /// The length of this transform, which corresponds to the length of <see cref="Content"/>.
    /// </summary>
    public int Length => Content.Length;
}
