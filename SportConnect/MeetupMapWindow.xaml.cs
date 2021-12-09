using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public User CurUser { get; private set; }
        private MapInteraction mapInteract;
        public MeetupMapWindow(User currentUser)
        {
            //CurUser = currentUser;
            User fu = new User(1);
            CurUser = fu;
            InitializeComponent();
            MaxHeight = SystemParameters.WorkArea.Height;
            MaxWidth = SystemParameters.WorkArea.Width;
            WebView.Source = new Uri(System.IO.Path.GetFullPath(MAP_PATH));
            mapInteract = new MapInteraction(this);
            InitializeAsync();
            

        }

        /**
* Trevor Abel
* Ensure webview is intialized before starting and add responses to map inputs
*/
        async void InitializeAsync()
        {
            await WebView.EnsureCoreWebView2Async(null);
            await WebView.CoreWebView2.ExecuteScriptAsync("window.addEventListener('contextmenu', window => {window.preventDefault();});");
            await AddEventsToMap();
            WebView.CoreWebView2.WebMessageReceived += RespondToEvent;
            
        }

        async Task AddEventsToMap()
        {
            BusinessLogic dbConn = new BusinessLogic();
            List<Event> fullEventList = dbConn.GetAllEvents();
            foreach (Event item in fullEventList)
            {
                if(item.Start > DateTime.Now)
                {
                    await WebView.CoreWebView2.ExecuteScriptAsync($"var cm = L.marker([{item.Latitude},{item.Longitude}]);");
                    await WebView.CoreWebView2.ExecuteScriptAsync($"var pop = '{item.Name} <button name=\"{item.Id}\" onclick=\"attendEvent(name)\">Join</button>';");
                    await WebView.CoreWebView2.ExecuteScriptAsync("cm.bindPopup(pop);");
                    await WebView.CoreWebView2.ExecuteScriptAsync("cm.addTo(myMap);");
                }
                
            }

        }

        private void RespondToEvent(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string response = e.TryGetWebMessageAsString();
            string[] splitResponse = response.Split('@');
            switch (splitResponse[0])
            {
                case "CreateEvent":
                    
                    string eventDetails = mapInteract.CreateEvent(splitResponse[1]);
                    WebView.CoreWebView2.PostWebMessageAsString(eventDetails);
                    WebView.CoreWebView2.ExecuteScriptAsync($"console.log({eventDetails})");
                    break;
                case "AttendEvent":
                    if(mapInteract.AttendEvent(CurUser.UserId, splitResponse[1]))
                    {
                        MessageBox.Show(this, "Event joined successfully");
                    }
                    else
                    {
                        MessageBox.Show(this, "Failed to join event");
                    }
                    break;
            }
        }

        private void ProfileButtonOnClick(object sender, RoutedEventArgs e)
        {
            //Dalton- when calling profile page call in the order (the id of the profile your viewing, the id of the current user, this)
            //Also make sure to only hide your window instead of close it. That way I can use the back button to reopen your window
            ProfilePage profile = new ProfilePage(CurUser.UserId , CurUser.UserId, this);
            profile.Show();
            Hide();
        }

        private void ChatButtonOnClick(object sender, RoutedEventArgs e)
        {
            ChatPage chatPage = new ChatPage(this);
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
            Application.Current.Shutdown();
        }

        private void WindowBorMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void WindowOnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(WebView.CoreWebView2 != null)
            {
                ResizeMap();
            }
            
        }
    }
}
