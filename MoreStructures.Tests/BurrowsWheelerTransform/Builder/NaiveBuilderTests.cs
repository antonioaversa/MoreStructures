using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreStructures.BurrowsWheelerTransform.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreStructures.Tests.BurrowsWheelerTransform.Builder;

[TestClass]
public class NaiveBuilderTests : BuilderTests
{
    public NaiveBuilderTests() : base(new NaiveBuilder())
    {
    }
}
