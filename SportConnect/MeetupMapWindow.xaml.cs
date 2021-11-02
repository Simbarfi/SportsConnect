using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SportConnect
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MeetupMapWindow : Window
    {
        private const string MAPPATH = "./Map/location.html";
        public MeetupMapWindow()
        {
            InitializeComponent();
            MapBro.Source = new Uri(System.IO.Path.GetFullPath(MAPPATH));
            MapBro.ObjectForScripting = this;
        }

        private void ProfileButtonOnClick(object sender, RoutedEventArgs e)
        {
            ProfilePage profile = new ProfilePage();
            profile.Show();
            Close();
        }

        private void ChatButtonOnClick(object sender, RoutedEventArgs e)
        {
            ChatPage chatPage = new ChatPage();
            chatPage.ShowDialog();
            Close();
        }

        public void CreateEvent(string msg)
        {
            MessageBox.Show(this, msg);
        }
    }
}
