using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;

namespace SportConnect
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MeetupMapWindow : Window
    {
        private const string MAP_PATH = "./Map/location.html";
        private User curUser;
        private MapInteraction mapInteract;
        public MeetupMapWindow(User currentUser)
        {
            curUser = currentUser; 
            InitializeComponent();
            //MapBro.Source = new Uri(System.IO.Path.GetFullPath(MAP_PATH));
            //MapBro.ObjectForScripting = new MapInteraction(MapBro, this);
            MaxHeight = SystemParameters.WorkArea.Height;
            MaxWidth = SystemParameters.WorkArea.Width;
            WebView.Source = new Uri(System.IO.Path.GetFullPath(MAP_PATH));
            InitializeAsync();
            mapInteract = new MapInteraction(this);
        }
        /**
         * Trevor Abel
         * Ensure webview is intialized before starting and add responses to map inputs
         */
        async void InitializeAsync()
        {
            await WebView.EnsureCoreWebView2Async(null);
            WebView.CoreWebView2.WebMessageReceived += RespondToEvent;
            
        }

        private void RespondToEvent(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string response = e.TryGetWebMessageAsString();
            string[] splitResponse = response.Split(',');
            switch (splitResponse[0])
            {
                case "CreateEvent": 
                    string eventDetails = mapInteract.CreateEvent(splitResponse[1]);
                    WebView.CoreWebView2.PostWebMessageAsString(eventDetails);
                    break;
            }
        }

        private void ProfileButtonOnClick(object sender, RoutedEventArgs e)
        {
            //Dalton- when calling profile page call in the order (the id of the profile your viewing, the id of the current user, this)
            //Also make sure to only hide your window instead of close it. That way I can use the back button to reopen your window
            ProfilePage profile = new ProfilePage(curUser.UserId ,curUser.UserId,this);
            profile.Show();
            Hide();
        }

        private void ChatButtonOnClick(object sender, RoutedEventArgs e)
        {
            ChatPage chatPage = new ChatPage();
            chatPage.Show();
            Hide();
        }

        private void MaximizeButOnClick(object sender, RoutedEventArgs e)
        {
            switch(WindowState)
            {
                case WindowState.Normal:
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

        private void ResizeMap()
        {
            WebView.CoreWebView2.ExecuteScriptAsync($"document.getElementById('mapid').style.height = '{WebView.ActualHeight}px';");
            WebView.CoreWebView2.ExecuteScriptAsync($"document.getElementById('mapid').style.width = '{WebView.ActualWidth}px';");
            WebView.CoreWebView2.ExecuteScriptAsync("myMap.invalidateSize(15);");
        }

        private void MinimizeButOnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButOnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowBorMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
