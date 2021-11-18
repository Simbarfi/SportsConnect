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
        private string connectionStringToDB =
            ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString;
        DataConnection DT = new DataConnection();

        BusinessLogic BL = new BusinessLogic();
        public SignIn()
        {
            InitializeComponent();

        }

        //private MySqlConnection conexion()
        //{
        //    return new MySqlConnection(connectionStringToDB);

        //}
        private void Login_Click(object sender, RoutedEventArgs e)
        {

            if (txtUserName.Text != string.Empty || txtPassword.Text != string.Empty)
            {
                using (BL.selectForlogin(txtUserName.Text, txtPassword.Text))
                {
                    if (BL.selectForlogin(txtUserName.Text, txtPassword.Text).HasRows)
                    {

                        MeetupMapWindow meetup = new MeetupMapWindow();
                        meetup.Show();

                        //dr.close();
                        MessageBox.Show("no account available with this username and password ", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                    }
                    else
                    {
                        MessageBox.Show("Please fill out all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                    }
                }

            }
        }
    }
}
