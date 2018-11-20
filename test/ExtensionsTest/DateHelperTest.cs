using System;
using Xunit;
namespace ExtensionsTest
{
	public class DateHelperTest
	{
		[Fact]
		public void DifferenceYearTest()
		{
			var date1 = new DateTime(2008, 10, 12, 15, 32, 44, 243);
			var date2 = new DateTime(2010, 1, 3, 23, 22, 9, 345);

			var difference = Extensions.Helper.DateHelper.ElapsedTime(date2, date1);

			Assert.True(difference == "1 سال پیش");
		}

		[Fact]
		public void DifferenceMonthTest()
		{
			var date1 = new DateTime(2018, 10, 1, 15, 32, 44, 243);
			var date2 = new DateTime(2018, 12, 1, 15, 32, 44, 243);

			var difference = Extensions.Helper.DateHelper.ElapsedTime(date2, date1);

			Assert.True(difference == "2 ماه پیش");
		}

		[Fact]
		public void DifferenceWeekTest()
		{
			var date1 = new DateTime(2018, 10, 1, 15, 32, 44, 243);
			var date2 = new DateTime(2018, 10, 13, 15, 32, 44, 243);

			var difference = Extensions.Helper.DateHelper.ElapsedTime(date2, date1);

			Assert.True(difference == "1 هفته پیش");
		}

		[Fact]
		public void DifferenceDayTest()
		{
			var date1 = new DateTime(2018, 10, 13, 15, 32, 44, 243);
			var date2 = new DateTime(2018, 10, 19, 15, 32, 44, 243);

			var difference = Extensions.Helper.DateHelper.ElapsedTime(date2, date1);

			Assert.True(difference == "6 روز پیش");
		}

		[Fact]
		public void DifferenceHourTest()
		{
			var date1 = new DateTime(2018, 10, 13, 15, 32, 44, 243);
			var date2 = new DateTime(2018, 10, 13, 20, 32, 44, 243);

			var difference = Extensions.Helper.DateHelper.ElapsedTime(date2, date1);

			Assert.True(difference == "5 ساعت پیش");
		}

		[Fact]
		public void DifferenceMinuteTest()
		{
			var date1 = new DateTime(2018, 10, 13, 20, 2, 44, 243);
			var date2 = new DateTime(2018, 10, 13, 20, 52, 44, 243);

			var difference = Extensions.Helper.DateHelper.ElapsedTime(date2, date1);

			Assert.True(difference == "50 دقیقه پیش");
		}

		[Fact]
		public void DifferenceThrowsTest()
		{
			var date1 = new DateTime(2018, 10, 13, 20, 2, 44, 243);
			var date2 = new DateTime(2017, 10, 13, 20, 52, 44, 243);

			Assert.Throws<ArgumentException>(() => Extensions.Helper.DateHelper.ElapsedTime(date2, date1));
		}
	}
}
