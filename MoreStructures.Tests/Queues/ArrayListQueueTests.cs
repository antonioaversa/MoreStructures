using MoreStructures.Queues;

namespace MoreStructures.Tests.Queues;

[TestClass]
public class ArrayListQueueTests : QueueTests
{
    protected override IQueue<T> Build<T>() => new ArrayListQueue<T>();
}
