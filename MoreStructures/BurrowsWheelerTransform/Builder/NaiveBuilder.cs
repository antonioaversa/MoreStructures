using MoreLinq;
using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform.Builder;

/// <summary>
/// <inheritdoc cref="IBuilder"/>
/// This implementation adopts the simplest approach at <see cref="BWMatrix"/> building, which results in a more than
/// quadratic time and space. <see cref="BWTransform"/> is calculated via the <see cref="BWMatrix"/>, therefore same
/// level of Time and Space Complexity.
/// </summary>
/// <remarks>
/// Check specific builder methods, such as <see cref="BuildMatrix(MoreStructures.TextWithTerminator)"/>, for further
/// information about the complexity of each operation.
/// </remarks>
public class NaiveBuilder : IBuilder
{
    /// <inheritdoc cref="IBuilder" path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <para id="complexity">
    ///     Since this operation requires computing a n * n matrix, where n is the 
    ///     <see cref="TextWithTerminator.Length"/> of <paramref name="text"/>, it can be intensive operation, both in 
    ///     time: 
    ///     <para>
    ///     Sorting a large number of strings on a large non-constant alphabet takes n * log(n) * m, where m is
    ///     the cost of a comparison of two n-sized strings, which is O(n). 
    ///     Therefore Time Complexity is O(n^2 * log(n)). 
    ///     If the alphabet can be considered of constant size and comparison between two strings happens in
    ///     constant time, the complexity is O(n * log(n)).
    ///     </para>
    ///     <para>
    ///     The output is a n * n matrix of chars (all cyclic rotations of a n-sized string). 
    ///     Therefore Space Complexity is O(n^2 * m), when no assumption is made on the size of a char being 
    ///     constant, where m = log(w, M), with w = size of a word in memory and M = size of the alphabet. 
    ///     If the alphabet can be considered of constant size, the complexity is O(n^2).
    ///     </para>
    ///     </para>
    /// </remarks>
    public BWMatrix BuildMatrix(TextWithTerminator text)
    {
        var stringsComparer = new StringIncludingTerminatorComparer(text.Terminator);
        var content = Enumerable
            .Range(0, text.Length)
            .Select(i => new string(text.Skip(i).Take(text.Length - i).Concat(text.Take(i)).ToArray()))
            .OrderBy(i => i, stringsComparer)
            .ToValueReadOnlyCollection();

        return new(text, content);
    }

    /// <inheritdoc/>
    public BWTransform BuildTransform(BWMatrix matrix) => matrix.Transform;

    /// <inheritdoc cref="IBuilder" path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// <inheritdoc/>
    /// Done by first constructing the <see cref="BWMatrix"/> of <paramref name="text"/>, and then invoking 
    /// <see cref="BuildTransform(BWMatrix)"/> on it.
    /// </remarks>
    public BWTransform BuildTransform(TextWithTerminator text) => BuildTransform(BuildMatrix(text));

    /// <inheritdoc/>
    public TextWithTerminator InvertMatrix(BWMatrix matrix)
    {
        var firstBWMRow = matrix.Content[0]; // In the form "$..." where $ is separator
        return new TextWithTerminator(firstBWMRow[1..], firstBWMRow[0]);
    }

    /// <inheritdoc cref="IBuilder.InvertTransform(RotatedTextWithTerminator)" path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="IBuilder.InvertTransform(RotatedTextWithTerminator)" 
    ///         path="/remarks/para[@id='terminator-required']"/>
    ///     <para>
    ///     This implementation inverts the BWT by iteratively building n+1-mers from n-mers. 2-mers are just the 
    ///     juxtaposition of the column given by the sorted BWT and the column given by the BWT provided as input into
    ///     a matrix of 2 columns and n rows (where n = length of <paramref name="lastBWMColumn"/>).
    ///     3-mers are derived from matching with sorted 2-mers and so on...
    ///     </para>
    /// </remarks>
    public TextWithTerminator InvertTransform(RotatedTextWithTerminator lastBWMColumn)
    {
        var charsComparer = new StringIncludingTerminatorComparer(lastBWMColumn.Terminator);

        var allBWMColumnsExceptLast = new List<List<char>> { };
        foreach (var columnIndex in Enumerable.Range(0, lastBWMColumn.Length - 1)) // # columns in the BWM - 1
        {
            var nextBWMColumn = (
                from rowIndex in Enumerable.Range(0, lastBWMColumn.Length) // # rows in the BWM
                let lastElementOfRow = Enumerable.Repeat(lastBWMColumn[rowIndex], 1)
                let ithFirstElementsOfRow = (
                    from j in Enumerable.Range(0, columnIndex)
                    select allBWMColumnsExceptLast[j][rowIndex])
                let lastAndIthFirstElementsOfRow = 
                    string.Join(string.Empty, lastElementOfRow.Concat(ithFirstElementsOfRow))
                select lastAndIthFirstElementsOfRow)
                .OrderBy(lastAndIthFirstElementsOfRow => lastAndIthFirstElementsOfRow, charsComparer)
                .Select(lastAndIthFirstElementsOfRow => lastAndIthFirstElementsOfRow[^1]);
            allBWMColumnsExceptLast.Add(nextBWMColumn.ToList());
        }

        var text = string.Join(
            string.Empty, 
            Enumerable
                .Range(1, lastBWMColumn.Length - 2) // # columns in the BWM - 1
                .Select(i => allBWMColumnsExceptLast[i][0])
                .Concat(new char[] { lastBWMColumn[0] }));
        return new TextWithTerminator(text, lastBWMColumn.Terminator);
    }
}
