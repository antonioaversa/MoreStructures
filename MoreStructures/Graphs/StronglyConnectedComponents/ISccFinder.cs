namespace MoreStructures.Graphs.StronglyConnectedComponents;

/// <summary>
/// An algorithm finding the Strongly Connected Components (SCC) of a <see cref="IGraph"/>.
/// </summary>
/// <remarks>
///     <para id="definition">
///     DEFINITION
///     <br/>
///     - A Strongly Connected Component (SCC) of a graph G is a set of vertices of G such that, taken any two vertices
///       v1 and v2 of G, there is a directed path from v1 to v2 and a directed path from v2 to v1.
///       <br/>
///     - That is, every vertex in a SCC can reach and can be reached by any other vertex of the same SCC, whereas two
///       vertices of two different SCC can be such that one is reachable from the other, but not viceversa.
///       <br/>
///     - A SCC is an extension specific to directed graphs of the concept of Connected Component, which instead only
///       requires a path from v1 to v2 for CC[v1] to be the same of CC[v2].
///       <br/>
///     - While in an undirected graph a path from v1 to v2 implies a path from v2 to v1, that's not the case for 
///       directed graphs. So bidirectionality is required.
///     </para>
/// </remarks>
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
    public IList<int> Find(IGraph graph);
}
