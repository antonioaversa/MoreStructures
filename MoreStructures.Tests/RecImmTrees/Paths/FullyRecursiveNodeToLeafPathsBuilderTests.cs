using MoreStructures.RecImmTrees.Paths;

namespace MoreStructures.Tests.RecImmTrees.Paths;

[TestClass]
public class FullyRecursiveNodeToLeafPathsBuilderTests : NodeToLeafPathsBuilderTests
{
    public FullyRecursiveNodeToLeafPathsBuilderTests() : base(new FullyRecursiveNodeToLeafPathsBuilder())
    {
    }
}
