using System;
using Xunit;
using Extensions;

namespace ExtensionsTest
{
    public class StringExtensionTest
    {
        [Theory]
        [InlineData("1 апреля")]
        public void ShouldPass_ForNoneASCII(string input)
        {
            Assert.True(!input.IsASCII());
        }

        [Theory]
        [InlineData( "سلام !# بر همه <شما دوستان>")]
        public void ShouldPass_ToUrlTitle(string url)
        {
            Assert.True(url.ToUrlTitle() == "سلام-بر-همه-شما-دوستان-");
        }

        [Theory]
        [InlineData("sallary")]
        [InlineData("Sallary")]
        public void ShouldPass_UpperFirstCharacter(string word)
        {
            Assert.True(word.UpperFirstCharacter() == "Sallary");   
        }

        [Theory]
        [InlineData("Sallary")]
        public void ReverseString(string word)
        {
            Assert.True(word.ReverseString() == "yrallaS");
            Assert.False(word.ReverseString() == "yrallas");
        }

        [Theory]
        [InlineData("salam")]
        public void ToByteArray(string word)
        {
            var byted = word.ToByteArray();

            Assert.True(byted[0] == 115);
        }

        [Theory]
        [InlineData("salam")]
        public void Left(string word)
        {
            var lefted = word.Left(2, false);

            Assert.True(lefted == "sa");
        }

        [Theory]
        [InlineData("how are you?")]
        public void Left_Complete(string word)
        {
            var lefted = word.Left(5, true);

            Assert.True(lefted == "how");
        }

        [Theory]
        [InlineData("salam")]
        public void ShouldThrow_OutOfRangeException_Left(string word)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => word.Left(5, true));
        }

        [Theory]
        [InlineData("salam")]
        public void Right(string word)
        {
            var lefted = word.Right(2);

            Assert.True(lefted == "am");
        }

        [Theory]
        [InlineData("salam")]
        public void ShouldThrow_FormatException_ToIntArray(string word)
        {
            Assert.Throws<FormatException>(() => word.ToIntArray());
        }

        [Theory]
        [InlineData("12,13")]
        public void ShouldPass_ToIntArray(string word)
        {
            Assert.True(word.ToIntArray()[0] == 12);
        }

        [Theory]
        [InlineData("~6s&^dg+=")]
        public void ShouldPass_UrlGuidEncode(string word)
        {
            Assert.True(word.RemoveSpecialCharacters("-") == "-6s-dg-");
        }
    }
}
