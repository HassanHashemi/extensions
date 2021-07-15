//using Extensions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Xunit;

//namespace ExtensionsTest
//{
//    public class HashUtilityTest
//    {
//        [Theory]
//        public void Should_Pass_TagExteraction(string data)
//        {
//            var tagList = HashtagUtility.TagExteraction(data).ToList();
//            Assert.True(tagList[2] == "دولت_عربستان");
//            Assert.False(tagList[3] == "عرب ها");
//            Assert.True(tagList[4] == "وهابیت");
//        }
//    }
//}
