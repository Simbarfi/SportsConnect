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

namespace SportConnect
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd;
            SqlConnection cn;
            SqlDataReader dr;

            cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\EE-LT-10033\Source\Repos\lab6.5\SportConnect\SportConnectDatabase.mdf;Integrated Security=True");
            cn.Open();

            if (txtUserName.Text != string.Empty || txtPassword.Text != string.Empty)
            {
                cmd = new SqlCommand("select * from LoginTable where username='" + txtUserName.Text + "' and password='" + txtPassword.Text+"'", cn);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    this.Hide();
                    MeetupMapWindow meetup = new MeetupMapWindow();
                    meetup.Show();
                }
                else
                {
                    dr.Close();
                    MessageBox.Show("No Account available with this username and password ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please fill out all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            
        }
    }
}
