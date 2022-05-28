using MoreStructures.BurrowsWheelerTransform.Builders.LastFirstFinders;
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
public partial class LastFirstPropertyBasedBuilder : NaiveBuilder
{
    /// <summary>
    /// The strategy by which this builder finds chars in the BWT and its sorted version.
    /// </summary>
    public Func<RotatedTextWithTerminator, ILastFirstFinder> FirstLastFinderBuilder { get; init; } =
        (lastBWMColumn) => new PrecomputedDictionaryLastFirstFinder(lastBWMColumn);

    /// <inheritdoc cref="IBuilder.InvertTransform(RotatedTextWithTerminator)" path="//*[not(self::remarks)]"/>
    /// <remarks>
    ///     <inheritdoc cref="IBuilder.InvertTransform(RotatedTextWithTerminator)" 
    ///         path="/remarks/para[@id='terminator-required']"/>
    ///     <para id="algo">
    ///         This implementation inverts the BWT by using the last-first property.
    ///         <para>
    ///         - First column of the matrix (sBWT) is just the last column (BWT), sorted.
    ///         - By last-first property, the 1-st (and only) occurrence of terminator in sBWT at sBWT[0] corresponds 
    ///           to the 1st occurrence of terminator in BWT at BWT[i0]. BWTs[i0] is the 1-st char of the text.
    ///         - Again by last-first property, the n-th occurrence of c in BWTs at sBWTs[i0] corresponds to the n-th
    ///           occurrence of c in BWT at BWT[i1]. BWTs[i1] is the 2-st char of the text.
    ///         - And so on, until BWTs[i(n-1)], the terminator, is reached.
    ///         </para>
    ///     </para>
    /// </remarks>
    public override TextWithTerminator InvertTransform(RotatedTextWithTerminator lastBWMColumn)
    {
        var terminator = lastBWMColumn.Terminator;
        var firstLastFinder = FirstLastFinderBuilder(lastBWMColumn);
        var sbwt = firstLastFinder.SortedBWT;

        var text = new StringBuilder();

        var index = 0;
        var occurrence = 0; // Remark: occurrences are 0-based (0 is 1st occurrence)

        do
        {
            index = firstLastFinder.FindIndexOfNthOccurrenceInBWT(sbwt[index], occurrence);
            occurrence = firstLastFinder.FindOccurrenceOfCharInSortedBWT(index);
            if (sbwt[index] != terminator) 
                text.Append(sbwt[index]);
        }
        while (sbwt[index] != terminator);

        return new(text.ToString(), terminator);
    }
}
