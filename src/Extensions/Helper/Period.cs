using System;

namespace Extensions.Helper
{
	internal struct Period
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

		public static Period Difference(DateTime first, DateTime second)
		{
			if (first < second)
			{
				throw new ArgumentException("First must be bigger than second.");
			}

			int years, months, week, days, hours, minutes;

			var span = first - second;
			months = 12 * (first.Year - second.Year) + (first.Month - second.Month);
			//-500 : برای تاریخ یکسان کاربرد دارد
			if (first.CompareTo(second.AddMonths(months).AddMilliseconds(-500)) <= 0)
			{
				months--;
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
				var mfirst = new DateTime(first.Year, first.Month, first.Day);
				var msecond = new DateTime(second.Year, second.Month, first.Day);
				var mDays = (int)(mfirst - msecond).TotalDays;

				if (mDays > span.Days)
				{
					mDays = (int)(mfirst.AddMonths(-1) - msecond).TotalDays;
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
}
