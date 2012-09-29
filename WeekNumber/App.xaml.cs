using WeekCalendar;
using WeekNumber.Common;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Data.Xml.Dom;
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
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {

            //SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
            
            //TileUpdateManager.CreateTileUpdaterForApplication().
            // Do not repeat app initialization when already running, just ensure that
            // the window is active
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                Window.Current.Activate();
                return;
            }

            var rootFrame = new Frame();
            SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

            if (rootFrame.Content == null)
            {
                var lang = GlobalizationPreferences.Languages[0];
                var week = new Week(lang);
                
                var t = TileUpdateManager.CreateTileUpdaterForApplication();

                var xmlString = string.Format(@"<tile><visual branding='logo'>
                <binding template='TileWideBlockAndText02'><text id='1'>{0}</text><text id='2'>{1}</text><text id='3'></text></binding>
                <binding template='TileSquareBlock'><text id='1'>{1}</text><text id='2'>{2}</text></binding></visual></tile>",
                              week.GetYearMonthAndDayFormatted(DateTime.Today),
                              week.GetWeekNumberFromDate(DateTime.Today).ToString(),
                              week.DayAndMonthStringFromDate(DateTime.Today));
                var xml = new XmlDocument();
                xml.LoadXml(xmlString);
                var scheduledTileWide = new ScheduledTileNotification(xml, DateTime.Now.AddSeconds(1));
                t.AddToSchedule(scheduledTileWide);

                t.StartPeriodicUpdate(new Uri("http://weeknumber.apphb.com/TileWideBlockAndText02.aspx?culture=" + lang), PeriodicUpdateRecurrence.HalfHour);


                    //var badgeXml = Notifications.BadgeUpdateManager.getTemplateContent(Notifications.BadgeTemplateType.badgeNumber);
                    //var badgeAttributes = badgeXml.getElementsByTagName("badge");
                    //badgeAttributes[0].setAttribute("value", "7");

                    //BadgeNumericNotificationContent badgeContent = new BadgeNumericNotificationContent(6);
                    //var badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
                    //badgeXml.DocumentElement.SetAttribute("value","3");
                    //badgeAttributes[0].NodeValue = 7;
                
                //LockScreenUpdateManager

                  //  BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(new BadgeNotification(badgeXml));

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(GroupedItemsPage)))
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

        protected override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            SearchResultsPage.Activate(args.QueryText);
            //if (!new Frame().Navigate(typeof(SearchResultsPage), args.QueryText))
            //{
              //  throw new Exception("Failed to navigate to page from search");
            //}
        }

    }

}
