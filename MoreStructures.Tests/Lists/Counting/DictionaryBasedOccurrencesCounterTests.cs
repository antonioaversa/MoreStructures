using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Lists.Counting;

namespace MoreStructures.Tests.Lists.Counting;

[TestClass]
public class DictionaryBasedOccurrencesCounterTests : OccurrencesCounterTests
{
    public DictionaryBasedOccurrencesCounterTests() 
        : base(new DictionaryBasedOccurrencesCounter())
    {
    }
}
