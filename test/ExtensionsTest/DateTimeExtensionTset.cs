using Project.Base;
using System;
using System.Globalization;
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
        public void NewDateTime_input_ToPersianDateTime_Test()
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
        public void NewDateTime_input_ToFullStringPersianDateTime_Test()
        {
            var date = new DateTime(2018, 10, 09, 00, 00, 00);
            var result = date.ToFullStringPersianDateTime();
            var persianDayOfWeek = result.Split(' ')[0];
            Assert.True(persianDayOfWeek  == "سه");
        }

        [Fact]
        public void Should_Pass_ValidData_GetDayName_Test()
        {
            var persianDayWeek = (DayOfWeek.Thursday).GetDayName();
            Assert.True(persianDayWeek == "پنج شنبه");
        }

        [Fact]
        public void Should_Pass_ValidData_GetPersianYearAndMounth_Test()
        {
            var date = new DateTime(2019, 03, 21, 01, 00,00);
            var persianYear = date.GetPersianYear();
            var persianMonth = date.GetPersianMonth();
            Assert.True(persianYear == 1398);
            Assert.True(persianMonth == 01);
        }

        [Fact]
        public void ShouldPass_ValidInput_GetDateTimeAsDirectoryPath_Test()
        {
            var date = new DateTime(2018,10,13);
            var x = date.GetDateTimeAsDirectoryPath();
            Assert.True(date.GetDateTimeAsDirectoryPath() == "2018/10/13");
        }

        [Fact]
        public void ShouldPass_ValidInput_ToDateFromPersian_Test()
        {
            var x = "1397/7/23".ToDateFromPersian();
            Assert.True("1397/7/23".ToDateFromPersian() == new DateTime(2018, 10, 15));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ShouldException_NullInput_ToDateFromPersian_Test(string persianStringDate)
        {
            Assert.Throws<ArgumentNullException>(() =>persianStringDate.ToDateFromPersian());
        }

        [Fact]
        public void ToEpoch_Test()
        {
            var date = new DateTime(2018, 10, 17,13,56,00);
            Assert.True(date.ToEpoch() == 1539771960000);
        }

    }
}
