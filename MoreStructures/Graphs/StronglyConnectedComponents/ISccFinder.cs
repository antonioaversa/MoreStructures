using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreStructures.Graphs.StronglyConnectedComponents;

/// <summary>
/// An algorithm finding the Strongly Connected Components (SCC) of a <see cref="IGraph"/>.
/// </summary>
public interface ISccFinder
{
    /// <summary>
    /// Finds the Strongly Connected Components (SCC) of the provided <paramref name="graph"/>.
    /// </summary>
    /// <param name="graph">An <see cref="IGraph"/> instance of any type, with or without loops.</param>
    /// <returns>
    /// A list L of as many <see cref="int"/> as the number of vertices in <paramref name="graph"/>.
    /// <br/>
    /// The i-th element of the L represents the label of the SCC, the vertex i is in.
    /// </returns>
    public IList<int> FindScc(IGraph graph);
}
