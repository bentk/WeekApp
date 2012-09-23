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
            DefaultViewModel["WeekNumber"] = SampleDataSource.ThisBindableWeek.WeekNumber;
            DefaultViewModel["Date"] = SampleDataSource.ThisBindableWeek.Date;
            ((ListViewBase)semanticZoom.ZoomedOutView).ItemsSource = DefaultViewModel["Groups"];
            foreach (var item in flipView.Items)
            {
                var week = item as BindableWeek;
                if(week != null && week.WeekNumber == SampleDataSource.ThisBindableWeek.WeekNumber)
                {
                    flipView.SelectedItem = week;
                    break;
                }
            }
            flipView.SelectedItem = SampleDataSource.ThisBindableWeek;
            flipView.SelectionChanged += FlipViewSelectionChanged;
            itemGridView.TabIndex = 2;
        }

        void HeaderClick(object sender, RoutedEventArgs e)
        {
            //var group = (sender as FrameworkElement).DataContext;
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            //this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataGroup)group).UniqueId);
        }

        private void SemanticZoomViewChangeCompleted1(object sender, SemanticZoomViewChangedEventArgs e)
        {
            
        }

        private void ItemGridViewLoaded1(object sender, RoutedEventArgs e)
        {
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
            SetWeek(week);
            SetListView(week);
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
            //flipView.SelectionChanged -= FlipViewSelectionChanged;
            //itemGridView.ItemsSource = null;
            //itemGridView.ItemsSource = DefaultViewModel["Items"];
            //DefaultViewModel["Items"] = SampleDataSource.GetWeek(SampleDataSource.ThisBindableWeek.WeekNumber).Days;
            //SetCurrentWeek();
            //flipView.SelectedIndex = index;
            //flipView.SelectionChanged += FlipViewSelectionChanged;
        }

        private void SetItemSize()
        {
            var itemSize = ((int) (ActualWidth - 200)/7);

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
//                    SetWeek(week);
                }
            }

        }

        private void Grid_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            itemGridView.Width = ActualWidth;
        }
    }
}
