using System;
using Xunit;
using Extensions;

namespace ExtensionsTest
{
    public class TextUnifierHelper
    {
        [Theory]
        [InlineData("اِنشاء نویسي یك 'هُنر' است! که اَلبتة مؤید کار سخت _نویسندگانـ می باشد.(آن %نوشته% ای مورد قبول است که *روان* باشد)")]
        public void Should_Pass_RemoveDiacriticsAndNotAlpha(string data)
        {
            Assert.True(data.RemoveDiacriticsAndNotAlpha() == "انشانویسيیكهنراستکهالبتةمؤیدکارسختنویسندگانمیباشدآننوشتهایموردقبولاستکهروانباشد");
        }

        [Theory]
        [InlineData("اِنشاء نویسي یك 'هُنر' است! که اَلبتة مؤید کار سخت _نویسندگانـ می باشد.(آن %نوشته% ای مورد قبول است که *روان* باشد)")]
        public void Should_Pass_ReplaceArabicCharacters(string data)
        {
            Assert.True(data.ReplaceArabicCharacters() == "اِنشاء نویسی یک 'هُنر' است! که اَلبته موید کار سخت _نویسندگانـ می باشد.(ان %نوشته% ای مورد قبول است که *روان* باشد)");
        }

        [Theory]
        [InlineData("اِنشاء نویسي یك 'هُنر' است! که اَلبتة مؤید کار سخت _نویسندگانـ می باشد.(آن %نوشته% ای مورد قبول است که *روان* باشد)")]
        public void Should_Pass_ReplaceArabicCharacters_ReplaceArabicCharacters(string data)
        {
            Assert.True(data.ReplaceArabicCharacters().RemoveDiacriticsAndNotAlpha() == "انشانویسییکهنراستکهالبتهمویدکارسختنویسندگانمیباشداننوشتهایموردقبولاستکهروانباشد");
        }
    }
}
