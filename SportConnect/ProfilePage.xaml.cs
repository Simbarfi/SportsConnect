using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SportConnect
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 

    public partial class ProfilePage : Window
    {
        private string connectionStringToDB =
            ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString;
        private int user_Id = -1;
        private Window previousWindow;
        DataConnection db = new();

        public ProfilePage(int profileUserId, int currentUserId, Window previous)
        {
            //Depending on who views the profile page hide the edit profile button
            //Also Depending on profile viewed...pull their info
            previousWindow = previous;
            user_Id = profileUserId;
            InitializeComponent();
            InsertInfo(user_Id);

            //Cannot edit if not
            if(profileUserId != currentUserId)
            {
                EditProfileButton.IsEnabled = false;
                EditProfileButton.Visibility = Visibility.Hidden;
            }

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (EditProfileButton.Content.ToString() == "Edit Profile")
            {

                EditProfPic.IsEnabled = true;
                EditProfPic.Opacity = 100;
                BioDesc.IsEnabled = true;
                EditProfileButton.Content = "Save";
            }
            else if (EditProfileButton.Content.ToString() == "Save")
            {
                //Go to save info and check info
                EditProfPic.IsEnabled = false;
                EditProfPic.Opacity = 0;
                BioDesc.IsEnabled = false;
                EditProfileButton.Content = "Edit Profile";

                if (checkValues())
                {
                    //Update in sql
                    string bioString = BioDesc.Text.ToString();
                    if (updateUser(bioString, ProfilePic.Source))
                    {/**All Good**/} 
                    else
                    {
                        MessageBox.Show("COULD NOT SAVE");
                    }
                }
                else
                {
                    MessageBox.Show("Could not update your profile");
                }
            }
        }

        private void EditProfPic_Click(object sender, RoutedEventArgs e)
        {
            //Get An Image
            string fileLocation = "";

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = /**"JPEG Files (*.jpeg)";|*.jpeg|*/"PNG Files (*.png)|*.png";/**|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";**/


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                fileLocation = filename;

                BitmapImage basicImg = new BitmapImage();
                basicImg.BeginInit();
                basicImg.UriSource = new Uri(@""+ fileLocation , UriKind.RelativeOrAbsolute);
                basicImg.EndInit();
                ProfilePic.Source = basicImg;
            }



        }


        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void UpcomingEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //NOTHING ONLY THING LIST DOES IS SCROLL
        }

        private void Back_Click_1(object sender, RoutedEventArgs e)
        {
            //MeetupMapWindow window = new();
            //window.Show();
            previousWindow.Show();
            this.Close();
        }

        private Boolean checkValues()
        {
            //Biography
            if (BioDesc.Text.Length > 1000)
            {
                return false;
            }

            return true;
        }

        private void InsertInfo(int userid)
        {

            MySqlConnection connection = new MySqlConnection(connectionStringToDB);
            connection.Open();
            MySqlCommand command = new MySqlCommand(db.GetUserInfo(userid), connection);
            MySqlDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    string textToInput = reader["bio"].ToString();
                    if (reader["bio"] != null)
                    {
                        BioDesc.Text = textToInput;
                    }
                    FirstLast.Content = reader["first_name"] + " " + reader["last_name"];

                    if ((reader["image"].Equals(System.DBNull.Value))) //NEEDS TO BE CHANGED BACK
                    {
                        //take blob and covert into source
                        //ProfilePic.Source = ConvertByteArrayToBitmapImage((byte[])reader["image"]);
                        BitmapImage basicImg = new BitmapImage();
                        basicImg.BeginInit();
                        basicImg.UriSource = new Uri(@"/User.png", UriKind.RelativeOrAbsolute);
                        basicImg.EndInit();

                        ProfilePic.Source = basicImg;
                    } else
                    {
                        BitmapImage basicImg = new BitmapImage();
                        basicImg.BeginInit();
                        basicImg.UriSource = new Uri(@"/User.png", UriKind.RelativeOrAbsolute);
                        basicImg.EndInit();

                        ProfilePic.Source = basicImg;
                    }
                }
            }
            finally
            { 
                reader.Close();
            }

            MySqlCommand command2 = new MySqlCommand(db.getUpcomingEvents(user_Id), connection);
            MySqlDataReader reader2 = command2.ExecuteReader();

            try
            {
                List<Event> myList = new List<Event>(); 
                while (reader2.Read())
                {
                    //for each event in reader2 add a listboxitem to upcomingEvents
                    //create an event class for each row
                    Event currentEvent = new Event(Int16.Parse(reader2["event_id"].ToString()),
                    reader2["event_name"].ToString(),
                    reader2["sport"].ToString(),
                    reader2["start_date"].ToString(),
                    reader2["end_date"].ToString(),
                    Int16.Parse(reader2["max_players"].ToString()),
                    reader2["skill_level"].ToString(),
                    reader2["location"].ToString(),
                    Int16.Parse(reader2["owner"].ToString()));

                    myList.Add(currentEvent);

                    /**
                    MessageBox.Show(reader2["start_date"].ToString());
                    MessageBox.Show(reader2["max_players"].ToString());
                    MessageBox.Show(reader2["sport"].ToString());
                    MessageBox.Show(reader2["location"].ToString());
                    **/
                    
                    
                }
                UpcomingEvents.ItemsSource = myList;
            }
            finally
            {
                // Always call Close when done reading.
                reader2.Close();
            }
            
        }

        /**
         * 
         * Method takes user bio and updates in database.
         * Returns boolean, true if successful
         * 
         **/
        private Boolean updateUser(string bio, ImageSource imgsource)
        {
            MySqlConnection connection = new MySqlConnection(connectionStringToDB);
            connection.Open();

            //convert ImageSource into byte array

            BitmapImage basicImg = new BitmapImage();
            basicImg.BeginInit();
            basicImg.UriSource = new Uri(@"" + imgsource, UriKind.RelativeOrAbsolute);
            basicImg.EndInit();

            Byte[] bits = BitmapToByteArray(basicImg);
            

            MySqlCommand command = new MySqlCommand(db.UpdateUserBioInDatabase(user_Id,bio, bits), connection);
            int result = command.ExecuteNonQuery();
            return result == 1;
        }

        public static Byte[] BitmapToByteArray(BitmapImage image)
        {
            byte[] Data;
            PngBitmapEncoder PngEncoder = new PngBitmapEncoder();
            PngEncoder.Frames.Add(BitmapFrame.Create(image));
            using (System.IO.MemoryStream MS = new System.IO.MemoryStream())
            {
                PngEncoder.Save(MS);
                Data = MS.ToArray();
            }
            return Data;
        }
        public static BitmapImage ConvertByteArrayToBitmapImage(Byte[] bytes)
        {
            System.IO.MemoryStream Stream = new System.IO.MemoryStream(bytes,0,bytes.Length);
            //Stream.Write(bytes, 0, bytes.Length);
            //Stream.Position = 0;
            System.Drawing.Image img = System.Drawing.Image.FromStream(Stream);
            BitmapImage bitImage = new BitmapImage();
            bitImage.BeginInit();
            System.IO.MemoryStream MS = new System.IO.MemoryStream();
            img.Save(MS, System.Drawing.Imaging.ImageFormat.Png);
            MS.Seek(0, System.IO.SeekOrigin.Begin);
            bitImage.StreamSource = MS;
            bitImage.EndInit();
            return bitImage;
        }

        private void openChat(object sender, RoutedEventArgs e)
        {
            // if no selection made in upcoming events
            if(UpcomingEvents.SelectedItem != null)
            {
                Event currentEvent = (Event)UpcomingEvents.SelectedItem;
                MessageBox.Show(currentEvent.Name);

                //I will send a user and event to the chat window and hide the 
                //ChatPage chat = new ChatPage();
                ChatPage chat = new ChatPage(user_Id,currentEvent, this);
                chat.Show();
                Hide();
            } 
            else{
                MessageBox.Show("Select An Event");
            }
        }
    }



    //
    //
    //
    //
    
    

}
