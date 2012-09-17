﻿using System;
using System.Globalization;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace WeekCalendarTest
{
    [TestClass]
    public class CalendarTests
    {
        [TestMethod]
        public void GetWeekNumberFromDate20120101()
        {
            var weekNo = new WeekCalendar.WeekCalendar().GetWeekNumberFromDate(new DateTime(2011, 1, 1));
            Assert.AreEqual(1, weekNo);
        }

        [TestMethod]
        public void GetWeekNumberFromDate20121231()
        {
            var weekNumber = new WeekCalendar.WeekCalendar().GetWeekNumberFromDate(new DateTime(2012, 12, 31));
            Assert.AreEqual(53, weekNumber);
        }

        [TestMethod]
        public void GetWeekNumberFromDate20120229()
        {
            var weekNumber = new WeekCalendar.WeekCalendar().GetWeekNumberFromDate(new DateTime(2012, 02, 29));
            Assert.AreEqual(9, weekNumber);
        }

        [TestMethod]
        public void GetDaysInCurrentWeekFromDate20120912()
        {
            var daysInCurrentWeek = new WeekCalendar.WeekCalendar().GetDaysInCurrentWeek(new DateTime(2012, 9, 12));
            Assert.AreEqual(DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek, daysInCurrentWeek[0].DayOfWeek);
        }

    }
}
