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
            //Dalton- when calling profile page call in the order (the id of the profile your viewing, the id of the current user, this)
            //Also make sure to only hide your window instead of close it. That way I can use the back button to reopen your window
            ProfilePage profile = new ProfilePage(1,1,this);
            profile.Show();
            this.Hide();
        }

        private void ChatButtonOnClick(object sender, RoutedEventArgs e)
        {
            ChatPage chatPage = new ChatPage();
            chatPage.ShowDialog();
            Close();
        }

        
    }
}
