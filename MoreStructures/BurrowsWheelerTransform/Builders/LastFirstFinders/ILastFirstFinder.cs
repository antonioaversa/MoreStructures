namespace MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

/// <summary>
/// A stategy used by a <see cref="IBuilder"/> to find chars in <see cref="BWT"/> and in its sorted version 
/// <see cref="SortedBWT"/>.
/// </summary>
public interface ILastFirstFinder
{
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
