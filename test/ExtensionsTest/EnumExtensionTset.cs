using Extensions;
using System;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace ExtensionsTest
{
    public partial class EnumExtensionTset
    {

        [Fact]
        public void ShouidPass_GetItems_ValidInput()
        {
            var value = EnumExtensions.GetItems<TestEnum>();
            var selected = value.Where(x => x.Value == 1).FirstOrDefault();

            Assert.True(selected.Description == "Hashemi's name");
            Assert.True(selected.Value == 1);
        }

        [Fact]
        public void ShouidPass_GetEnumAttribute_ValidInput()
        {
            var value = typeof(TestEnum).GetEnumAttribute<DescriptionAttribute>("Mahdi");

            Assert.True(value.Description == "my name");
        }

        [Fact]
        public void Should_Return_AttrValue()
        {
            var attr = TestEnum.Hosein.GetAttributeValue<TestAttribute>();
        
            Assert.True(attr.Value == "Hassan Hsahemi");

            Assert.True(TestEnum.Hassan.GetAttributeValue<TestAttribute>() == null);
        }
    }
}
