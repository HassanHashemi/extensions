using Extensions;
using System;
using System.Globalization;
using System.Linq;
using Xunit;

namespace ExtensionsTest
{
    public class DateTimeExtensionTset
    {
        [Fact]
        public void Should_Pass_ForValidInput()
        {
            var date = new DateTime(2019, 03, 20);
            var result = date.ToPersianDate();
            var items = result.Split("/");
            var year = items[0];
            var month = items[1];
            var day = items[2];

            Assert.True(int.Parse(year) == 1397);
            Assert.True(int.Parse(month) == 12);
            Assert.True(int.Parse(day) == 29);
        }


        [Fact]
        public void ShouldPass_ToPersianDateTime_NewDateTimeInput()
        {
            var date = new DateTime(2018, 10, 09,05,20,30);
            var result = date.ToPersianDateTime();
            var items = result.Split("/");
            var year = items[0];
            var month = items[1];
            var Time = items[2].Split('-')[1];
            var timeDetails = Time.Split(':');
            var hour = timeDetails[0];
            var minute = timeDetails[1];

            Assert.True(int.Parse(year) == 1397);
            Assert.True(int.Parse(minute) == 20);

        }

        [Fact]
        public void ShouldPass_ToFullStringPersianDateTime_NewDateTimeInput()
        {
            var date = new DateTime(2018, 10, 09, 00, 00, 00);
            var result = date.ToFullStringPersianDateTime();
            var persianDayOfWeek = result.Split(' ')[0];

            Assert.True(persianDayOfWeek == "سه");
        }

        [Fact]
        public void ShouldPass_ToFullStringPersianDate()
        {
            var date = new DateTime(2018, 10, 09, 00, 00, 00);
            var result = date.ToFullStringPersianDate();
            var persianDayOfWeek = result.Split(' ');

            Assert.True(persianDayOfWeek[3] == "مهر");
            Assert.True(int.TryParse(persianDayOfWeek.Last(),out _));
        }

        [Fact]
        public void ShouldPass_GetDayName_ValidData()
        {
            var persianDayWeek = (DayOfWeek.Thursday).GetDayName();

            Assert.True(persianDayWeek == "پنج شنبه");
        }

        [Fact]
        public void ShouldPass_GetPersianYearAndMounth_ValidData()
        {
            var date = new DateTime(2019, 03, 21, 01, 00,00);
            var persianYear = date.GetPersianYear();
            var persianMonth = date.GetPersianMonth();

            Assert.True(persianYear == 1398);
            Assert.True(persianMonth == 01);
        }

        [Fact]
        public void ShouldPass_GetDateTimeAsDirectoryPath_ValidInput()
        {
            var date = new DateTime(2018,10,13);

            Assert.True(date.GetDateTimeAsDirectoryPath() == "2018/10/13");
        }

        [Fact]
        public void ShouldPass_ToDateFromPersian_ValidInput()
        {
            var converted = "1397/7/23".ToDateFromPersian();

            Assert.True(converted == new DateTime(2018, 10, 15));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldThrow_ToDateFromPersian_NullInput(string persianStringDate)
        {
            Assert.Throws<ArgumentNullException>(() =>persianStringDate.ToDateFromPersian());
        }

        [Fact]
        public void ToEpoch()
        {
            var date = new DateTime(2018, 10, 17,13,56,00);

            Assert.True(date.ToEpoch() == 1539771960000);
        }

        [Fact]
        public void Should_Pass_ToDateTimeFromPersian()
        {
            var persianDateTime = "1397/11/01 10:46";

            Assert.True(persianDateTime.ToDateTimeFromPersian() == new DateTime(2019, 01, 21, 10, 46, 0, 0));
        }
    }
}
