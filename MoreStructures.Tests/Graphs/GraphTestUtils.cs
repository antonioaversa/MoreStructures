namespace MoreStructures.Tests.Graphs;

internal static class GraphTestUtils
{
    public static IList<ISet<int>> BuildNeighborhoods(int numberOfVertices, IList<(int start, int end)> edges)
    {
        var neighborhoods = new ISet<int>[numberOfVertices];
        for (var i = 0; i < numberOfVertices; i++)
        {
            neighborhoods[i] = new HashSet<int>();
        }

        foreach (var edge in edges)
        {
            neighborhoods[edge.start].Add(edge.end);
        }

        return neighborhoods;
    }

    public static bool[,] BuildMatrix(int numberOfVertices, IList<(int start, int end)> edges)
    {
        var matrix = new bool[numberOfVertices, numberOfVertices];

        foreach (var (start, end) in edges)
        {
            matrix[start, end] = true;
        }

        return matrix;
    }
}
