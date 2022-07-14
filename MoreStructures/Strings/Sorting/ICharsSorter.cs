namespace MoreStructures.Strings.Sorting;

/// <summary>
/// An algorithm sorting the chars of the provided string.
/// </summary>
public interface ICharsSorter
{
    /// <summary>
    /// Sorts the chars of <paramref name="input"/>, returning the list of positions of each of the chars in 
    /// <paramref name="input"/> (i.e. the <see cref="int"/> value item of the order list O at index i, O[i], is the 
    /// position in the <paramref name="input"/> string, of the <see cref="char"/> in the sorted string at index i).
    /// </summary>
    /// <param name="input">The string whose <see cref="char"/> items have to be sorted.</param>
    /// <returns>A list of position in the sorted string: 0 being the first in the order, 1 the second, etc.</returns>
    /// <example>
    /// Sorting the input string <c>"cabcba"</c> gives the position list <c>{ 1, 5, 2, 4, 0, 5 }</c>, because:
    /// <br/>
    /// - the sorted string is <c>"aabbcc"</c>;
    ///   <br/>
    /// - the <see cref="char"/> in the sorted string at index 0 is <c>'a'</c> and the position in the 
    ///   <paramref name="input"/> string of the first <c>'a'</c> is <c>1</c>;
    ///   <br/>
    /// - the <see cref="char"/> in the sorted string at index 1 is <c>'a'</c> and the position in the 
    ///   <paramref name="input"/> string of the second <c>'a'</c> is <c>5</c>;
    ///   <br/>
    /// - the <see cref="char"/> in the sorted string at index 2 is <c>'b'</c> and the position in the 
    ///   <paramref name="input"/> string of the first <c>'b'</c> is <c>2</c>;
    ///   <br/>
    /// - etc.
    /// </example>
    IList<int> Sort(string input);
}
