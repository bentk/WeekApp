using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WeekNumber.Data
{
    [Windows.Foundation.Metadata.WebHostHidden]
    public class Week : Common.BindableBase
    {
        public int WeekNumber { get; private set; }
        public int Year { get; private set; }

        public Week(DateTime day)
        {
            var s = new WeekCalendar.WeekCalendar();
            WeekNumber = s.GetWeekNumberFromDate(day);
            Days = s.GetDaysInCurrentWeek(day);
            Year = day.Year;
        }

        public List<DateTime> Days { get; private set; }
    }


    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<Week> _allWeeks = new ObservableCollection<Week>();

        public ObservableCollection<Week> AllWeeks
        {
            get { return this._allWeeks; }
        }

        //public static int currentWeek
        public static IEnumerable<Week> GetAllWeeks(int year)
        {
            return _sampleDataSource.AllWeeks.Where(w=>w.Year == year);
        }

        public static Week GetWeek(int weekNumber)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllWeeks.Where(week => week.WeekNumber.Equals(weekNumber));
            var enumerable = matches as Week[] ?? matches.ToArray();
            if (enumerable.Count() == 1)
                return enumerable.First();
            return null;
        }

        public static Week GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            //  var matches = _sampleDataSource.AllWeeks.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            //            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
            var startDate = new DateTime(DateTime.Today.Year, 1, 1);
            while (startDate.Year == DateTime.Today.Year)
            {
                AllWeeks.Add(new Week(startDate));
                startDate = startDate.AddDays(7);
            }
        }
        private static Week _thisWeek = new Week(DateTime.Today);
        public static Week ThisWeek{ get { return _thisWeek; }

        }
    }
}
