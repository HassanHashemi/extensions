using Extensions;
using Project.Base;
using System;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace ExtensionsTest
{
    public class ObjectExtensionTset
    {
        public class Testboject
        {
            public string Test1 { get; set; }
            public int Test2 { get; set; }
            public bool Test3 { get; set; }
        }

        [Fact]
        public void ShouldPass_HasProperty_ValidInput()
        {
            var obj = new Testboject();

            Assert.False(obj.HasProperty("Test"));
            Assert.True(obj.HasProperty("Test3"));
        }

        [Fact]
        public void ShouldThrow_NullException_HasProperty_For_NullObject()
        {
            object nullObj = null;

            Assert.Throws<ArgumentNullException>(() => nullObj.HasProperty("Test"));
        }

        [Fact]
        public void ShouldPass_GetPropertyValue_ValidInput ()
        {
            var obj = new Testboject()
            {
                Test1 = "salam",
                Test2 = 100
            };

            Assert.True((string)obj.GetPropertyValue("Test1") == "salam");
        }

        [Fact]
        public void ShouldThrow_NullException_GetPropertyValue_For_NotExistProperty()
        {
            var obj = new Testboject();

            Assert.Throws<ArgumentNullException>(() => obj.GetPropertyValue("Test4"));
        }

        [Fact]
        public void ShouldThrow_NullException_GetPropertyValue_NullObject()
        {
            object nullObj = null;

            Assert.Throws<ArgumentNullException>(() => nullObj.GetPropertyValue("Test1"));
        }

        [Fact]
        public void ShouldPass_SetProperty_ValidInput()
        {
            var obj = new Testboject();

            obj.SetProperty("Test1", "salam");

            Assert.True((string)obj.GetPropertyValue("Test1") == "salam");
        }

        [Fact]
        public void ShouldThrow_NullException_SetProperty_NullObject()
        {
            object nullObj = null;

            Assert.Throws<ArgumentNullException>(() => nullObj.SetProperty("Test4", "salam"));
        }

        [Fact]
        public void ShouldThrow_InvalidOperationException_SetProperty_NotExistProperty()
        {
            var obj = new Testboject();

            Assert.Throws<InvalidOperationException>(() => obj.SetProperty("Test4", "salam"));
        }

    }
}
