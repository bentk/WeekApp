using System;
using System.Globalization;
using System.Linq;
using System.Collections.ObjectModel;
using WeekCalendar;
using Windows.System.UserProfile;

namespace WeekNumber.Data
{
    [Windows.Foundation.Metadata.WebHostHidden]
    public class BindableWeek : Common.BindableBase
    {
        public int WeekNumber { get; private set; }
        public int Year { get; private set; }
        public int SelectedWeek { get; set; }
        public bool IsSelectedWeek { get { return WeekNumber == SelectedWeek; } }

        public bool IsCurrentWeek
        {
            get
            {
                //var culture = GlobalizationPreferences.Languages[0] + "-" + GlobalizationPreferences.HomeGeographicRegion;
                //int weekNumber;
                //try
                //{
                //    weekNumber = new Week(culture).GetWeekNumberFromDate(DateTime.Today);   
                //}
                //catch
                //{

                //    weekNumber = new Week(CultureInfo.CurrentCulture.ToString()).GetWeekNumberFromDate(DateTime.Today);
                //}
                return WeekNumber == new Week(GlobalizationPreferences.Languages[0]).GetWeekNumberFromDate(DateTime.Today);
            }
        }

        public BindableWeek(DateTime day)
        {

            var s = new Week(GlobalizationPreferences.Languages[0]);
            WeekNumber = s.GetWeekNumberFromDate(day);
            Days = new ObservableCollection<BindableDay>(s.GetDaysInCurrentWeek(day).Select(d=> new BindableDay(d)).ToList());
            Year = day.Year;
            MonthName = s.GetMonthString(day);
        }

        public string MonthName{get; set; }

        private int _gridSize = 150;
        public int GridSizeWeek
        {
            get { return _gridSize; }
            set
            {
                _gridSize = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BindableDay> Days { get; private set; }

        public string Date
        {
            get
            {
                //var culture = GlobalizationPreferences.Languages[0] + "-" + GlobalizationPreferences.HomeGeographicRegion;
                return new Week(GlobalizationPreferences.Languages[0]).GetYearMonthAndDayFormatted(DateTime.Today);
            }

        }
        
    }

    public class BindableDay : Common.BindableBase
    {
        public bool IsToday { get { return DateTime.Today.CompareTo(_dateTime) == 0; } }

        public string Name
        {
            get
            {
                var dateTimeFormat = new CultureInfo(GlobalizationPreferences.Languages[0]).DateTimeFormat;
                return dateTimeFormat.GetDayName(_dateTime.DayOfWeek);
            }
        }

        private int _gridSize = 250;
        public int GridSize
        {
            get { return _gridSize; }
            set
            {
                _gridSize = value;
                OnPropertyChanged();
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
        public static ObservableCollection<BindableWeek> GetAllWeeks()
        {
            return _sampleDataSource.AllWeeks;
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
                var week = new BindableWeek(startDate);
                if (AllWeeks.Count == 0 && week.WeekNumber > 50)
                    startDate = startDate.AddDays(7);
                AllWeeks.Add(new BindableWeek(startDate));
                startDate = startDate.AddDays(7);
            }
        }
        private static BindableWeek _thisBindableWeek = new BindableWeek(DateTime.Today);
        public static BindableWeek ThisBindableWeek{ get { return _thisBindableWeek; }

        }
    }
}
