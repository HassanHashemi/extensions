using System;
using System.ComponentModel;
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

    public enum DatePostfix
    {
        [Description("ثانیه")]
        Seconds,

        [Description("دقیقه")]
        Minute,

        [Description("ساعت")]
        Hours,

        [Description("روز")]
        Days,

        [Description("هفته")]
        Week,

        [Description("ماه")]
        Month,

        [Description("سال")]
        Year
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
            return $"{date.DayOfWeek.GetDayName()} {date.ToPersianDate()} {(date.Hour > 0 || date.Minute > 0 ? " ساعت " + date.ToString("HH:mm") : "")}";
        }

        public static string ToFullStringPersianDate(this DateTime date)
        {
            var persian = ToPersian(date);
            return $"{date.DayOfWeek.GetDayName()} {persian.Day} {GetMonthName(persian.Month)} {persian.Year}";
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
        public static string GetMonthName(this int Month)
        {
            switch (Month)
            {
                case 1: return "فروردین";
                case 2: return "اردیبهشت";
                case 3: return "خرداد";
                case 4: return "تیر";
                case 5: return "مرداد";
                case 6: return "شهریور";
                case 7: return "مهر";
                case 8: return "آبان";
                case 9: return "آذر";
                case 10: return "دی";
                case 11: return "بهمن";
                case 12: return "اسفند";
                default: return string.Empty;
            }
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

        public static DateTime ToDateTimeFromPersian(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException();
            }

            value = Regex.Replace(value, @".*?([0-9]{4}/[0-9]{2}/[0-9]{2})\s*([0-9]{2}:[0-9]{2}).*", "$1 $2");
            var date = value.Split(' ')[0].Split('/').Select(d => int.Parse(d)).ToArray();
            var time = value.Split(' ')[1].Split(':').Select(d => int.Parse(d)).ToArray();
            return new PersianCalendar().ToDateTime(date[0], date[1], date[2], time[0], time[1], 0, 0);
        }

        public static string ToDurationSeo(this int source) => ToDurationSeo((double)source);

        public static string ToDurationSeo(this double source)
        {
            var span = TimeSpan.FromSeconds(source);

            var formatted = string.Format("{0}{1}{2}",
                span.Duration().Hours > 0 ? string.Format("{0:D2}:", span.Hours) : string.Empty,
                span.Duration().Minutes > 0 ? string.Format("{0:D2}:", span.Minutes) : "00:",
                span.Duration().Seconds > 0 ? string.Format("{0:D2}", span.Seconds) : "00:");

            if (formatted.EndsWith(":"))
            {
                formatted = formatted.Substring(0, formatted.Length - 1);
            }

            return formatted;
        }

        public static string GetTimeAgoString(this DateTime first)
        {
            return DifferenceString(first, DateTime.Now);
        }

        public static string DifferenceString(DateTime first, DateTime second)
        {
            (int value, DatePostfix prefix) = Difference(first, second);

            return string.Concat(value, " ", prefix.GetEnumDescription(), " ", "پیش");
        }

        public static (int value, DatePostfix prefix) Difference(DateTime first, DateTime second)
        {
            var prefix = default(DatePostfix);
            var value = default(int);
            var diff = second - first;

            if (diff.TotalHours <= 1)
            {
                if (diff.TotalMinutes == 0)
                {
                    value = (int)diff.TotalSeconds;
                    prefix = DatePostfix.Seconds;
                }
                else
                {
                    value = (int)diff.TotalMinutes;
                    prefix = DatePostfix.Minute;
                }
            }
            else if (diff.TotalDays <= 1)
            {
                value = (int)diff.TotalHours;
                prefix = DatePostfix.Hours;
            }
            else if (diff.TotalDays < 7 && diff.TotalDays > 1)
            {
                value = (int)diff.TotalDays;
                prefix = DatePostfix.Days;
            }
            else if (diff.TotalDays <= 30 && diff.TotalDays >= 7)
            {
                value = (int)diff.TotalDays / 7;
                prefix = DatePostfix.Week;
            }
            else if (diff.TotalDays < 365 && diff.TotalDays >= 30)
            {
                value = (int)diff.TotalDays / 30;
                prefix = DatePostfix.Month;
            }
            else
            {
                value = (int)diff.TotalDays / 356;
                prefix = DatePostfix.Year;
            }

            return (value, prefix);
        }
    }
}
