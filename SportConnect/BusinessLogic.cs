using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SportConnect
{
    
    public class BusinessLogic
    {
        DataConnection dc = new DataConnection();


        public MySqlDataReader selectForlogin(string username, string password)
        {
            MySqlConnection connectionStringToDB = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);
            connectionStringToDB.Open();
            MySqlCommand cmd = dc.SelectUsers( username,  password, connectionStringToDB);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            MySqlDataReader dataReader = cmd.ExecuteReader();
            return dataReader;

        }

        public MySqlDataReader createCurrent(string username, string password)
        {
            MySqlConnection connectionStringToDB = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);
            connectionStringToDB.Open();
            MySqlCommand cmd = dc.SelectUsers(username, password, connectionStringToDB);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            MySqlDataReader dataReader = cmd.ExecuteReader();
            return dataReader;

        }



        public bool InsertUser(string Username, string FName, string LName, string Email, string Password, string bio, string DOB, string image)
        {
            string query = dc.InsertUserIntoDatabase( Username, FName, LName, Email, Password, bio, DOB, image);
            try
            {
                MySqlConnection connectionStringToDB = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);
                MySqlCommand cmd = new MySqlCommand(query, connectionStringToDB);
                MySqlDataReader MyReader;
                connectionStringToDB.Open();
                MyReader = cmd.ExecuteReader();
                

                while (MyReader.Read())
                {

                }
                connectionStringToDB.Close();
                return true;

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
