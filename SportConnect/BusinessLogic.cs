using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SportConnect
{
    
    public class BusinessLogic
    {
        DataConnection dc = new DataConnection();


        public MySqlDataReader selectForlogin(string username, string password)
        {
            MySqlDataReader dataReader;
            MySqlConnection connectionStringToDB = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);
            
            string query = dc.SelectUsers(username, password);
            MySqlCommand cmd = new MySqlCommand(query, connectionStringToDB);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            connectionStringToDB.Open();
            dataReader = cmd.ExecuteReader();
            return dataReader;
        }

        public bool InsertUser(string Username, string FName, string LName, string Email, string Password, string bio, string DOB, byte[] image)
        {
            string query = dc.InsertUserIntoDatabase( Username, FName, LName, Email, Password, bio, DOB, image);
            try
            {
                MySqlConnection connectionStringToDB = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);
                MySqlCommand cmd = new MySqlCommand(query, connectionStringToDB);
                MySqlDataReader MyReader;
                connectionStringToDB.Open();
                MyReader = cmd.ExecuteReader();
                connectionStringToDB.Close();
                return true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public Byte[] BitmapToByteArray(BitmapImage image)
        {
            byte[] Data;
            JpegBitmapEncoder JpegEncoder = new JpegBitmapEncoder();
            JpegEncoder.Frames.Add(BitmapFrame.Create(image));
            using (System.IO.MemoryStream MS = new System.IO.MemoryStream())
            {
                JpegEncoder.Save(MS);
                Data = MS.ToArray();
            }
            return Data;
        }
        public BitmapImage ConvertByteArrayToBitmapImage(Byte[] bytes)
        {
            System.IO.MemoryStream Stream = new System.IO.MemoryStream();
            Stream.Write(bytes, 0, bytes.Length);
            Stream.Position = 0;
            System.Drawing.Image img = System.Drawing.Image.FromStream(Stream);
            BitmapImage bitImage = new BitmapImage();
            bitImage.BeginInit();
            System.IO.MemoryStream MS = new System.IO.MemoryStream();
            img.Save(MS, System.Drawing.Imaging.ImageFormat.Jpeg);
            MS.Seek(0, System.IO.SeekOrigin.Begin);
            bitImage.StreamSource = MS;
            bitImage.EndInit();
            return bitImage;
        }
    }
}
