using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreStructures.Tests.Utilities;

[TestClass]
public class NumericUtilitiesTests
{
    [TestMethod]
    public void Middle_IsCorrect()
    {
        Assert.AreEqual(4, (2, 6).Middle());
        Assert.AreEqual(3, (0, 6).Middle());
        Assert.AreEqual(1, (2, 0).Middle());
        Assert.AreEqual(0, (0, 0).Middle());
        Assert.AreEqual(-2, (-3, -1).Middle());
        Assert.AreEqual(-1, (-3, 1).Middle());

        Assert.AreEqual(0, (int.MinValue / 2 + 1, int.MaxValue / 2).Middle());

        Assert.AreEqual(int.MaxValue / 2 + 1, (int.MaxValue / 2, int.MaxValue / 2 + 2).Middle());
        Assert.AreEqual(int.MaxValue / 2 + 2, (int.MaxValue / 2 + 1, int.MaxValue / 2 + 3).Middle());
        Assert.AreEqual(int.MaxValue / 2 + 1, (int.MaxValue / 2, int.MaxValue / 2 + 2).Middle());
        
        Assert.AreEqual(int.MinValue / 2 - 1, (int.MinValue / 2, int.MinValue / 2 - 2).Middle());
        Assert.AreEqual(int.MinValue / 2 - 2, (int.MinValue / 2 - 1, int.MinValue / 2 - 3).Middle());
        Assert.AreEqual(int.MinValue / 2 - 1, (int.MinValue / 2, int.MinValue / 2 - 2).Middle());
    }
}
