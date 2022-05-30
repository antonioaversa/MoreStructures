namespace MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

/// <summary>
/// A stategy used by a <see cref="IBuilder"/> to find chars in <see cref="BWT"/> and in its sorted version 
/// <see cref="SortedBWT"/>.
/// </summary>
public interface ILastFirstFinder
{
    /// <summary>
    /// A strategy to sort a <see cref="RotatedTextWithTerminator"/> (tipically containing the <see cref="BWT"/>) using
    /// <see cref="Enumerable.OrderBy{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey}, IComparer{TKey}?)"/>,
    /// which uses a QuickSort with Time Complexity = O(n * log(n)) in average and O(n^2) in the worst case.
    /// </summary>
    public static readonly Func<RotatedTextWithTerminator, IComparer<char>, RotatedTextWithTerminator> QuickSort =
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
    /// The Burrows-Wheeler Transform.
    /// </summary>
    public RotatedTextWithTerminator BWT { get; }

    /// <summary>
    /// The sorted version of the Burrows-Wheeler Transform.
    /// </summary>
    public RotatedTextWithTerminator SortedBWT { get; }

    /// <summary>
    /// Find the index of the n-th occurrence (0-based) of the provided <paramref name="charToFind"/> in 
    /// <see cref="BWT"/>.
    /// </summary>
    /// <param name="charToFind">The char to find in <see cref="BWT"/>.</param>
    /// <param name="occurrenceRank">The 0-based occurrence rank to find. 0 = 1st occurrence.</param>
    /// <returns>
    /// The index of the n-th occurrence of <paramref name="charToFind"/>, if it exists. 
    /// If it doesn't exist, returns -1.
    /// </returns>
    int FindIndexOfNthOccurrenceInBWT(char charToFind, int occurrenceRank);

    /// <summary>
    /// Find the index of the n-th occurrence (0-based) of the provided <paramref name="charToFind"/> in 
    /// <see cref="SortedBWT"/>.
    /// </summary>
    /// <param name="charToFind">The char to find in <see cref="SortedBWT"/>.</param>
    /// <param name="occurrenceRank">The 0-based occurrence rank to find. 0 = 1st occurrence.</param>
    /// <returns>
    /// The index of the n-th occurrence of <paramref name="charToFind"/>.
    /// If it doesn't exist, returns -1.
    /// </returns>
    int FindIndexOfNthOccurrenceInSortedBWT(char charToFind, int occurrenceRank);

    /// <summary>
    /// Find the rank of occurrence rank (0-based) of the char at the provided index in <see cref="BWT"/>.
    /// </summary>
    /// <param name="indexOfChar">The index of the char, whose occurrence has to be found.</param>
    /// <returns>The 0-based occurrence rank of the char at index <paramref name="indexOfChar"/>.</returns>
    int FindOccurrenceRankOfCharInBWT(int indexOfChar);

    /// <summary>
    /// Find the rank of occurrence rank (0-based) of the char at the provided index in <see cref="SortedBWT"/>.
    /// </summary>
    /// <param name="indexOfChar">The index of the char, whose occurrence has to be found.</param>
    /// <returns>The 0-based occurrence rank of the char at index <paramref name="indexOfChar"/>.</returns>
    int FindOccurrenceRankOfCharInSortedBWT(int indexOfChar);

    /// <summary>
    /// Given the index (0-based) of a char in the <see cref="BWT"/>, it finds the index of the corresponding char in 
    /// the <see cref="SortedBWT"/> and its occurrence rank (0-based).
    /// </summary>
    /// <param name="indexOfChar">The index of the char, to locate in the <see cref="SortedBWT"/>.</param>
    /// <returns>The index of the char in the <see cref="SortedBWT"/> and its occurence rank (0-based).</returns>
    (int indexInSortedBWT, int occurrenceRank) LastToFirst(int indexOfChar);
}
