using MoreStructures.Queues;

namespace MoreStructures.Tests.Queues;

[TestClass]
public class LinkedListQueueTests : QueueTests
{
    protected override IQueue<T> Build<T>() => new LinkedListQueue<T>();
}
