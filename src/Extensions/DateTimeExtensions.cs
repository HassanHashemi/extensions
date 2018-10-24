using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Project.Base
{
    public class PersianDateWrapper
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        public string Full => $"{this.Year}/{this.Month}/{this.Day}";
    }

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
            return $"{new PersianCalendar().GetDayOfWeek(date).GetDayName()} {date.ToPersianDate()} {( date.Hour > 0 || date.Minute > 0 ? " ساعت " + date.ToString("HH:mm") : "" )}";
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
            => new PersianCalendar().GetYear(date);

        public static int GetPersianMonth(this DateTime date)
            => new PersianCalendar().GetMonth(date);

        public static string GetDateTimeAsDirectoryPath(this DateTime date)
            => $"{date.Year}/{date.Month}/{ date.Day}";

        public static string ToSqlDate(this string date)
        {
            throw new NotImplementedException();
            //string result = string.Empty;
            //if (date.Length > 0)
            //{
            //    var dd = "";
            //    var mm = "";
            //    var yyyy = "";
            //    var buf = date;
            //    yyyy = buf.Substring(0, buf.IndexOf("-", 1, StringComparison.Ordinal));
            //    if (yyyy.Length != 4)
            //    {
            //        return string.Empty;
            //    }
            //    buf = buf.Substring(buf.IndexOf("-", 1, StringComparison.Ordinal) + 1, buf.Length - (buf.IndexOf("-", 1, StringComparison.Ordinal) + 1));
            //    mm = buf.Substring(0, buf.IndexOf("-", 1, StringComparison.Ordinal));
            //    if (mm.Length > 2 || mm.Length == 0)
            //    {
            //        return string.Empty;
            //    }
            //    else if (mm.ToInt(0) > 12 && mm.ToInt(0) < 0)
            //    {
            //        throw new ArgumentOutOfRangeException();
            //    }
            //    buf = buf.Substring(buf.IndexOf("-", 1, StringComparison.Ordinal) + 1, buf.Length - (buf.IndexOf("-", 1, StringComparison.Ordinal) + 1));
            //    dd = buf;
            //    if (mm.Length > 2 || mm.Length == 0)
            //    {
            //        return string.Empty;
            //    }
            //    else if (dd.ToInt(0) > 31 && dd.ToInt(0) < 0)
            //    {
            //        throw new ArgumentOutOfRangeException();
            //    }
            //    result = yyyy + "/" + mm + "/" + dd;
            //}
            //return result;
        }

        public static DateTime SqlDateToDate(this string sqlDate, DateTime defaultDateTime)
        {
            throw new NotImplementedException();
            //try
            //{
            //    return DateTime.Parse(sqlDate);
            //}
            //catch
            //{
            //    return defaultDateTime;
            //}
        }

        public static DateTime ToDate(this string date, DateTime defaultDateTime)
        {
            throw new NotImplementedException();
            //try
            //{
            //    return DateTime.Parse(date.ToSqlDate());
            //}
            //catch
            //{
            //    return defaultDateTime;
            //}
        }
        public static DateTime ToDateFromPersian(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException();
            }

            value = Regex.Replace(value, ".*?([0-9]{4}/[0-9]{2}/[0-9]{2}).*", "$1");
            var dateParts = value.Split('/').Select(d => int.Parse(d)).ToArray();
            DateTime date = new DateTime(dateParts[0], dateParts[1], dateParts[2], new PersianCalendar());
            return date;
        }

        public static double ToEpoch(this DateTime date)
        {
            return date.ToUniversalTime()
                .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds;
        }
    }
}
