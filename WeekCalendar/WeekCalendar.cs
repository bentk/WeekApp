using System;
using System.Collections.Generic;
using System.Globalization;

namespace WeekCalendar
{
    public class WeekCalendar
    {   
        public int GetWeekNumberFromDate(DateTime dateTime)
        {
            return DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(dateTime, DateTimeFormatInfo.CurrentInfo.CalendarWeekRule,     DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek);
        }

        public List<DateTime> GetDaysInCurrentWeek(DateTime dateTime)
        {
            var dayOfWeek = DateTimeFormatInfo.CurrentInfo.Calendar.GetDayOfWeek(dateTime);// DayOfWeek(dateTime).GetWeekOfYear(dateTime, DateTimeFormatInfo.CurrentInfo.CalendarWeekRule, DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek);
            var firstDayOfThisWeekDate = dateTime;
            while (dayOfWeek.CompareTo(DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek) != 0)
            {
                firstDayOfThisWeekDate = firstDayOfThisWeekDate.AddDays(-1);
                dayOfWeek = DateTimeFormatInfo.CurrentInfo.Calendar.GetDayOfWeek(firstDayOfThisWeekDate);
            }

            var returnValues = new List<DateTime>(7);
            for (var i = 0; i < 7; i++)
            {
                returnValues.Add(firstDayOfThisWeekDate.AddDays(i));
            }

            
            return returnValues;
        }

        
    }
}
