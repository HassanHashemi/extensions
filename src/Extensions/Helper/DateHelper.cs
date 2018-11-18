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

	public struct Period
	{
		public long Minutes { get; }
		public long Hours { get; }
		public int Days { get; }
		public int Weeks { get; }
		public int Months { get; }
		public int Years { get; }

		internal Period(int years, int months, int weeks, int days, long hours, long minutes)
		{
			this.Years = years;
			this.Months = months;
			this.Weeks = weeks;
			this.Days = days;
			this.Hours = hours;
			this.Minutes = minutes;
		}

		public static Period Difference(DateTime d1, DateTime d2)
		{
			int years, months, week, days, hours, minutes;

			if (d1 < d2)
			{
				var d3 = d2;
				d2 = d1;
				d1 = d3;
			}
			var span = d1 - d2;
			months = 12 * (d1.Year - d2.Year) + (d1.Month - d2.Month);
			if (d1.CompareTo(d2.AddMonths(months).AddMilliseconds(-500)) <= 0)
			{
				--months;
			}

			years = months / 12;
			months -= years * 12;

			if (months == 0 && years == 0)
			{
				days = span.Days;
				week = days / 7;
				days = days % 7;
			}
			else
			{
				var md1 = new DateTime(d1.Year, d1.Month, d1.Day);
				var md2 = new DateTime(d2.Year, d2.Month, d1.Day);
				var mDays = (int)(md1 - md2).TotalDays;

				if (mDays > span.Days)
				{
					mDays = (int)(md1.AddMonths(-1) - md2).TotalDays;
				}

				days = span.Days - mDays;
				week = days / 7;
				days = days % 7;
			}
			hours = span.Hours;
			minutes = span.Minutes;

			return new Period(years, months, week, days, hours, minutes);
		}
	}

	public class PeriodWrapper
	{
		private Period _period;
		public string Result { get; private set; }

		public PeriodWrapper(Period period)
		{
			_period = period;
		}

		public PeriodWrapper WithYear()
		{
			if (_period.Years > 0)
			{
				Result = $"{_period.Years} سال پیش";
			}

			return this;
		}

		public PeriodWrapper WithMonth()
		{
			if (_period.Years < 1 && this._period.Months > 0)
			{
				Result = $"{_period.Months} ماه پیش";
			}

			return this;
		}

		public PeriodWrapper WithWeek()
		{
			if (_period.Months < 1 && _period.Weeks > 0)
			{
				Result = $"{_period.Weeks} هفته پیش";
			}

			return this;
		}

		public PeriodWrapper WithDay()
		{
			if (_period.Weeks < 1 && _period.Days > 0)
			{
				Result = $"{_period.Days} روز پیش";
			}

			return this;
		}

		public PeriodWrapper WithHour()
		{
			if (_period.Days < 1 && _period.Hours > 0)
			{
				Result = $"{_period.Hours} ساعت پیش";
			}

			return this;
		}

		public PeriodWrapper WithMinutes()
		{
			if (_period.Hours < 1 && _period.Minutes > 0)
			{
				Result = $"{_period.Minutes} دقیقه پیش";
			}

			return this;
		}
	}

}
