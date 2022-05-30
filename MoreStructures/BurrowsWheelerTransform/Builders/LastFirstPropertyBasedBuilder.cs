using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;
using MoreStructures.Utilities;
using System.Text;

namespace MoreStructures.BurrowsWheelerTransform.Builders;

/// <summary>
/// An extension of <see cref="NaiveBuilder"/> which takes advantange of the last-first property to reduce the
/// complexity of <see cref="IBuilder.InvertTransform(MoreStructures.RotatedTextWithTerminator)"/>.
/// </summary>
/// <remarks>
/// A <see cref="ILastFirstFinder"/>, built by <see cref="FirstLastFinderBuilder"/> is used to jump between the BWT 
/// and its sorted version.
/// </remarks>
public class LastFirstPropertyBasedBuilder : NaiveBuilder
{
    /// <summary>
    /// The strategy by which this builder finds chars in the BWT and its sorted version.
    /// </summary>
    public Func<RotatedTextWithTerminator, ILastFirstFinder> FirstLastFinderBuilder { get; init; } =
        (lastBWMColumn) => new PrecomputedFinder(lastBWMColumn, ILastFirstFinder.QuickSort);

    /// <inheritdoc path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc path="/remarks/para[@id='terminator-required']"/>
    ///     <para id="algo">
    ///         This implementation inverts the BWT by using the last-first property.
    ///         - First column of the matrix (sBWT) is just the last column (BWT), sorted.
    ///           <br/>
    ///         - By last-first property, the 1-st (and only) occurrence of terminator in sBWT at sBWT[0] corresponds 
    ///           to the 1st occurrence of terminator in BWT at BWT[i0]. BWTs[i0] is the 1-st char of the text.
    ///           <br/>
    ///         - Again by last-first property, the n-th occurrence of c in BWTs at sBWTs[i0] corresponds to the n-th
    ///           occurrence of c in BWT at BWT[i1]. BWTs[i1] is the 2-st char of the text.
    ///           <br/>
    ///         - And so on, until BWTs[i(n-1)], the terminator, is reached.
    ///     </para>
    ///     <para id="complexity">
    ///         Complexity:
    ///         <br/>
    ///         - Before any iteration, Sorted BWT is computed, taking O(n * log(n)) time, where n is the length of 
    ///           <paramref name="lastBWMColumn"/>. If the alphabet is of constant size sigma, Counting Sort reduces 
    ///           the overall Time Complexity of this step to O(n).
    ///           <br/>
    ///         - After that the finder may also preallocate other supporting structures, to speed up searches (such
    ///           the dictionary used in <see cref="PrecomputedFinder"/>. Although it depends on the specific
    ///           implementation built by <see cref="FirstLastFinderBuilder"/>, we may assume this cost to also be 
    ///           linear with n.
    ///           <br/>
    ///         - From terminator to terminator, there are n top-level iterations. Each iteration takes m1 + m2, 
    ///           where m1 is the cost of <see cref="ILastFirstFinder.FindIndexOfNthOccurrenceInBWT(char, int)"/> 
    ///           and m2 is the cost of <see cref="ILastFirstFinder.FindOccurrenceRankOfCharInSortedBWT(int)"/>.
    ///           <br/>
    ///         - Finally, the <see cref="StringBuilder"/> used as accumulator generates the text string. At most O(n).
    ///           <br/>
    ///         - So total Time Complexity is O(n * (m1 + m2)) and Space Complexity is O(n).
    ///         <br/>
    ///         <br/>
    ///         Using <see cref="NaiveFinder"/>, m1 and m2 are both O(n), so Time Complexity is O(n^2).
    ///         <br/>
    ///         Using <see cref="BinarySearchFinder"/>, m1 is O(n) and m2 is O(log(n)), so overall Time 
    ///         Complexity is still O(n^2).
    ///         <br/>
    ///         Using <see cref="PrecomputedFinder"/>, m1 is O(1), whereas m2 is O(log(n / sigma)) where 
    ///         sigma is the size of the alphabet, so overall Time Complexity is O(n * log(n)) if sigma is constant.
    ///         <br/>
    ///     </para>
    /// </remarks>
    public override TextWithTerminator InvertTransform(RotatedTextWithTerminator lastBWMColumn)
    {
        var terminator = lastBWMColumn.Terminator;
        var firstLastFinder = FirstLastFinderBuilder(lastBWMColumn);
        var sbwt = firstLastFinder.SortedBWT;

        var text = new StringBuilder();

        var index = 0;
        var occurrenceRank = 0; // Remark: occurrence ranks are 0-based (0 is 1st occurrence)

        do
        {
            index = firstLastFinder.FindIndexOfNthOccurrenceInBWT(sbwt[index], occurrenceRank);
            occurrenceRank = firstLastFinder.FindOccurrenceRankOfCharInSortedBWT(index);
            if (sbwt[index] != terminator) 
                text.Append(sbwt[index]);
        }
        while (sbwt[index] != terminator);

        return new(text.ToString().AsValue(), terminator);
    }
}
