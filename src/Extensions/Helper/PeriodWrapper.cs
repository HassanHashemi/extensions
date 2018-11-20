namespace Extensions.Helper
{
	internal sealed class PeriodWrapper
	{
		internal PeriodWrapper(Period period)
		{
			_period = period;
		}

		public string Result { get; private set; }

		private readonly Period _period;

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
