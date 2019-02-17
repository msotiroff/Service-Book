using System;

namespace ServiceBook.Extensions
{
    public static class DateTimeExtensions
    {
        private const string DateFormat = "yyyy-MM-dd";

        public static string ToSimplifiedDateString(this DateTime dateTime)
        {
            return dateTime.ToString(DateFormat);
        }
    }
}
