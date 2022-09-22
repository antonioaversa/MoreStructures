namespace MoreStructures.Graphs.ShortestDistance;

internal static class ShortestDistanceFinderHelper
{
    public static void ValidateParameters(IGraph graph, int start, int end)
    {
        var numberOfVertices = graph.GetNumberOfVertices();
        if (start < 0 || start >= numberOfVertices)
            throw new ArgumentException(
                "Must be non-negative and smaller than the total number of vertices in the graph.", nameof(start));
        if (end < 0 || end >= numberOfVertices)
            throw new ArgumentException(
                "Must be non-negative and smaller than the total number of vertices in the graph.", nameof(end));
    }
}
