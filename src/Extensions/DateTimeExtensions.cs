using System;
using System.Text;
using System.Globalization;

namespace Project.Base
{
    public static class DateTimeExtensions
    {
        public static string ToPersianDate(this DateTime date)
        {
            var calendar = new PersianCalendar();
            return string.Format("{0}/{1}/{2}",
                calendar.GetYear(date),
                calendar.GetMonth(date).ToString().PadLeft(2, '0'),
                calendar.GetDayOfMonth(date).ToString().PadLeft(2, '0'));
        }

        public static string ToPersianDateTime(this DateTime date)
        {
            var calendar = new PersianCalendar();
            return string.Format("{0}/{1}/{2}-{3}:{4}",
                calendar.GetYear(date),
                calendar.GetMonth(date).ToString().PadLeft(2, '0'),
                calendar.GetDayOfMonth(date).ToString().PadLeft(2, '0'),
                date.Hour.ToString().PadLeft(2, '0'),
                date.Minute.ToString().PadLeft(2, '0'));
        }

        public static string ToFullStringPersianDateTime(this DateTime date)
            => GlobalsCommon.AS(new PersianCalendar().GetDayOfWeek(date).GetDayName(),
                " ",
                date.ToPersianDate(),
                (date.Hour > 0 || date.Minute > 0 ? " ساعت " + date.ToString("HH:mm") : ""));

        public static string GetDayName(this DayOfWeek day)
        {
            if (day == DayOfWeek.Friday)
            {
                return "جمعه";
            }
            if (day == DayOfWeek.Monday)
            {
                return "دوشنبه";
            }
            if (day == DayOfWeek.Saturday)
            {
                return "شنبه";
            }
            if (day == DayOfWeek.Sunday)
            {
                return "يکشنبه";
            }
            if (day == DayOfWeek.Thursday)
            {
                return "پنج شنبه";
            }
            if (day == DayOfWeek.Tuesday)
            {
                return "سه شنبه";
            }
            if (day == DayOfWeek.Wednesday)
            {
                return "چهارشنبه";
            }
            return string.Empty;

        }

        public static int GetPersianYear(this DateTime date)
            => new PersianCalendar().GetYear(date);

        public static int GetPersianMonth(this DateTime date)
            => new PersianCalendar().GetMonth(date);
    }
}
