using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Extensions
{
    public class PersianDateWrapper
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int? Hours { get; set; }
        public int? Minutes { get; set; }
        public int? Seconds { get; set; }

        public string Full => $"{this.Year}/{this.Month}/{this.Day}";
    }

    public static class DateTimeExtensions
    {
        public static PersianDateWrapper ToPersian(this DateTime source)
        {
            var calendar = new PersianCalendar();
            return new PersianDateWrapper()
            {
                Year = calendar.GetYear(source),
                Month = calendar.GetMonth(source),
                Day = calendar.GetDayOfMonth(source),
                Hours = calendar.GetHour(source),
                Minutes = calendar.GetMinute(source),
                Seconds = calendar.GetSecond(source),
            };
        }

        public static string ToPersianDate(this DateTime date)
        {
            var persian = ToPersian(date);
            return string.Format("{0}/{1}/{2}",
                persian.Year,
                persian.Month.ToString().PadLeft(2, '0'),
                persian.Day.ToString().PadLeft(2, '0'));
        }

        public static string ToPersianDateTime(this DateTime date)
        {
            var persian = ToPersian(date);
            return string.Format("{0}/{1}/{2}-{3}:{4}",
                persian.Year,
                persian.Month.ToString().PadLeft(2, '0'),
                persian.Day.ToString().PadLeft(2, '0'),
                persian.Hours.ToString().PadLeft(2, '0'),
                persian.Minutes.ToString().PadLeft(2, '0'));
        }

        public static string ToFullStringPersianDateTime(this DateTime date)
        {
            var persian = ToPersian(date);
            return $"{date.DayOfWeek.GetDayName()} {date.ToPersianDate()} {( date.Hour > 0 || date.Minute > 0 ? " ساعت " + date.ToString("HH:mm") : "" )}";
        }

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
        {
            return ToPersian(date).Year;
        }

        public static int GetPersianMonth(this DateTime date)
        {
            return ToPersian(date).Month;
        }

        public static string GetDateTimeAsDirectoryPath(this DateTime date)
        {
            return $"{date.Year}/{date.Month}/{ date.Day}";
        }

        public static DateTime ToDateFromPersian(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException();
            }

            value = Regex.Replace(value, ".*?([0-9]{4}/[0-9]{2}/[0-9]{2}).*", "$1");
            var dateParts = value.Split('/').Select(d => int.Parse(d)).ToArray();
            return new DateTime(dateParts[0], dateParts[1], dateParts[2], new PersianCalendar());
        }

        public static double ToEpoch(this DateTime date)
        {
            return date.ToUniversalTime()
                .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds;
        }
    }
}
