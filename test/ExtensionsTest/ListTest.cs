using System;
using System.Collections.Generic;
using Xunit;

namespace ExtensionsTest
{
    public class ListTest
    {
        private class ListItem
        {
            public string Property { get; set; }
        }

        [Fact]
        public void Should_Swap_items()
        {
            var first = new ListItem { Property = "val1" };
            var second = new ListItem { Property = "val2" };
            var third = new ListItem { Property = "val3" };
            var fourth = new ListItem { Property = "val4" };

            var list = new List<ListItem>() { first, second, third, fourth };

            list.Swap(first, third);

            Assert.True(list[0].Property == "val3");
            Assert.True(list[2].Property == "val1");
        }
    }
}
