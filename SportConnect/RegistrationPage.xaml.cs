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
using System.Data;
using System.Data.SqlClient;

namespace SportConnect
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Window
    {
        BusinessLogic BL = new BusinessLogic();
        public RegistrationPage()
        {
            InitializeComponent();
            
        }
        
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
           
            if (txtConfirmPassword.Password != string.Empty || txtPassword.Password != string.Empty ||
                txtLastName.Text != string.Empty || txtFirstName.Text != string.Empty || txtEmail.Text != string.Empty)
            {
                if (txtPassword.Password == txtConfirmPassword.Password)
                {
                    BL.InsertUser(txtUsername.Text, txtFirstName.Text, txtLastName.Text, txtEmail.Text, txtPassword.Password, "", "", "");
                    MessageBox.Show("User Added");
                }
                else
                {
                    MessageBox.Show("Please confirm password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please fill out all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
