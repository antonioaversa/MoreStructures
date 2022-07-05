using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreStructures.Tests.KnuthMorrisPratt.Borders;

/// <summary>
/// An algorithm extracting <b>borders</b> from the given text.
/// </summary>
/// <remarks>
///     <para id="definition">
///     DEFINITION
///     <br/>
///     A border of a text T of a prefix of T which is also a suffix of T, T excluded.
///     <br/>
///     For example, the borders of <c>T = "abcabeabcab"</c> are <c>"a"</c>, <c>"ab"</c> and <c>"abcab"</c>.
///     </para>
/// </remarks>
public interface IBordersExtraction
{
    /// <summary>
    /// Retrieves all borders from the provided text, by decreasing length.
    /// </summary>
    /// <param name="text">The text, to extract borders from.</param>
    /// <returns>An enumerable of strings.</returns>
    IEnumerable<string> GetAllBordersByDescLength(string text);
}
