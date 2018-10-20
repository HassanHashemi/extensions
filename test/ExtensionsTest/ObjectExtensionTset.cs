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
        public void HasProperty_Null_And_ValidInput_Test()
        {
            var obj = new Testboject();
            Assert.False(obj.HasProperty("Test"));
            Assert.True(obj.HasProperty("Test3"));
            object NullObj = null;
            Assert.Throws<ArgumentNullException>(() => NullObj.HasProperty("Test"));
        }

        [Fact]
        public void GetPropertyValue_Null_And_ValidInput_Test ()
        {
            var obj = new Testboject()
            {
                Test1 = "salam",
                Test2 = 100
            };
            Assert.True((string)obj.GetPropertyValue("Test1") == "salam");
            object NullObj = null;
            Assert.Throws<ArgumentNullException>(() => NullObj.GetPropertyValue("Test1"));
            Assert.Throws<ArgumentNullException>(() => obj.GetPropertyValue("Test4"));
        }

        [Fact]
        public void SetProperty_Null_And_ValidInput_Test()
        {
            var obj = new Testboject();
            obj.SetProperty("Test1", "salam");
            Assert.True(obj.HasProperty("Test3"));
            Assert.False(obj.HasProperty("Test"));
            Assert.True((string)obj.GetPropertyValue("Test1") == "salam");
            object NullObj = null;
            Assert.Throws<InvalidOperationException>(() => obj.SetProperty("Test4", "salam"));
            Assert.Throws<ArgumentNullException>(() => NullObj.SetProperty("Test4", "salam"));
        }
    }
}
