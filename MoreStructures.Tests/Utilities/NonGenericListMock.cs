using System.Collections;

namespace MoreStructures.Tests.Utilities;

[ExcludeFromCodeCoverage(Justification = "Mock structure only partially used")]
internal class NonGenericListMock : IList, IEnumerable<int>
{
    private readonly int _count;

    public int ElementReturned { get; set; } = 0;

    public NonGenericListMock(int count)
    {
        _count = count;
    }

    public object? this[int index] 
    {
        get => ElementReturned;
        set { } 
    }

    public bool IsFixedSize => throw new NotImplementedException();

    public bool IsReadOnly => throw new NotImplementedException();

    public int Count => _count;

    public bool IsSynchronized => throw new NotImplementedException();

    public object SyncRoot => throw new NotImplementedException();

    public int Add(object? value)
    {
        throw new NotImplementedException();
    }

    public void Add(int item)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(object? value)
    {
        throw new NotImplementedException();
    }

    public bool Contains(int item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(Array array, int index)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(int[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public int IndexOf(object? value)
    {
        throw new NotImplementedException();
    }

    public int IndexOf(int item)
    {
        throw new NotImplementedException();
    }

    public void Insert(int index, object? value)
    {
        throw new NotImplementedException();
    }

    public void Insert(int index, int item)
    {
        throw new NotImplementedException();
    }

    public void Remove(object? value)
    {
        throw new NotImplementedException();
    }

    public bool Remove(int item)
    {
        throw new NotImplementedException();
    }

    public void RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    IEnumerator<int> IEnumerable<int>.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
