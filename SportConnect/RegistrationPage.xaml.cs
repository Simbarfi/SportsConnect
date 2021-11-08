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
        //MODIFIED
        //insert data inot the database
       private void InsertData() 
       {
            //MODIFIED
            SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=
               C:\Users\EE-LT-10033\Source\Repos\lab6.5\SportConnect\SportConnectDatabase.mdf;Integrated Security=True");
            SqlCommand cmd;
            //MODIFIED
            cmd = new SqlCommand("ISERT INTO LoginTable VALUES(@username,@password)", connect);
            cmd.Parameters.AddWithValue("username", txtUsername.Text);
            cmd.Parameters.AddWithValue("password", txtPassword.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("You Account is created.", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        //MODIFIED
        //select data from database
        private SqlCommand SelectData()
        {
            //MODIFIED
            SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename
            =C:\Users\EE-LT-10033\Source\Repos\lab6.5\SportConnect\SportConnectDatabase.mdf;Integrated Security=True");
            //MODIFIED
            return new SqlCommand("SELECT * FROM LoginTable WHERE username = '" + txtUsername.Text + "'", connect);
        }
        //MODIFIED
        // check if the infomation inseted are correct
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand cmd;
            //MODIFIED
            SqlConnection connect;
            //MODIFIED
            SqlDataReader datareader;
            //MODIFIED
            connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=
            C:\Users\EE-LT-10033\Source\Repos\lab6.5\SportConnect\SportConnectDatabase.mdf;Integrated Security=True");
            connect.Open();

            //MODIFIED
            if (txtConfirmPassword.Text != string.Empty || txtPassword.Text != string.Empty || 
                txtLastName.Text != string.Empty || txtFirstName.Text != string.Empty || txtEmail.Text != string.Empty)
            {
                if(txtPassword.Text == txtConfirmPassword.Text)
                {
                    cmd = SelectData();
                    //MODIFIED
                    datareader = cmd.ExecuteReader();
                    //MODIFIED
                    if (datareader.Read())
                    {
                        //MODIFIED
                        datareader.Close();
                        MessageBox.Show("Username Already exist please try another ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        //MODIFIED
                        datareader.Close();
                        InsertData();

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
