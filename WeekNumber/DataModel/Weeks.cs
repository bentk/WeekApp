using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.System.UserProfile;

namespace WeekNumber.Data
{
    [Windows.Foundation.Metadata.WebHostHidden]
    public class BindableWeek : Common.BindableBase
    {
        public int WeekNumber { get; private set; }
        public int Year { get; private set; }
        public bool IsCurrentWeek { get
        {
            var culture = GlobalizationPreferences.Languages[0] + "-" + GlobalizationPreferences.HomeGeographicRegion;
            return WeekNumber == new WeekCalendar.Week(culture).GetWeekNumberFromDate(DateTime.Today);
        } }
        public BindableWeek(DateTime day)
        {
            var culture = GlobalizationPreferences.Languages[0] + "-" + GlobalizationPreferences.HomeGeographicRegion;
            var s = new WeekCalendar.Week(culture);
            WeekNumber = s.GetWeekNumberFromDate(day);
            Days = s.GetDaysInCurrentWeek(day).Select(d=> new BindableDay(d)).ToList();
            Year = day.Year;
        }

        public List<BindableDay> Days { get; private set; }
    }

    public class BindableDay : Common.BindableBase
    {
        public bool IsToday { get { return DateTime.Today.CompareTo(_dateTime) == 0; } }
        public string Name{ get
        {
            var culture = GlobalizationPreferences.Languages[0] + "-" + GlobalizationPreferences.HomeGeographicRegion;
            var dateTimeFormat = new CultureInfo(culture).DateTimeFormat;
            return dateTimeFormat.Calendar.GetDayOfWeek(_dateTime).ToString();
        }
        }

        public string DayNumber
        {
            get { return _dateTime.Day.ToString(); }
        }

        private DateTime _dateTime;
        public BindableDay(DateTime dateTime)
        {
            _dateTime = dateTime;
        }
        //public string NameOfDay
        //{
        //    return 
        //}
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<BindableWeek> _allWeeks = new ObservableCollection<BindableWeek>();

        public ObservableCollection<BindableWeek> AllWeeks
        {
            get { return _allWeeks; }
        }

        //public static int currentWeek
        public static IEnumerable<BindableWeek> GetAllWeeks(int year)
        {
            return _sampleDataSource.AllWeeks.Where(w=>w.Year == year);
        }

        public static BindableWeek GetWeek(int weekNumber)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllWeeks.Where(week => week.WeekNumber.Equals(weekNumber));
            var enumerable = matches as BindableWeek[] ?? matches.ToArray();
            if (enumerable.Count() == 1)
                return enumerable.First();
            return null;
        }

        public static BindableWeek GetItem(string uniqueId)
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
                AllWeeks.Add(new BindableWeek(startDate));
                startDate = startDate.AddDays(7);
            }
        }
        private static BindableWeek _thisBindableWeek = new BindableWeek(DateTime.Today);
        public static BindableWeek ThisBindableWeek{ get { return _thisBindableWeek; }

        }
    }
}
