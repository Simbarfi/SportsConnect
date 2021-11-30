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

        public ProfilePage()
        {
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
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                fileLocation = filename;
                ProfilePic.Source = new BitmapImage(new Uri(fileLocation));
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

                    if (!(reader["image"].Equals(System.DBNull.Value)))
                    {
                        //take blob and covert into source
                        //ProfilePic.Source = ConvertByteArrayToBitmapImage((byte[])reader["Image"]);

                    } else
                    {
                        ProfilePic.Source = new BitmapImage( new Uri("User.png"));
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
                    reader2["location"].ToString());

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
            var bmp = imgsource as BitmapImage;

            int height = bmp.PixelHeight;
            int width = bmp.PixelWidth;
            int stride = width * ((bmp.Format.BitsPerPixel + 7) / 8);

            byte[] bits = new byte[height * stride];
            bmp.CopyPixels(bits, stride, 0);


            MySqlCommand command = new MySqlCommand(db.UpdateUserBioInDatabase(user_Id,bio, bits), connection);
            int result = command.ExecuteNonQuery();
            return result == 1;
        }

        public static BitmapImage ConvertByteArrayToBitmapImage(Byte[] bytes)
        {
            var stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        private void openChat(object sender, RoutedEventArgs e)
        {
            // if no selection made in upcoming events
            if(UpcomingEvents.SelectedItem != null)
            {
                Event currentEvent = (Event)UpcomingEvents.SelectedItem;
                MessageBox.Show(currentEvent.Name);

                //I will send a user and event to the chat window and hide the 
                ChatPage chat = new ChatPage();
                //ChatPage chat = new ChatPage(user_Id,currentEvent, this);
                chat.Show();
                Hide();
            } 
            else{
                MessageBox.Show("Select An Event");
            }
        }
    }
}
