namespace MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

/// <summary>
/// A stategy used by a <see cref="IBuilder"/> to find chars in <see cref="BWT"/> and in its sorted version 
/// <see cref="SortedBWT"/>.
/// </summary>
public interface ILastFirstFinder
{
    /// <summary>
    /// Any strategy to sort the <see cref="char"/> of a <see cref="RotatedTextWithTerminator"/>, for example to turn
    /// a BWT into its sorted version.
    /// </summary>
    /// <param name="text">The text to be sorted.</param>
    /// <param name="comparer">The <see cref="IComparer{T}"/> of <see cref="char"/> to be used for comparison.</param>
    /// <returns>
    /// A new <see cref="RotatedTextWithTerminator"/>, sorted according to the provided <paramref name="comparer"/>.
    /// </returns>
    public delegate RotatedTextWithTerminator SortStrategy(RotatedTextWithTerminator text, IComparer<char> comparer);

    /// <summary>
    /// A strategy to sort a <see cref="RotatedTextWithTerminator"/> using
    /// <see cref="Enumerable.OrderBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey}, IComparer{TKey}?)"/>,
    /// which in turn uses a QuickSort with Time Complexity = O(n * log(n)) in average and O(n^2) in the worst case.
    /// </summary>
    /// <remarks>
    /// Tipically used to sort the <see cref="BWT"/>.
    /// </remarks>
    public static readonly SortStrategy QuickSort = 
        (text, charComparer) => new(text.OrderBy(c => c, charComparer), text.Terminator);

    /// <summary>
    /// The <see cref="IComparer{T}"/> used to compare chars of <see cref="BWT"/> or <see cref="SortedBWT"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="Comparer{T}.Default"/> of <see cref="char"/> cannot be used because the terminator in
    /// <see cref="BWT"/> and <see cref="SortedBWT"/> has to be treated in a special way 
    /// (<see cref="TextWithTerminator.Terminator"/> is always to be considered smaller than any other char).
    /// </remarks>
    IComparer<char> CharComparer { get; }

    /// <summary>
    /// The Burrows-Wheeler Transform. Also, the last column of the Burrows-Wheeler Matrix.
    /// </summary>
    public RotatedTextWithTerminator BWT { get; }

    /// <summary>
    /// The sorted version of the Burrows-Wheeler Transform. Also, the first column of the Burrows-Wheeler Matrix.
    /// </summary>
    public RotatedTextWithTerminator SortedBWT { get; }

    /// <summary>
    /// Find the index of the n-th occurrence (0-based) in <see cref="BWT"/>, of the char in <see cref="BWT"/> at the 
    /// provided index <paramref name="indexOfCharInBWT"/>.
    /// </summary>
    /// <param name="indexOfCharInBWT">
    /// The index of the char in <see cref="BWT"/>, to find the n-th occurrence of, again in <see cref="BWT"/>.
    /// </param>
    /// <param name="occurrenceRank">The 0-based occurrence rank to find. 0 = 1st occurrence.</param>
    /// <returns>
    /// The index of the n-th occurrence of the char in <see cref="BWT"/> at <paramref name="indexOfCharInBWT"/>.
    /// </returns>
    int FindIndexOfNthOccurrenceInBWT(int indexOfCharInBWT, int occurrenceRank);

    /// <summary>
    /// Find the index of the n-th occurrence (0-based) in <see cref="SortedBWT"/>, of the char in 
    /// <see cref="BWT"/> at the provided index <paramref name="indexOfCharInBWT"/>.
    /// </summary>
    /// <param name="indexOfCharInBWT">
    /// The index of the char in <see cref="BWT"/>, to find the n-th occurrence of, in <see cref="SortedBWT"/>.
    /// </param>
    /// <param name="occurrenceRank">The 0-based occurrence rank to find. 0 = 1st occurrence.</param>
    /// <returns>
    /// The index in <see cref="SortedBWT"/> of the n-th occurrence of the char in <see cref="BWT"/> at index 
    /// <paramref name="indexOfCharInBWT"/>.
    /// </returns>
    int FindIndexOfNthOccurrenceInSortedBWT(int indexOfCharInBWT, int occurrenceRank);

    /// <summary>
    /// Find the occurrence rank in <see cref="BWT"/> of the char in <see cref="BWT"/> at the provided index 
    /// <paramref name="indexOfCharInBWT"/>.
    /// </summary>
    /// <param name="indexOfCharInBWT">The index of the char in <see cref="BWT"/>.</param>
    /// <returns>The 0-based occurrence rank of the char at index <paramref name="indexOfCharInBWT"/>.</returns>
    int FindOccurrenceRankOfCharInBWT(int indexOfCharInBWT);

    /// <summary>
    /// Find the occurrence rank in <see cref="SortedBWT"/> of the char in <see cref="SortedBWT"/> at the 
    /// provided index <paramref name="indexOfCharInSortedBWT"/>.
    /// </summary>
    /// <param name="indexOfCharInSortedBWT">The index of the char in <see cref="SortedBWT"/>.</param>
    /// <returns>The 0-based occurrence rank of the char at index <paramref name="indexOfCharInSortedBWT"/>.</returns>
    int FindOccurrenceRankOfCharInSortedBWT(int indexOfCharInSortedBWT);

    /// <summary>
    /// Given the index of a char in <see cref="BWT"/>, it finds the index of the corresponding char in 
    /// the <see cref="SortedBWT"/> and its occurrence rank.
    /// </summary>
    /// <param name="indexOfCharInBWT">The index (0-based) of the char in <see cref="BWT"/>.</param>
    /// <returns>The index of the char in the <see cref="SortedBWT"/> and its occurence rank (0-based).</returns>
    (int indexInSortedBWT, int occurrenceRank) LastToFirst(int indexOfCharInBWT);
}
