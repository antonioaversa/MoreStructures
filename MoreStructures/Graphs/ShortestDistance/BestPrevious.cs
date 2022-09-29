namespace MoreStructures.Graphs.ShortestDistance;

/// <summary>
/// Represents the best estimate of the shortest distance of a vertex in a graph, from a given start vertex.
/// Includes the id of the previous vertex, to reconstruct a path of such shortest distance.
/// </summary>
/// <param name="DistanceFromStart">
/// The shortest distance from the start vertex s to the vertex v. Possibly negative.
/// </param>
/// <param name="PreviousVertex">
/// The id of the vertex in the graph which preceeds the vertex v, in one of the shortest paths from start to v.
/// <br/>
/// It's negative when the vertex v has no predecessor, because v corresponds to s.
/// </param>
/// <remarks>
///     Given:
///     <br/>
///     - a starting vertex s and another vertex v, both from the set of vertices V of a graph G,
///       <br/>
///     - and a mapping from V to the corresponding <see cref="BestPrevious"/> estimate of shortest path from s,
///       <br/>
///     the full path <see cref="PreviousVertex"/> is <b>enough to reconstruct the entire path</b> from v to s. 
///     <br/>
///     <br/>
///     That is thanks to the following properties of optimal paths:
///     <br/>
///     - given a correct mapping from vertices to optimal paths for a shortest path, BP;
///       <br/>
///     - given an optimal path Pv from s to v, where <c>u = BP[v].PreviousVertex</c> is the immediate predecessor of 
///       v in Pv;
///       <br/>
///     - given an optimal path Pu from s to u, where <c>w = BP[u].PreviousVertex</c> is the immediate predecessor of
///       u in Pu;
///       <br/>
///     - given the path <c>Pu' = Pv - (u, v)</c>, with Pu is different Pu';
///       <br/>
///     Then:
///     <br/>
///     - the path Pu', although different than Pu, is also an optimal path from s to u;
///       <br/>
///     - the path <c>Pv' = Pu union (u, v)</c>, although different than Pv, is also an optimal path from s to v.
///     <br/>
///     <br/>
///     That is, sub-paths of optimal paths are also optimal, and replacing a sub-path in an optimal path by a 
///     different optimal sub-path yiels a path different than the initial one, yet optimal.
/// </remarks>
public record struct BestPrevious(int DistanceFromStart, int PreviousVertex);
