using Extensions;
using Project.Base;
using System;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace ExtensionsTest
{
    public class EnumExtensionTset
    {
        enum testEnum
        {
            [DescriptionAttribute("my name")]
            Mahdi = 0,
            [DescriptionAttribute("Hashemi's name")]
            Hassan = 1,
            Hosein = 2
        }

        [Fact]
        public void Shouid_Pass_GetItems_ValidInput_Test()
        {
            var value = EnumExtensions.GetItems<testEnum>();
            var selected = value.Where(x => x.Value == 1).FirstOrDefault();
            Assert.True(selected.Description == "Hashemi's name");
            Assert.True(selected.Value == 1);
        }

        [Fact]
        public void Shouid_Pass_GetEnumAttribute_ValidInput_Test()
        {
            var value = typeof(testEnum).GetEnumAttribute<DescriptionAttribute>("Mahdi");
            Assert.True(value.Description == "my name");

        }

    }
}
