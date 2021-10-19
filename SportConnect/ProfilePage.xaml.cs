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
    public partial class ProfilePage : Window
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (EditProfileButton.Content.ToString() == "Edit Profile") {

                EditProfPic.IsEnabled = true;
                EditProfPic.Opacity = 100;
                BioDesc.IsEnabled = true;
                AddAccInfo.IsEnabled = true;
                EditProfileButton.Content = "Save";
            } else if(EditProfileButton.Content.ToString() == "Save")
            {
                EditProfPic.IsEnabled = false;
                EditProfPic.Opacity = 0;
                BioDesc.IsEnabled = false;
                AddAccInfo.IsEnabled = false;
                EditProfileButton.Content = "Edit Profile";
            }

        }

        private void EditProfPic_Click(object sender, RoutedEventArgs e)
        {
            //Ask for valid image
            MessageBox.Show("Input valid image");
            
        }

        private void FriendsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FindFriends findFriends = new();
            findFriends.Show();
        }

        private void TextBox_TextChanged_3(object sender, TextChangedEventArgs e)
        {

        }
    }
}
