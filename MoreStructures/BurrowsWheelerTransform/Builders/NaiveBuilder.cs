using MoreLinq;
using MoreStructures.Utilities;

namespace MoreStructures.BurrowsWheelerTransform.Builders;

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
    public virtual BWMatrix BuildMatrix(TextWithTerminator text)
    {
        var stringsComparer = StringIncludingTerminatorComparer.Build(text.Terminator);
        var content = Enumerable
            .Range(0, text.Length)
            .Select(i => new string(text.Skip(i).Take(text.Length - i).Concat(text.Take(i)).ToArray()))
            .OrderBy(i => i, stringsComparer)
            .ToValueReadOnlyCollection();

        return new(text, content);
    }
    
    /// <inheritdoc/>
    public BWMatrix BuildMatrix(BWTransform lastBWMColumn)
    {
        var allBWMColumnsExceptLast = BuildAllBWMColumnsExceptLast(lastBWMColumn.Content);
        var allBWMRows = (
            from rowIndex in Enumerable.Range(0, lastBWMColumn.Length)
            let rowExceptLastChar = string.Concat(
                from columnIndex in Enumerable.Range(0, allBWMColumnsExceptLast.Count)
                select allBWMColumnsExceptLast[columnIndex][rowIndex])
            select rowExceptLastChar + lastBWMColumn.Content[rowIndex])
            .ToList(); // The Burrows-Wheeler Matrix requires direct random access to any of its elements
        return new(lastBWMColumn.Text, allBWMRows);
    }

    /// <inheritdoc/>
    public virtual BWTransform BuildTransform(BWMatrix matrix) => matrix.Transform;

    /// <inheritdoc cref="IBuilder" path="//*[not(self::remarks)]"/>
    /// <remarks>
    /// <inheritdoc/>
    ///     <para>
    ///     Done without constructing the <see cref="BWMatrix"/> of <paramref name="text"/>, which would requires 
    ///     O(n^2) space.
    ///     </para>
    ///     <para>
    ///     Instead, n <see cref="VirtuallyRotatedTextWithTerminator"/> objects are created (one per char of 
    ///     <paramref name="text"/>), mapping a specific rotation of the original <paramref name="text"/> and taking 
    ///     into account the rotation in its all its char-position dependent functionalities, such as
    ///     <see cref="VirtuallyRotatedTextWithTerminator.CompareTo(VirtuallyRotatedTextWithTerminator?)"/>, 
    ///     <see cref="VirtuallyRotatedTextWithTerminator.GetEnumerator"/> etc.
    ///     </para>
    /// </remarks>
    public virtual BWTransform BuildTransform(TextWithTerminator text)
    {
        var content = Enumerable
            .Range(0, text.Length)
            .Select(i => text.ToVirtuallyRotated(i))
            .OrderBy(i => i)
            .Select(vrtext => vrtext[^1]);
        return new(text, new(content, text.Terminator));
    }

    /// <inheritdoc/>
    /// <remarks>
    /// No computation to be done, except for building the string of the <see cref="TextWithTerminator"/>.
    /// Time Complexity = O(n), Space Complexity = O(n), where n = edge of <paramref name="matrix"/>.
    /// </remarks>
    public virtual TextWithTerminator InvertMatrix(BWMatrix matrix)
    {
        var firstBWMRow = matrix.Content[0]; // In the form "$..." where $ is separator
        return new TextWithTerminator(firstBWMRow[1..].AsValueEnumerable(), firstBWMRow[0]);
    }

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="IBuilder.InvertTransform(RotatedTextWithTerminator)" 
    ///         path="/remarks/para[@id='terminator-required']"/>
    ///     <para id="algo">
    ///         This implementation inverts the BWT by iteratively building n+1-mers from n-mers.
    ///         <para>
    ///         - 1-mers (first column of the matrix) is just the last column (BWT), sorted.
    ///           That gives a matrix M0 of 1 columns and n rows (where n = length of 
    ///           <paramref name="lastBWMColumn"/>).
    ///         - 2-mers are derived from 1-mers, by juxtaposing side-by-side last column (BWT) and M0, sorted.
    ///           That gives a matrix M1 of 2 columns and n rows.
    ///         </para>
    ///         <para>
    ///         - 3-mers are derived from 2-mers, by juxtaposing side-by-side last column (BWT) and M1, sorted.
    ///           That gives a matrix M2 of 3 columns and n rows.
    ///         </para>
    ///         <para>
    ///         - And so on, up to (n - 1)-mers and matrix M(n - 2) of n - 1 columns and n rows.
    ///         </para>
    ///         <para>
    ///         - The last column is already known (BWT), so the text can be extracted from the first line: the first
    ///           char is the separator, the rest is the text without separator.
    ///         </para>
    ///     </para>
    ///     <para id="complexity">
    ///         <para>
    ///         There are n top-level iterations, where n is the length of <paramref name="lastBWMColumn"/>.
    ///         </para>
    ///         <para>
    ///         Each iteration takes n * log(n) * m time to sort, where m is the length of strings to compare = n.
    ///         </para>
    ///         <para>
    ///         So total Time Complexity is O(n^3 * log(n)) and Space Complexity is O(n^2).
    ///         </para>
    ///     </para>
    /// </remarks>
    public virtual TextWithTerminator InvertTransform(RotatedTextWithTerminator lastBWMColumn)
    {
        var allBWMColumnsExceptLast = BuildAllBWMColumnsExceptLast(lastBWMColumn);

        // The text is in the first row of the matrix
        var text = 
            Enumerable
                .Range(1, lastBWMColumn.Length - 2) // # columns in the BWM - 1
                .Select(i => allBWMColumnsExceptLast[i][0])
                .Concat(new char[] { lastBWMColumn[0] });
        return new TextWithTerminator(text, lastBWMColumn.Terminator);
    }

    private static IList<IList<char>> BuildAllBWMColumnsExceptLast(RotatedTextWithTerminator lastBWMColumn)
    {
        var stringsComparer = StringIncludingTerminatorComparer.Build(lastBWMColumn.Terminator);

        var allBWMColumnsExceptLast = new List<IList<char>> { };
        foreach (var columnIndex in Enumerable.Range(0, lastBWMColumn.Length - 1)) // # columns in the BWM - 1
        {
            var nextBWMColumn = (
                from rowIndex in Enumerable.Range(0, lastBWMColumn.Length) // # rows in the BWM
                let lastElementOfRow = Enumerable.Repeat(lastBWMColumn[rowIndex], 1)
                let ithFirstElementsOfRow = (
                    from j in Enumerable.Range(0, columnIndex)
                    select allBWMColumnsExceptLast[j][rowIndex])
                let lastAndIthFirstElementsOfRow =
                    string.Concat(lastElementOfRow.Concat(ithFirstElementsOfRow))
                select lastAndIthFirstElementsOfRow)
                .OrderBy(lastAndIthFirstElementsOfRow => lastAndIthFirstElementsOfRow, stringsComparer)
                .Select(lastAndIthFirstElementsOfRow => lastAndIthFirstElementsOfRow[^1]);
            allBWMColumnsExceptLast.Add(nextBWMColumn.ToList());
        }

        return allBWMColumnsExceptLast;
    }
}
