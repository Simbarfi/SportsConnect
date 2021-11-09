using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
namespace SportConnect
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MeetupMapWindow : Window
    {
        private const string MAP_PATH = "./Map/location.html";
        public MeetupMapWindow()
        {
            InitializeComponent();
            MapBro.Source = new Uri(System.IO.Path.GetFullPath(MAP_PATH));
            MapBro.ObjectForScripting = new MapInteraction(MapBro, this);
        }

        private void ProfileButtonOnClick(object sender, RoutedEventArgs e)
        {
            ProfilePage profile = new ProfilePage(1);
            profile.Show();
            Close();
        }

        private void ChatButtonOnClick(object sender, RoutedEventArgs e)
        {
            ChatPage chatPage = new ChatPage();
            chatPage.ShowDialog();
            Close();
        }

        
    }
}
