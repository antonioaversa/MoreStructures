using MoreStructures.RecImmTrees.Paths;

namespace MoreStructures.Tests.RecImmTrees.Paths;

[TestClass]
public class FullyIterativeNodeToLeafPathsBuilderTests : NodeToLeafPathsBuilderTests
{
    public FullyIterativeNodeToLeafPathsBuilderTests() : base(new FullyIterativeNodeToLeafPathsBuilder())
    {
    }
}