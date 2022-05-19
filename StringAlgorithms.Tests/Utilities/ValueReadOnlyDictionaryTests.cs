using Microsoft.VisualStudio.TestTools.UnitTesting;
using StringAlgorithms.Utilities;
using System.Collections.Generic;

namespace StringAlgorithms.Tests.Utilities
{
    [TestClass]
    public class ValueReadOnlyDictionaryTests
    {
        [TestMethod]
        public void Ctor_EmbedInputEnumerable()
        {
            var dictionary = new Dictionary<string, int>() { ["a"] = 1, ["b"] = 2 };
            var valueDictionary = new ValueReadOnlyDictionary<string, int>(dictionary);
            Assert.AreEqual(2, valueDictionary.Count);
            Assert.AreEqual(1, valueDictionary["a"]);
            Assert.AreEqual(2, valueDictionary["b"]);
        }

        [TestMethod]
        public void Ctor_IndependenceFromInputEnumerable()
        {
            var dictionary = new Dictionary<string, int>() { ["a"] = 1, ["b"] = 2 };
            int initialCount = dictionary.Count;
            var valueDictionary = new ValueReadOnlyDictionary<string, int>(dictionary);

            dictionary["c"] = 3;
            Assert.AreEqual(initialCount, valueDictionary.Count);

            dictionary.Remove("a");
            Assert.AreEqual(initialCount, valueDictionary.Count);
        }

        [TestMethod]
        public void Ctor_WithEnumerableOfEntries()
        {
            var dictionary = new Dictionary<string, int>() 
            { 
                ["a"] = 1, 
                ["b"] = 2 
            };
            var entries = new List<KeyValuePair<string, int>> 
            {
                KeyValuePair.Create("a", 1),
                KeyValuePair.Create("b", 2),
            };
            var valueDictionary1 = new ValueReadOnlyDictionary<string, int>(dictionary);
            var valueDictionary2 = new ValueReadOnlyDictionary<string, int>(entries);
            Assert.AreEqual(valueDictionary1, valueDictionary2);
        }

        [TestMethod]
        public void Equals_CheckType()
        {
            var valueDictionary1 = new ValueReadOnlyDictionary<string, int>(
                new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 });
            var dictionary = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 };
            Assert.IsFalse(valueDictionary1.Equals(dictionary));
        }


        [TestMethod]
        public void Equals_IsByValue()
        {
            var valueDictionary1 = new ValueReadOnlyDictionary<string, int>(
                new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 });
            var valueDictionary2 = new ValueReadOnlyDictionary<string, int>(
                new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 });
            Assert.IsTrue(valueDictionary1.Equals(valueDictionary2));

            var valueDictionary3 = new ValueReadOnlyDictionary<string, int>(
                new Dictionary<string, int> { ["b"] = 2 });
            Assert.IsFalse(valueDictionary1.Equals(valueDictionary3));

            var valueDictionary4 = new ValueReadOnlyDictionary<string, int>(
                new Dictionary<string, int> { ["a"] = 2, ["b"] = 2 });
            Assert.IsFalse(valueDictionary1.Equals(valueDictionary4));
        }

        [TestMethod]
        public void EqualsOperator_IsByValue()
        {
            var valueDictionary1 = new ValueReadOnlyDictionary<string, int>(
                new Dictionary<string, int> { ["a"] = 1, ["b"] = 1 });
            var valueDictionary2 = new ValueReadOnlyDictionary<string, int>(
                new Dictionary<string, int> { ["a"] = 1, ["b"] = 1 });
            Assert.IsTrue(valueDictionary1 == valueDictionary2);

            var valueDictionary3 = new ValueReadOnlyDictionary<string, int>(
                new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 });
            Assert.IsFalse(valueDictionary1 == valueDictionary3);

            var valueDictionary4 = new ValueReadOnlyDictionary<string, int>(
                new Dictionary<string, int> { ["a"] = 2, ["b"] = 2 });
            Assert.IsFalse(valueDictionary1 == valueDictionary4);
        }

        [TestMethod]
        public void DifferentOperator_IsByValue()
        {
            var valueDictionary1 = new ValueReadOnlyDictionary<string, int>(
                new Dictionary<string, int> { ["a"] = 1, ["b"] = 1 });
            var valueDictionary2 = new ValueReadOnlyDictionary<string, int>(
                new Dictionary<string, int> { ["a"] = 1, ["b"] = 1 });
            Assert.IsFalse(valueDictionary1 != valueDictionary2);

            var valueDictionary3 = new ValueReadOnlyDictionary<string, int>(
                new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 });
            Assert.IsTrue(valueDictionary1 != valueDictionary3);

            var valueDictionary4 = new ValueReadOnlyDictionary<string, int>(
                new Dictionary<string, int> { ["a"] = 2, ["b"] = 2 });
            Assert.IsTrue(valueDictionary1 != valueDictionary4);
        }

        [TestMethod]
        public void GetHashCode_IsByValue_WithPrimitiveType()
        {
            var dictionary1 = new Dictionary<string, int> { ["a"] = 1, ["b"] = 1 };
            var valueDictionary1 = new ValueReadOnlyDictionary<string, int>(dictionary1);
            var dictionary2 = new Dictionary<string, int> { ["a"] = 1, ["b"] = 1 };
            var valueDictionary2 = new ValueReadOnlyDictionary<string, int>(dictionary2);
            Assert.IsTrue(valueDictionary1.GetHashCode() == valueDictionary2.GetHashCode());
        }

        private record DictionaryKey(int V1, string V2);
        private record DictionaryValue(bool V1, char V2);

        [TestMethod]
        public void GetHashCode_IsByValue_WithRecordType()
        {
            var dictionary1 = new Dictionary<DictionaryKey, DictionaryValue>
            {
                [new(1, "a")] = new(true, 'a'),
                [new(2, "b")] = new(true, 'b'),
            };
            var valueDictionary1 = new ValueReadOnlyDictionary<DictionaryKey, DictionaryValue>(dictionary1);
            var dictionary2 = new Dictionary<DictionaryKey, DictionaryValue> 
            {
                [new(1, "a")] = new(true, 'a'),
                [new(2, "b")] = new(true, 'b'),
            };
            var valueDictionary2 = new ValueReadOnlyDictionary<DictionaryKey, DictionaryValue>(dictionary2);
            Assert.IsTrue(valueDictionary1.GetHashCode() == valueDictionary2.GetHashCode());
        }

        [TestMethod]
        public void ToString_IncludesToStringOfKeysAndValues()
        {
            var dictionary = new Dictionary<string, int> { ["el1"] = 123, ["el2"] = 456 };
            var valueDictionary1 = new ValueReadOnlyDictionary<string, int>(dictionary);
            var valueDictionary1Str = valueDictionary1.ToString();

            foreach (var item in dictionary)
            {
                Assert.IsTrue(valueDictionary1Str.Contains(item.Key.ToString()));
                Assert.IsTrue(valueDictionary1Str.Contains(item.Value.ToString()));
            }
        }
    }
}
