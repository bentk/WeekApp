using WeekCalendar;
using WeekNumber.Common;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.System.UserProfile;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


// The Grid App template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

namespace WeekNumber
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App 
    {
        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            
            InitializeComponent();
            Suspending += OnSuspending;
        }
        public static Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Point point = buttonTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Do not repeat app initialization when already running, just ensure that
            // the window is active
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                Window.Current.Activate();
                return;
            }

            // Create a Frame to act as the navigation context and associate it with
            // a SuspensionManager key
            var rootFrame = new Frame();
            SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                // Restore the saved session state only when appropriate
                await SuspensionManager.RestoreAsync();
            }

            if (rootFrame.Content == null)
            {
                var culture = GlobalizationPreferences.Languages[0] + "-" + GlobalizationPreferences.HomeGeographicRegion;
                var week = new Week(culture);
                
               
                var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareBlock);
                
                var tileTextAttributes = tileXml.GetElementsByTagName("text");
                tileTextAttributes[0].InnerText = week.GetWeekNumberFromDate(DateTime.Today).ToString();
                tileTextAttributes[1].InnerText = week.DayAndMonthStringFromDate(DateTime.Today);
                var scheduledTile = new ScheduledTileNotification(tileXml, DateTime.Now.AddSeconds(1));
                
                var tileXmlWide = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideBlockAndText02);
                var tileTextAttributesWide = tileXmlWide.GetElementsByTagName("text");
                tileTextAttributesWide[0].InnerText = week.GetWeekNumberFromDate(DateTime.Today).ToString();
                tileTextAttributesWide[1].InnerText = week.DayAndMonthStringFromDate(DateTime.Today);
                var scheduledTileWide = new ScheduledTileNotification(tileXml, DateTime.Now.AddSeconds(1));
                
                var t = TileUpdateManager.CreateTileUpdaterForApplication();
                
                t.AddToSchedule(scheduledTile);
                t.AddToSchedule(scheduledTileWide);
            
                t.StartPeriodicUpdate(new Uri("http://weeknumber.apphb.com/TileSquareBlock.aspx?culture=" + culture), PeriodicUpdateRecurrence.HalfHour);

                t.StartPeriodicUpdate(new Uri("http://weeknumber.apphb.com/TileWideBlockAndText02.aspx?culture=" + culture), PeriodicUpdateRecurrence.HalfHour);
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(GroupedItemsPage), "AllWeeks"))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }
    }
}
