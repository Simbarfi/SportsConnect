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
        private User CurUser;
        private Window previousWindow;
        DataConnection db = new();

        public ProfilePage(int profileUserId, User currentUser, Window previous)
        {
            //Depending on who views the profile page hide the edit profile button
            //Also Depending on profile viewed...pull their info
            previousWindow = previous;
            user_Id = profileUserId;
            CurUser = currentUser;
            InitializeComponent();
            InsertInfo(user_Id);

            //Cannot edit if not
            if(profileUserId != currentUser.UserId)
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

                    if (updateUser(bioString))
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
            dlg.Filter = "JPEG Files (*.jpeg);|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                fileLocation = filename;


                BitmapImage basicImg = new BitmapImage(new Uri(fileLocation, UriKind.RelativeOrAbsolute));
                ProfilePic.Source = basicImg;

                using (FileStream fs = new FileStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    System.IO.BufferedStream bf = new BufferedStream(fs);
                    byte[] buffer = new byte[bf.Length];
                    bf.Read(buffer, 0, buffer.Length);

                    byte[] buffer_new = buffer;

                    fs.Close();
                    bf.Close();
                

                    //Change source to byte array and update user
                    updateUser(BioDesc.Text, buffer_new);
                }

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

                    if (!(reader["image"].Equals(System.DBNull.Value))) //NEEDS TO BE CHANGED BACK
                    {
                        //Attempting to pull image from database
                        Byte[] byteArray = (byte[])reader["image"];

                        //take byte array and turn into profilepic source

                        ProfilePic.Source = ToImage(byteArray);

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
                                   
                }
                UpcomingEvents.ItemsSource = myList;
            }
            finally
            {
                // Always call Close when done reading.
                reader2.Close();
                connection.Close();
            }
            
        }

        /**
         * 
         * Method takes user bio and updates in database.
         * Returns boolean, true if successful
         * 
         **/
        private Boolean updateUser(string bio, Byte[] img)
        {
            
            MySqlConnection connection = new MySqlConnection(connectionStringToDB);
            db.UpdateUserImgInDatabase(user_Id, img,connection);
            return true;
        }

        private Boolean updateUser(string bio)
        {
            MySqlConnection connection = new MySqlConnection(connectionStringToDB);
            connection.Open();
            MySqlCommand command = new MySqlCommand(db.UpdateUserBioInDatabase(user_Id, bio), connection);
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result == 1;
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
                EventChat chat = new EventChat(CurUser,currentEvent, this);
                chat.Show();
                Hide();
            } 
            else{
                MessageBox.Show("Select An Event");
            }
        }

        public BitmapImage ToImage(byte[] array)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new System.IO.MemoryStream(array);
            image.EndInit();
            return image;
        }

        private void LeaveEvent(object sender, RoutedEventArgs e)
        {
            if (UpcomingEvents.SelectedItem != null)
            {
                Event currentEvent = (Event)UpcomingEvents.SelectedItem;
                MessageBox.Show(currentEvent.Name);

                MessageBoxResult result = MessageBox.Show("ARE YOU SURE?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNoCancel);
                
                if(result == MessageBoxResult.Yes)
                {
                    MySqlConnection connection = new MySqlConnection(connectionStringToDB);
                    connection.Open();
                    //if owner delete event 
                    //remove all from attended with eventId
                    if (currentEvent.Owner == user_Id)
                    {
                        //remove all from attended with eventId
                        //then delete event
                        MySqlCommand command = new MySqlCommand(db.RemoveAllAttendingEvent(currentEvent.Id), connection);
                        command.ExecuteNonQuery();

                        //Possibly delete chats if needed

                        
                        MySqlCommand command2 = new MySqlCommand(db.DeleteEvent(currentEvent.Id), connection);
                        command2.ExecuteNonQuery();

                        //MessageBox.Show("YOU GOT HERE" + user_Id + " " +currentEvent.Owner);

                    }
                    else
                    {
                        //else remove from attended events
                        MySqlCommand command = new MySqlCommand(db.RemoveOneAttendingEvent(currentEvent.Id, user_Id), connection);
                        command.ExecuteNonQuery();
                        
                    }
                    
                    connection.Close();
                    InsertInfo(user_Id);

                }
            }
            else
            {
                MessageBox.Show("Select An Event");
            }
        }
    }



    //
    //
    //
    //
    
    

}
