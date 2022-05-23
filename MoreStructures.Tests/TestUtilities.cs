using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreStructures.Tests
{
    internal static class TestUtilities
    {
        public static TextWithTerminator ExampleText1 => new("aaa");
        public static TextWithTerminator ExampleText2 => new("ababaa");
    }
}
