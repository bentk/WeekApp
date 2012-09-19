using System;
using System.Collections.Generic;
using System.Globalization;

namespace WeekCalendar
{
    public class Week
    {
        private DateTimeFormatInfo _dateTimeFormat;

        public Week(string culture)
        {   
            _dateTimeFormat = new CultureInfo(culture).DateTimeFormat;
        }


        public Week(DateTimeFormatInfo dateTimeFormat)
        {
            _dateTimeFormat = dateTimeFormat;
        }

        public int GetWeekNumberFromDate(DateTime dateTime)
        {
            
            return _dateTimeFormat.Calendar.GetWeekOfYear(dateTime, _dateTimeFormat.CalendarWeekRule, _dateTimeFormat.FirstDayOfWeek);
        }

        public List<DateTime> GetDaysInCurrentWeek(DateTime dateTime)
        {
            var dayOfWeek = _dateTimeFormat.Calendar.GetDayOfWeek(dateTime);
            var firstDayOfThisWeekDate = dateTime;
            while (dayOfWeek.CompareTo(_dateTimeFormat.FirstDayOfWeek) != 0)
            {
                firstDayOfThisWeekDate = firstDayOfThisWeekDate.AddDays(-1);
                dayOfWeek = _dateTimeFormat.Calendar.GetDayOfWeek(firstDayOfThisWeekDate);
            }
            
            var returnValues = new List<DateTime>(7);
            for (var i = 0; i < 7; i++)
            {
                returnValues.Add(firstDayOfThisWeekDate.AddDays(i));
            }

            
            return returnValues;
        }


        public string DayAndMonthStringFromDate(DateTime today)
        {
            return today.ToString(_dateTimeFormat.MonthDayPattern, _dateTimeFormat);
        }

        public string GetYearMonthAndDayFormatted(DateTime today)
        {
            return today.ToString("D", _dateTimeFormat);
        }
    }
}
