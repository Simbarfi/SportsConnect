using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;

namespace SportConnect
{
    ///Trevor Abel
    /// <summary>
    /// Interaction logic for MeetupMapWindow, which contains the map for
    /// viewing and creating events.
    /// </summary>
    public partial class MeetupMapWindow : Window
    {
        private const string MAPPATH = "./Map/location.html";
        public User CurUser { get; private set; }
        private MapInteraction mapInteract;
        public MeetupMapWindow(User currentUser)
        {
            CurUser = currentUser;
            InitializeComponent();
            MaxHeight = SystemParameters.WorkArea.Height;
            MaxWidth = SystemParameters.WorkArea.Width;
            WebView.Source = new Uri(System.IO.Path.GetFullPath(MAPPATH));
            mapInteract = new MapInteraction(this);
            InitializeAsync();
        }

        /**
        * Trevor Abel
        * InitializeAsync
        * Ensure webview is intialized before starting, add responses
        * to map inputs, and add events to the map.
        */
        async void InitializeAsync()
        {
            await WebView.EnsureCoreWebView2Async(null);
            await WebView.ExecuteScriptAsync(
                MapScripts.PreventRightClickMenu);

            await AddEventsToMap();
            WebView.CoreWebView2.WebMessageReceived += RespondToEvent;
           
        }
        /**
         * Trevor Abel
         * AddEventsToMap
         * Gets current events from the database and displays them on the
         * map. Events are current if they have not started yet.
         */
        async Task AddEventsToMap()
        {
            BusinessLogic dbConn = new BusinessLogic();
            List<Event> fullEventList = dbConn.GetAllEvents();
            await WebView.CoreWebView2.ExecuteScriptAsync(
                MapScripts.RemoveMarkers + MapScripts.RemoveShadows);

            foreach (Event item in fullEventList)
            {
                if(item.Start > DateTime.Now)
                {
                    string eventHostName = 
                        new BusinessLogic().GetUserName(item.Owner);

                    await WebView.CoreWebView2.ExecuteScriptAsync(
                        MapScripts.AddEventToMap(item, eventHostName));
                }
            }
        }
        /**
         *Trevor Abel
         *RespondToEvent
         * Handles messages sent from the map to the program.
         * There are two possible messages: to create and event
         * or attend one.
         * Messages are strings with headers to identify what event
         * is happening.
         * Messages follow this format: 'HEADER@MESSAGE'
         */
        private void RespondToEvent(object sender, 
            CoreWebView2WebMessageReceivedEventArgs e)
        {
            string response = e.TryGetWebMessageAsString();
            string[] splitResponse = response.Split('@');
            switch (splitResponse[0])
            {
                case "CreateEvent":
                    
                    string eventDetails = 
                        mapInteract.CreateEvent(splitResponse[1]);

                    WebView.CoreWebView2.PostWebMessageAsString(eventDetails);
                    _ = AddEventsToMap();
                    break;
                case "AttendEvent":
                    if(mapInteract.AttendEvent(CurUser.UserId, splitResponse[1]))
                    {
                        MessageBox.Show(this, 
                            "Event joined successfully");
                    }
                    else
                    {
                        MessageBox.Show(this, 
                            "Failed to join event");
                    }
                    break;
            }
        }
        /**
         * Trevor Abel
         * ProfileButtonOnClick
         * Handler for Profile button's onclick.
         * Creates a new profile page and shows it.
         * This page is hidden to be brought back later.
         */
        private void ProfileButtonOnClick(object sender, RoutedEventArgs e)
        {
            ProfilePage profile = 
                new ProfilePage(CurUser.UserId , CurUser.UserId, this);
            profile.Show();
            Hide();
        }
        /**
         * Trevor Abel
         * ChatButtonOnClick
         * Handler for Chat button's onclick.
         * Creates a new chat page and shows it.
         * This page is hidden to be brought back later.
         */
        private void ChatButtonOnClick(object sender, RoutedEventArgs e)
        {
            ChatPage chatPage = new ChatPage(this);
            chatPage.Show();
            Hide();
        }
        /**
         *Trevor Abel
         *MaximizeButOnClick
         * Custom maximize button's onclick
         * Maximizes the window or sets it back to normal
         */
        private void MaximizeButOnClick(object sender, RoutedEventArgs e)
        {
            switch(WindowState)
            {
                case WindowState.Normal:
                    //Without noresize, the window incorrectly maximizes
                    ResizeMode = ResizeMode.NoResize;
                    WindowState = WindowState.Maximized;
                    ResizeMap();
                    break;
                case WindowState.Maximized:
                    ResizeMode = ResizeMode.CanResizeWithGrip;
                    WindowState = WindowState.Normal;
                    ResizeMap();
                    break;
            }
        }
        /**
         * Trevor Abel
         * ResizeMap
         * Resizes the JS map based on the webview's height and width
         */
        private void ResizeMap()
        { 
            WebView.ExecuteScriptAsync(
                MapScripts.ResizeMap(WebView.ActualHeight,
                                     WebView.ActualWidth));
        }
        /**
         *Trevor Abel
         *MinimizeButOnClick
         *Custom minimize button's onclick.
         *Minimizes the window.
         */
        private void MinimizeButOnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        /**
         *Trevor Abel
         *CloseButOnClick
         *Custom close button's onclick
         * Closes the entire application.
         */
        private void CloseButOnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        /**
         * Trevor Abel
         * WindowBorMouseDown
         * Custom window mouseDown.
         * Allows the window to be dragged like a normal window.
         */
        private void WindowBorMouseDown(object sender, 
            MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        /**
         * Trevor Abel
         * WindowOnSizeChanged
         * Window SizeChanged event
         * Resizes the map if the webview exists
         * Allows map to resize whenever the window is resized.
         */
        private void WindowOnSizeChanged(object sender, 
            SizeChangedEventArgs e)
        {
            if(WebView.CoreWebView2 != null)
            {
                ResizeMap();
            }
            
        }
    }
}
