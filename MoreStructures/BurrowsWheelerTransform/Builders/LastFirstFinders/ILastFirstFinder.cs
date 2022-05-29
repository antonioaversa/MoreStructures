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
    /// <param name="charToFind">The char to find in the BWT.</param>
    /// <param name="occurrence">The 0-based occurrence to find. 0 = 1st occurrence.</param>
    /// <returns>The index of the n-th occurrence of <paramref name="charToFind"/> in the BWT.</returns>
    int FindIndexOfNthOccurrenceInBWT(char charToFind, int occurrence);

    /// <summary>
    /// Find the number of occurrence (0-based) of the char at the provided index in <see cref="SortedBWT"/>.
    /// </summary>
    /// <param name="indexOfChar">The index of the char, whose occurrence has to be found.</param>
    /// <returns>The 0-based number of occurrence of the char at index <paramref name="indexOfChar"/>.</returns>
    int FindOccurrenceOfCharInSortedBWT(int indexOfChar);
}
