using Extensions.HashtagUtility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ExtensionsTest
{
    public class HashUtilityTest
    {
        [Theory]
        [InlineData("در جریان #قتل #خاشقچی #دولت_عربستان  مقصر شناخته شده است! اما این #عرب ها زیر بار نمی روند البته این مهم نیست چون #وهابیت=انگلیسی هست!")]
        public void Should_Pass_TagExteraction(string data)
        {

            var tagList = HashtagUtility.TagExteraction(data);
            Assert.True(tagList[2] == "#دولت_عربستان");
            Assert.False(tagList[3] == "#عرب ها");
            Assert.True(tagList[4] == "#وهابیت");
        }
    }
}
