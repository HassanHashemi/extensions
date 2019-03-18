using System;
using Xunit;

namespace ExtensionsTest
{
    public class GuidGenereatorTest
    {
        [Fact]
        public void ShouldGenerateGuid()
        {
            var generator1 = new GuidGenerator();
            var generator2 = new GuidGenerator();
            var first = generator1.Next();
            var second = generator2.Next();
            Assert.True(first != default(Guid));
            Assert.True(second != default(Guid));
            Assert.True(first != second);
        }
    }
}
