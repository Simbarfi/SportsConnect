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
        public RegistrationPage()
        {
            InitializeComponent();
        }

       

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd;
            SqlConnection cn;
            SqlDataReader dr;
            cn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\EE-LT-10033\Source\Repos\lab6.5\SportConnect\SportConnectDatabase.mdf;Integrated Security=True");
            cn.Open();


            if (txtConfirmPassword.Text != string.Empty || txtPassword.Text != string.Empty || txtLastName.Text != string.Empty || txtFirstName.Text != string.Empty || txtEmail.Text != string.Empty)
            {
                if(txtPassword.Text == txtConfirmPassword.Text)
                {
                    cmd = new SqlCommand("select * from LoginTable where username = '" + txtUsername.Text + "'", cn);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        dr.Close();
                        MessageBox.Show("Username Already exist please try another ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        dr.Close();
                        cmd = new SqlCommand("insert into LoginTable values(@username,@password)", cn);
                        cmd.Parameters.AddWithValue("username", txtUsername.Text);
                        cmd.Parameters.AddWithValue("password", txtPassword.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("You Account is created.", "Done",MessageBoxButton.OK, MessageBoxImage.Information);

                    }
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
    }
}
