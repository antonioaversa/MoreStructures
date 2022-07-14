namespace MoreStructures.Strings.Sorting;

/// <summary>
/// An implementation of <see cref="ICharsSorter"/> which uses Counting Sort to sort the input.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES
///     <br/>
///     The algorithm is a Counting Sort adaptation and specialization to strings, seen as lists of chars.
///     <br/>
///     It leverages a runtime linear in the size of the input and its alphabet, which is better than any worst-case
///     scenario of a comparison-based algorithm, such as QuickSort or MergeSort.
///     <br/>
///     However, because its runtime depends not only on the size of the input, but also on the size of the alphabet,
///     it is suitable for scenarios of small alphabets only, or at least when there is an upper bound on the number of
///     distinct chars in the input (and they are somehow known in advance), which is small enough to fit the histogram
///     of occurrences in memory.
///     <br/>
///     For example: genome sequences can be mapped to 4-chars strings; English lower-case sentences are limited to 26
///     chars plus space, punctuation, etc; digit sequences consist of an alphabet of 10 chars only etc.
///     <br/>
///     It is not a good fit for scenarios where the number of distinct values can be very high, such as an 
///     unconstrained Unicode string.
///     </para>
///     <para id="algo">
///     ALGORITHM
///     <br/>
///     - First, a histogram of the occurrences in the input of all the chars in the alphabet is created, going through
///       each char of the input and incrementing the counter of the char in the histogram.
///       <br/>
///     - Then the histogram is made cumulative, by going through each item of the histogram array, starting from the
///       second item, and cumulating the previous sum into the value at the current position.
///       <br/>
///     - Finally, all chars of the input are iterated once again, in reverse order, to build the order list, i.e. the
///       list of positions.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Building the histogram requires iterating over all the n chars of the input.
///       <br/>
///     - The histogram has a size equal to the size of the alphabet, sigma, of the input.
///       <br/>
///     - Making the histogram cumulative requires going through each item of it, and there are sigma items.
///       <br/>
///     - Finally, the second pass of the string means n more iterations, each one doing constant-time work.
///       <br/>
///     - The second pass builds the order list, which is a new array of n items (one per index of the input).
///       <br/>
///     - Therefore, Time Complexity is O(n + sigma) and Space Complexity is O(n + sigma).
///     </para>
/// </remarks>
public class CountingSortCharsSorter : ICharsSorter
{
    /// <summary>
    /// The alphabet of the input, i.e. the list of all <see cref="char"/> potentially appearing in the input string,
    /// mapped to an alphabet index.
    /// </summary>
    /// <remarks>
    /// Required by the Counting Sort algorithm, which builds the histogram of occurrences in the input, of all chars
    /// of the alphabet of the input.
    /// </remarks>
    public IDictionary<char, int> Alphabet { get; }

    /// <summary>
    ///     <inheritdoc cref="CountingSortCharsSorter"/>
    /// </summary>
    /// <remarks>
    ///     <inheritdoc cref="CountingSortCharsSorter"/>
    /// </remarks>
    /// <param name="alphabet"><inheritdoc cref="Alphabet" path="/summary"/></param>
    public CountingSortCharsSorter(IDictionary<char, int> alphabet)
    {
        Alphabet = alphabet;
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="CountingSortCharsSorter" path="/remarks"/>
    /// </remarks>
    public IList<int> Sort(string input)
    {
        // Build histogram
        var counts = new int[Alphabet.Count];
        for (int i = 0; i < input.Length; i++)
            counts[Alphabet[input[i]]]++;

        // Make histogram cumulative
        for (int i = 1; i < counts.Length; i++)
            counts[i] = counts[i] + counts[i - 1];

        // Calculate order list
        var order = new int[input.Length];
        for (int i = input.Length - 1; i >= 0; i--)
        {
            var alphabetIndex = Alphabet[input[i]];
            var position = counts[alphabetIndex] = counts[alphabetIndex] - 1;
            order[position] = i;
        }

        return order;
    }
}
