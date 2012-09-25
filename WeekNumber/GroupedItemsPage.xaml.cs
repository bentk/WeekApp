using WeekNumber.Data;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WeekNumber
{
    public sealed partial class GroupedItemsPage 
    {
        public GroupedItemsPage()
        {
            InitializeComponent();
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {

            flipView.SelectionChanged -= FlipViewSelectionChanged;
            
            DefaultViewModel["Groups"] = SampleDataSource.GetAllWeeks();
            DefaultViewModel["Items"] = SampleDataSource.GetWeek(SampleDataSource.ThisBindableWeek.WeekNumber).Days;
            DefaultViewModel["Date"] = SampleDataSource.ThisBindableWeek.Date;
            DefaultViewModel["Month"] = SampleDataSource.GetWeek(SampleDataSource.ThisBindableWeek.WeekNumber).MonthName;

            ((ListViewBase)semanticZoom.ZoomedOutView).ItemsSource = DefaultViewModel["Groups"];
            foreach (var item in flipView.Items)
            {
                var week = item as BindableWeek;
                if(week != null && week.WeekNumber == SampleDataSource.ThisBindableWeek.WeekNumber)
                {
                    flipView.SelectedItem = week;
                    week.SelectedWeek = week.WeekNumber;
                    break;
                }
            }

          
                flipView.SelectedItem = SampleDataSource.ThisBindableWeek;

            flipView.SelectionChanged += FlipViewSelectionChanged;
            itemGridView.TabIndex = 2;
            if (string.IsNullOrEmpty(navigationParameter as string) == false)
            {
                
                var weekNumberToShow = int.Parse(navigationParameter as string);
                flipView.SelectedIndex= weekNumberToShow;
                flipView.SelectedItem = SampleDataSource.GetWeek(weekNumberToShow);
            }

        }

        private void ItemListViewLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in itemListView.Items)
            {
                var day = item as BindableDay;
                if (day != null && day.IsToday)
                    itemListView.SelectedItem = day;
            }
        }

        private void WeeksGridView_OnLoaded(object sender, RoutedEventArgs e)
        {
            SetCurrentWeek();
        }

        private void SetCurrentWeek()
        {
            foreach (var item in weeksGridView.Items)
            {
                var week = item as BindableWeek;
                if (week != null && week.IsCurrentWeek)
                    weeksGridView.SelectedItem = week;
            }
            
        }

        private void FlipViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            var week = e.AddedItems[0] as BindableWeek;
            if (week != null)
            {
                week.SelectedWeek = week.WeekNumber;
                var removedWeek = e.RemovedItems[0] as BindableWeek;
                if (removedWeek != null)
                    removedWeek.SelectedWeek = week.WeekNumber;

                SetWeek(week);
                DefaultViewModel["Month"] = SampleDataSource.GetWeek(week.WeekNumber).MonthName;
                SetListView(week);
            }
            SetItemSize();
        }

        private void SetListView(BindableWeek week)
        {
            if (week != null)
            {
                itemListView.ItemsSource = SampleDataSource.GetWeek(week.WeekNumber).Days;
            }
        }

        private void SetWeek(BindableWeek week)
        {
            if (week != null)
            {
                itemGridView.ItemsSource = SampleDataSource.GetWeek(week.WeekNumber).Days;
                
            }
        }

        private void ItemGridViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetItemSize();
        }

        private void SetItemSize()
        {
            var itemSize = ((int) (ActualWidth - 200)/7);
            if(itemSize <10)
                return;

            foreach (var item in itemGridView.Items)
            {
                var gridItem = item as BindableDay;
                if (gridItem != null)
                {
                    gridItem.GridSize = itemSize;
                }
            }
        }

        private void WeeksGridView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var index = flipView.SelectedIndex;
            var itemSize = ((int) (weeksGridView.ActualWidth)/15);

            foreach (var item in weeksGridView.Items)
            {
                var gridItem = item as BindableWeek;
                if (gridItem != null)
                {
                    gridItem.GridSizeWeek = itemSize;
                }
            }
            flipView.SelectionChanged -= FlipViewSelectionChanged;
            weeksGridView.ItemsSource = null;
            weeksGridView.ItemsSource = DefaultViewModel["Groups"];
            SetCurrentWeek();
            flipView.SelectedIndex = index;
            flipView.SelectionChanged += FlipViewSelectionChanged;
        }
    
        private void DayTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var s = sender as FrameworkElement;
            if(s!=null)
            {
                //var day = s.DataContext as BindableDay;
                //if(day!= null)
                //    new MessageDialog("sdfasdfdad=" + day.Name).ShowAsync();
            }
        }

        private void WeekTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var s = sender as FrameworkElement;
            if (s != null)
            {
                var week = s.DataContext as BindableWeek;
                if(week != null)
                {
                    flipView.SelectedIndex = week.WeekNumber - 1;
                }
            }
        }

        private void GridSizeChanged1(object sender, SizeChangedEventArgs e)
        {
            itemGridView.Width = ActualWidth;
        }

        private void Date_OnClick(object sender, RoutedEventArgs e)
        {

            var week = SampleDataSource.GetWeek(SampleDataSource.ThisBindableWeek.WeekNumber);
            SetWeek(week);
            DefaultViewModel["Month"] = SampleDataSource.GetWeek(week.WeekNumber).MonthName;
            SetListView(week);
            flipView.SelectedItem = week;
            
        }
    }
}
