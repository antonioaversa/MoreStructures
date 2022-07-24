using MoreStructures.Utilities;

namespace MoreStructures.SuffixArrays.CyclicShifts;

/// <summary>
/// A <see cref="ISingleCharPcsClassifier"/> implementation which calculate equivalence classes using the definition.
/// </summary>
/// <remarks>
///     <inheritdoc cref="ISingleCharPcsClassifier"/>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - Compared to more advanced implementations, such as <see cref="OrderBasedSingleCharPcsClassifier"/>, it only
///       requires the <see cref="Input"/>, to calculate equivalence classes.
///       <br/>
///     - However, its runtime is worse, both in Time and Space Complexity, than a solution based on position list, 
///       where the effort of sorting the chars has been already done previously.
///     </para>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - Go though each char of the <see cref="Input"/>.
///       <br/>
///     - For each char, count the distinct chars of <see cref="Input"/> which are strictly smaller.
///       <br/>
///     - Build an output list of the result.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - For each of the n chars of the <see cref="Input"/>, a pass of all n chars of <see cref="Input"/> is done, to
///       count to keep the ones which are strictly smaller.
///       <br/>
///     - Count the distinct occurrences is a O(n) operation in time and space, done via the LINQ method 
///       <see cref="Enumerable.Distinct{TSource}(IEnumerable{TSource})"/>, which uses an internal lightweight
///       implementation of a set.
///       <br/>
///     - The output is a list of n items.
///       <br/>
///     - Therefore, Time Complexity is O(n^2) and Space Complexity is O(n).
///     </para>
/// </remarks>
public class NaiveSingleCharPcsClassifier : ISingleCharPcsClassifier
{
    private IComparer<char> CharsComparer { get; }

    /// <inheritdoc/>
    public string Input { get; }

    /// <summary>
    ///     <inheritdoc cref="OrderBasedSingleCharPcsClassifier"/>
    /// </summary>
    /// <param name="input"><inheritdoc cref="Input" path="/summary"/></param>
    /// <param name="inputWithTerminator">
    /// Whether <paramref name="input"/> is terminated by a terminator char. If so, the last char of 
    /// <paramref name="input"/> will be treated as a terminator char when comparing chars of the input.
    /// Otherwise, <see cref="Comparer{T}.Default"/> for <see cref="char"/> will be used.
    /// </param>
    public NaiveSingleCharPcsClassifier(string input, bool inputWithTerminator)
    {
        Input = input;
        CharsComparer = inputWithTerminator
            ? CharOrTerminatorComparer.Build(input[^1])
            : Comparer<char>.Default;
    }

    /// <inheritdoc/>
    public IList<int> Classify() => 
        Input.Select(c1 => Input.Where(c2 => CharsComparer.Compare(c1, c2) > 0).Distinct().Count()).ToList();
}