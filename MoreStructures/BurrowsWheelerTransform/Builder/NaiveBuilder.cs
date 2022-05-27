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
        var comparer = new StringIncludingTerminatorComparer(text.Terminator);
        var content = Enumerable
            .Range(0, text.Length)
            .Select(i => new string(text.Skip(i).Take(text.Length - i).Concat(text.Take(i)).ToArray()))
            .OrderBy(i => i, comparer)
            .ToValueReadOnlyCollection();

        return new(text, content);
    }

    /// <inheritdoc/>
    public BWTransform BuildTransform(BWMatrix matrix) => matrix.Transform;

    /// <inheritdoc/>
    public BWTransform BuildTransform(TextWithTerminator text) => BuildTransform(BuildMatrix(text));
}
