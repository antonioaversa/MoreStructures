using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreStructures.Strings.Sorting;

/// <summary>
/// An algorithm sorting the chars of the provided string.
/// </summary>
public interface ICharsSorter
{
    /// <summary>
    /// Sorts the chars of <paramref name="input"/>, returning the list of positions of each of the chars in 
    /// <paramref name="input"/> (i.e. the item of the order list O at index i, O[i], is the position in the sorted 
    /// string, of the char in <paramref name="input"/> with index i).
    /// </summary>
    /// <param name="input">The string whose <see cref="char"/> items have to be sorted.</param>
    /// <returns>A list of position in the sorted string: 0 being the first in the order, 1 the second, etc.</returns>
    IList<int> Sort(string input);
}
