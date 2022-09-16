namespace MoreStructures.Lists.Sorting;

/// <summary>
/// Used to mark sorting algorithms (in place or not), which are <b>stable</b>. Just for marking and information.
/// </summary>
/// <remarks>
/// A sorting algorithm is stable when for any two items of the input list L, L[i] and L[j] with i != j, which are
/// equivalent acconding to the IComparable of IComparer being used, if i &lt; j then i* &lt; j*, where i* and j*
/// are the indexes of L[i] and L[j] in the sorted version of L given as output.
/// <br/>
/// In other words, a stable sorting algorithm preserves the input order of equivalent items from the input to the
/// output.
/// <br/>
/// Standard implementations of QuickSort and HeapSort are examples of non-stable algorithms.
/// <br/>
/// QuickSort partitioning, depending on how the pivot is selected and where it ends up being, can change relative 
/// position of equivalent items.
/// <br/>
/// HeapSort may also change the order, since Sift Down and Up operation may swap relative position of equivalent 
/// items, either during heap construction or during heap rearrangement on pop.
/// <br/>
/// MergeSort can be implemented as a stable algorithm, if the 2-way merge procedure prefers the lower index when 
/// choice between two equivalent items, one from the lower half and one from the higher half, is given.
/// <br/>
/// Selection and Insertion Sort can both be implemented as stable sorting algorithms.
/// </remarks>
public interface IStableSortingAlgorithm
{
}
