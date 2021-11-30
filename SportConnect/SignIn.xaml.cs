using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace SportConnect
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        

        BusinessLogic BL = new BusinessLogic();
        public SignIn()
        {
            InitializeComponent();
           

        }

        User user = new User();
        
        private void Login_Click(object sender, RoutedEventArgs e)
        {

            if (txtUserName.Text != string.Empty || txtPassword.Password != string.Empty)
            {
                using (BL.selectForlogin(txtUserName.Text, txtPassword.Password))
                {
                    if (BL.selectForlogin(txtUserName.Text, txtPassword.Password).HasRows)
                    {
                        MySqlDataReader userReader = BL.selectForlogin(txtUserName.Text, txtPassword.Password);
                        userReader.Read();
                        user = new User(Int16.Parse(userReader["user_id"].ToString()));
                        MeetupMapWindow meetup = new MeetupMapWindow(user);
                        meetup.Show();
                        Hide();
                    }
                    else
                    {
                        MessageBox.Show("no account available with this username and password ", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please fill out all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
