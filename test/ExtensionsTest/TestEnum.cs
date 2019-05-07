using System.ComponentModel;
using System;

namespace ExtensionsTest
{
    public class TestAttribute : Attribute
    {
        public string Value { get; set; }
    }
    
    public enum TestEnum
    {
        [Description("my name")]
        Mahdi = 0,

        [Description("Hashemi's name")]
        Hassan = 1,

        [Test(Value = "Hassan Hsahemi")]
        Hosein = 2
    }
}
