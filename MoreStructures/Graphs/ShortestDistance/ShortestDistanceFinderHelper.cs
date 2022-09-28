using MoreStructures.PriorityQueues;
using MoreStructures.PriorityQueues.Extensions;

namespace MoreStructures.Graphs.ShortestDistance;

using GraphDistances = IDictionary<(int, int), int>;

internal static class ShortestDistanceFinderHelper
{
    public static void ValidateParameters(IGraph graph, int start, int? end = null)
    {
        var numberOfVertices = graph.GetNumberOfVertices();
        if (start < 0 || start >= numberOfVertices)
            throw new ArgumentException(
                "Must be non-negative and smaller than the total number of vertices in the graph.", nameof(start));
        if (end != null && (end < 0 || end >= numberOfVertices))
            throw new ArgumentException(
                "Must be non-negative and smaller than the total number of vertices in the graph.", nameof(end));
    }

    public static IList<int> BuildShortestPath(int end, BestPreviouses bestPreviouses)
    {
        var shortestPath = new List<int> { end };

        int current = end;
        while (bestPreviouses.Values.TryGetValue(current, out var bestPrevious))
        {
            current = bestPrevious.PreviousVertex;
            if (current < 0)
                break;

            shortestPath.Add(current);
        }

        shortestPath.Reverse();
        return shortestPath;
    }

    public static void RelaxOutgoingEdgesOfVertex(
        IGraph graph, 
        GraphDistances distances, 
        BestPreviouses bestPreviouses, 
        HashSet<int> added, IUpdatablePriorityQueue<int> vertexes, int lastAdded)
    {
        foreach (var (vertex, edgeStart, edgeEnd) in graph.GetAdjacentVerticesAndEdges(lastAdded, true))
        {
            if (added.Contains(vertex))
                continue;

            var edgeDistance = distances[(edgeStart, edgeEnd)];
            if (edgeDistance < 0)
                throw new InvalidOperationException(
                    $"Negative edges are not supported: distance of ({edgeStart}, {edgeEnd}) = {edgeDistance}.");

            var newDistance = bestPreviouses.Values[lastAdded].DistanceFromStart + edgeDistance;
            if (!bestPreviouses.Values.TryGetValue(vertex, out var bestPrevious) ||
                bestPrevious.DistanceFromStart > newDistance)
            {
                bestPreviouses.Values[vertex] = new(newDistance, lastAdded);
                vertexes.PushOrUpdate(vertex, -newDistance);
            }
        }
    }
}
