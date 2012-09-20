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
            itemGridView.IsEnabled = false;
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
        }

        void HeaderClick(object sender, RoutedEventArgs e)
        {
            //var group = (sender as FrameworkElement).DataContext;
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            //this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataGroup)group).UniqueId);
        }

        void ItemViewItemClick(object sender, ItemClickEventArgs e)
        {
            //var itemId = ((DateTime)e.ClickedItem);//.GetWeek(e.ClickedItem)
            //this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }

        private void SemanticZoomViewChangeCompleted1(object sender, SemanticZoomViewChangedEventArgs e)
        {
            
        }

        private void ItemGridViewLoaded1(object sender, RoutedEventArgs e)
        {
            SelectToday();
        }

        private void SelectToday()
        {
            foreach (var item in itemGridView.Items)
            {
                var day = item as BindableDay;
                if (day != null && day.IsToday)
                {
                    itemGridView.SelectedItem = day;
                }
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
                itemGridView.ItemsSource = SampleDataSource.GetWeek(week.WeekNumber).Days;
                SelectToday();
            }
        }
        private void ItemGridViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var itemSize = ((int)(itemGridView.ActualWidth - 100) / 7);

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
    }
}
