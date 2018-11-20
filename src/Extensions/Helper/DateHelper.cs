using System;

namespace Extensions.Helper
{
	public static class DateHelper
	{
		public static string ElapsedTime(DateTime startDate, DateTime endDate)
		{
			var period = Period.Difference(startDate, endDate);

			return new PeriodWrapper(period)
				.WithYear()
				.WithMonth()
				.WithWeek()
				.WithDay()
				.WithHour()
				.WithMinutes()
				.Result;
		}
	}
}
