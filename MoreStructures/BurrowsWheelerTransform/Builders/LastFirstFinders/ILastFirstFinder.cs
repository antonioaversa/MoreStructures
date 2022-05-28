namespace MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;

/// <summary>
/// A stategy used by a builder to find chars in the BWT and its sorted version.
/// </summary>
public interface ILastFirstFinder
{
    /// <summary>
    /// The Barrows-Wheeler Transform.
    /// </summary>
    public IList<char> BWT { get; }

    /// <summary>
    /// The sorted version of the Barrows-Wheeler Transform.
    /// </summary>
    public IList<char> SortedBWT { get; }

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
