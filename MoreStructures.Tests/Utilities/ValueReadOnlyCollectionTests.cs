using MoreStructures.Utilities;

namespace MoreStructures.Tests.Utilities
{
    [TestClass]
    public class ValueReadOnlyCollectionTests
    {
        [TestMethod]
        public void Ctor_EmbedInputEnumerable()
        {
            var list = new List<string>() { "a", "b" };
            var valueList = new ValueReadOnlyCollection<string>(list);
            Assert.AreEqual(2, valueList.Count);
            Assert.AreEqual("a", valueList[0]);
            Assert.AreEqual("b", valueList[1]);
        }

        [TestMethod]
        public void Ctor_IndependenceFromInputEnumerable()
        {
            var list = new List<string>() { "a", "b" };
            int initialCount = list.Count;
            var valueList = new ValueReadOnlyCollection<string>(list);
            list.Add("c");
            Assert.AreEqual(initialCount, valueList.Count);
        }

        [TestMethod]
        public void Equals_CheckForNull()
        {
            var valueList1 = new ValueReadOnlyCollection<string>(new List<string> { "a" });
            Assert.IsFalse(valueList1.Equals(null as object));
            Assert.IsFalse(valueList1.Equals(null));
        }

        [TestMethod]
        public void Equals_CheckType()
        {
            var valueList1 = new ValueReadOnlyCollection<string>(new List<string> { "a" });
            var list2 = new List<string> { "a" };
            Assert.IsFalse(valueList1.Equals(list2));
        }

        [TestMethod]
        public void Equals_IsByValue()
        {
            var valueList1 = new ValueReadOnlyCollection<string>(new List<string> { "a" });
            var valueList2 = new ValueReadOnlyCollection<string>(new List<string> { "a" });
            Assert.IsTrue(valueList1.Equals(valueList2));

            var valueList3 = new ValueReadOnlyCollection<string>(new List<string> { "b" });
            Assert.IsFalse(valueList1.Equals(valueList3));
        }

        [TestMethod]
        public void Equals_IsSensitiveToTheOrderOfAddition()
        {
            var valueList1 = new ValueReadOnlyCollection<string>(new List<string> { "a", "b" });
            var valueList2 = new ValueReadOnlyCollection<string>(new List<string> { "b", "a" });
            Assert.IsFalse(valueList1.Equals(valueList2));
        }

        [TestMethod]
        public void EqualsOperator_IsByValue()
        {
            var valueList1 = new ValueReadOnlyCollection<string>(new List<string> { "a", "a" });
            var valueList2 = new ValueReadOnlyCollection<string>(new List<string> { "a", "a" });
            Assert.IsTrue(valueList1 == valueList2);

            var valueList3 = new ValueReadOnlyCollection<string>(new List<string> { "a", "b" });
            Assert.IsFalse(valueList1 == valueList3);
        }

        [TestMethod]
        public void DifferentOperator_IsByValue()
        {
            var valueList1 = new ValueReadOnlyCollection<string>(new List<string> { "a", "a" });
            var valueList2 = new ValueReadOnlyCollection<string>(new List<string> { "a", "a" });
            Assert.IsFalse(valueList1 != valueList2);

            var valueList3 = new ValueReadOnlyCollection<string>(new List<string> { "a", "b" });
            Assert.IsTrue(valueList1 != valueList3);
        }

        [TestMethod]
        public void GetHashCode_WorksWithEmptyCollection()
        {
            var list1 = new List<string> { };
            var valueList1 = new ValueReadOnlyCollection<string>(list1);
            var list2 = new List<string> { };
            var valueList2 = new ValueReadOnlyCollection<string>(list2);
            Assert.IsTrue(valueList1.GetHashCode() == valueList2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCode_IsByValue_WithPrimitiveType()
        {
            var list1 = new List<string> { "a", "a" };
            var valueList1 = new ValueReadOnlyCollection<string>(list1);
            var list2 = new List<string> { "a", "a" };
            var valueList2 = new ValueReadOnlyCollection<string>(list2);
            Assert.IsTrue(valueList1.GetHashCode() == valueList2.GetHashCode());
        }

        private record CollectionItem(int V1, string V2);

        [TestMethod]
        public void GetHashCode_IsByValue_WithRecordType()
        {
            var list1 = new List<CollectionItem> { new (1, "a"), new(2, "b") };
            var valueList1 = new ValueReadOnlyCollection<CollectionItem>(list1);
            var list2 = new List<CollectionItem> { new(1, "a"), new(2, "b") };
            var valueList2 = new ValueReadOnlyCollection<CollectionItem>(list2);
            Assert.IsTrue(valueList1.GetHashCode() == valueList2.GetHashCode());
        }

        [TestMethod]
        public void ToString_IncludesToStringOfItems()
        {
            var list = new List<string> { "el1", "el2" };
            var valueList1 = new ValueReadOnlyCollection<string>(list);
            var valueList1Str = valueList1.ToString();

            foreach (var item in list)
            {
                Assert.IsTrue(valueList1Str.Contains(item.ToString()));
            }
        }
    }
}
