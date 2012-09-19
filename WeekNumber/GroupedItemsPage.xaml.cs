﻿using WeekNumber.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace WeekNumber
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class GroupedItemsPage 
    {
        public GroupedItemsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            this.DefaultViewModel["Items"] = SampleDataSource.GetWeek(SampleDataSource.ThisBindableWeek.WeekNumber).Days;
            this.DefaultViewModel["Groups"] = SampleDataSource.GetAllWeeks(SampleDataSource.ThisBindableWeek.Year);
            this.DefaultViewModel["WeekNumber"] = SampleDataSource.ThisBindableWeek.WeekNumber;
            this.DefaultViewModel["Date"] = SampleDataSource.ThisBindableWeek.Date;
        }
            
        /// <summary>
        /// Invoked when a group header is clicked.
        /// </summary>
        /// <param name="sender">The Button used as a group header for the selected group.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            var group = (sender as FrameworkElement).DataContext;

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            //this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataGroup)group).UniqueId);
        }

        /// <summary>
        /// Invoked when an item within a group is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            //var itemId = ((DateTime)e.ClickedItem);//.GetWeek(e.ClickedItem)
            //this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }

        private void semanticZoom_ViewChangeCompleted_1(object sender, SemanticZoomViewChangedEventArgs e)
        {
            
        }

        private void itemGridView_Loaded_1(object sender, RoutedEventArgs e)
        {

            foreach (var item in itemGridView.Items)
            {
                var day = item as BindableDay;
                if (day != null && day.IsToday)
                    itemGridView.SelectedItem = day;
            }
            
        }

        private void itemGridView_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            var itemSize = ((int)(itemGridView.ActualWidth -100)/7);
            
            //Application.Current.Resources
            foreach (var item in itemGridView.Items)
            {
                var gridItem = item as BindableDay;
                if (gridItem != null)
                {
                    gridItem.GridSize = itemSize;
                    
                }
            }
            itemGridView.ItemsSource= null;
            itemGridView.ItemsSource = DefaultViewModel["Items"];
            
        }
    }
}
