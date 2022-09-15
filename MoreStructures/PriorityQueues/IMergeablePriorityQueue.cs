namespace MoreStructures.PriorityQueues;

/// <summary>
/// An <see cref="IPriorityQueue{T}"/> which supports efficient merging of the items from a second 
/// <see cref="IMergeablePriorityQueue{T, TPQTarget}"/> of type <typeparamref name="TPQTarget"/>.
/// priority.
/// </summary>
/// <remarks>
///     <para id="advantages">
///     ADVANTAGES AND DISADVANTAGES
///     <br/>
///     - <see cref="MergeFrom"/> from a source S can be implemented in a general way by copying the entire target data 
///       structure T and then performing m <see cref="IPriorityQueue{T}.Pop"/>, where m is the number of items in T, 
///       each followed by a <see cref="IPriorityQueue{T}.Push"/> into S.
///       <br/>
///     - This approach has the advantage of being general and not mutating the target structure when performing the 
///       merge. 
///       <br/>
///     -  It is, however, expensive both in time and space, having O(m * log(m)) Time and O(n) Space Complexity for
///       all known implementation of <see cref="IPriorityQueue{T}"/>.
///       <br/>
///     - Implementing this interface can take advantage of the properties of the underlying data structure 
///       implementing the priority queue, and providing better performance.
///     </para>
///     <para id="complexity">
///     COMPLEXITY
///     <br/>
///     - Notice that linear performance can be achieved by basic implementations of <see cref="IPriorityQueue{T}"/>, 
///       such as <see cref="ArrayList.ArrayListPriorityQueue{T}"/>, by just concatenating the underlying data 
///       structures.
///       <br/>
///     - In cases such as <see cref="BinaryHeap.BinaryHeapPriorityQueue{T}"/>, after concatenating, heap constraints 
///       have to be restored. However, this operation too, can be performed in linear time.
///       <br/>
///     - An implementation based on linked lists would perform the merge in O(1) time, by just concatenating the two 
///       underlying structures. That is a the cost of all subsequent operations on the queue.
///       <br/>
///     - Implementations based on forest of heap trees can do it in O(log(n)) or even O(1), when lazy, and keep 
///       logarithmic time for all operations on the resulting queue.
///     </para>
///     <para id="side-effects">
///     SIDE EFFECTS
///     <br/>
///     - However, for sub-linear performance to be achieved, both with linked lists and forests of heaps, some form of
///       <b>structure sharing</b> between source and target is required, because replicating the content of the target
///       would take linear time.
///       <br/>
///     - To avoid interferences between the queues after merge, the target is emptied out during merge. 
///       <br/>
///     - This is an operation which usually takes sub-linear time, so it doesn't affect the overall complexity of the 
///       merge operation, and avoid post-merge side effects between the queues.
///     </para>
/// </remarks>
public interface IMergeablePriorityQueue<T, in TPQTarget> : IPriorityQueue<T>
    where T : notnull
    where TPQTarget : IMergeablePriorityQueue<T, TPQTarget>
{
    /// <summary>
    /// Merges all items the <paramref name="targetPriorityQueue"/> into this priority queue, emptying out the content 
    /// of the <paramref name="targetPriorityQueue"/>.
    /// </summary>
    /// <param name="targetPriorityQueue">
    /// The <see cref="IMergeablePriorityQueue{T, TPQTarget}"/>, to take the items from.
    /// </param>
    /// <remarks>
    ///     <inheritdoc cref="IMergeablePriorityQueue{T, TPQTarget}"/>
    /// </remarks>
    void MergeFrom(TPQTarget targetPriorityQueue);

    /// <summary>
    /// Clears this queue, wiping out all its items.
    /// </summary>
    /// <remarks>
    /// Used by <see cref="MergeFrom(TPQTarget)"/> on the target <see cref="IMergeablePriorityQueue{T, TPQTarget}"/>.
    /// </remarks>
    void Clear();
}
