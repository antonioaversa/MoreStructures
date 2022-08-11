namespace MoreStructures.EdgeListGraphs;

/// <summary>
/// A graph data structure, represented as an unsorted list of unlabelled edges, connecting unlabelled vertices.
/// </summary>
/// <param name="NumberOfVertices">
/// The total number n of vertices in the graph, identified with ids from 0 to n - 1.
/// </param>
/// <param name="Edges">
/// The list of edges of the graph, each one represented as a couple of ids of the vertices which constitute the 
/// extremes of the edge. Edges can be considered as directional or not, depending on the scenario.
/// </param>
/// <remarks>
/// If the edges are considered directional, i.e. (s, e) is considered as a different edge from (e, s), the resulting
/// graph is directed. Otherwise, the resulting graph is undirected.
/// </remarks>
public record EdgeListGraph(int NumberOfVertices, IList<(int start, int end)> Edges);
