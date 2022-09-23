using MoreStructures.Stacks;

namespace MoreStructures.Tests.Stack;

[TestClass]
public class LinkedListStackTests : StackTests
{
    public LinkedListStackTests() : base(() => new LinkedListStack<int>())
    {
    }
}
