using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MoreStructures.Tests.Utilities;

[ExcludeFromCodeCoverage(Justification = "Mock structure only partially used")]
internal class GenericListMock : IList<int>
{
    private readonly int _count;

    public int ElementReturned { get; set; } = 0;

    public GenericListMock(int count)
    {
        _count = count;
    }

    public int this[int index] 
    { 
        get => ElementReturned;
        set { } 
    }

    public int Count => _count;

    public bool IsReadOnly => throw new NotImplementedException();

    public void Add(int item)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(int item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(int[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<int> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public int IndexOf(int item)
    {
        throw new NotImplementedException();
    }

    public void Insert(int index, int item)
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

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
