using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;


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
                EditProfPic.Opacity = 100;
                BioDesc.IsEnabled = false;
                EditProfileButton.Content = "Edit Profile";

                if (checkValues())
                {
                    //Update in sql

                    string bioString = BioDesc.Text.ToString();
                    if (updateUser(bioString))
                    {
                        MessageBox.Show("SAVED");
                    } else
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
            //Ask to upload a file
            MessageBox.Show("Input valid image");
            
            //Put file into database as a blob


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
            //Image

            //Biography
            if (BioDesc.Text.Length > 1000)
            {
                return false;
            }

            return true;
        }

        private void InsertInfo(int userid)
        {
            string userQueryString = "SELECT first_name, last_name, image, bio " +
                "FROM Users " +
                "WHERE Users.user_id = " + userid;

            MySqlConnection connection = new MySqlConnection(connectionStringToDB);
            connection.Open();
            MySqlCommand command = new MySqlCommand(userQueryString, connection);
            
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

                    if(reader["image"] != null)
                    {
                        
                    } else
                    {
                        
                    }
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader.Close();
            }
            connection.Close();

            string upcomingEventString = "SELECT Events.start_date, Events.max_players, Events.sport, Events.location " +
                "FROM AttendedEvents, Events " +
                "WHERE user_id = " + userid + " " +
                "AND AttendedEvents.event_id = Events.event_id " +
                "AND Events.start_date > NOW()";


            connection.Open();
            MySqlCommand command2 = new MySqlCommand(upcomingEventString, connection);
            MySqlDataReader reader2 = command2.ExecuteReader();

            try
            {
                while (reader2.Read())
                {
                    //for each event in reader2 add a listboxitem to upcomingEvents

                    string eventStr = reader2["start_date"].ToString() + " " +
                        reader2["max_players"].ToString() + " " +
                        reader2["sport"].ToString() + " " +
                        reader2["location"].ToString();
                    /**
                    MessageBox.Show(reader2["start_date"].ToString());
                    MessageBox.Show(reader2["max_players"].ToString());
                    MessageBox.Show(reader2["sport"].ToString());
                    MessageBox.Show(reader2["location"].ToString());
                    **/
                    
                    UpcomingEvents.Items.Add(eventStr);
                }
            }
            finally
            {
                // Always call Close when done reading.
                reader2.Close();
            }
            
        }

        private Boolean updateUser(string bio)
        {
            MySqlConnection connection = new MySqlConnection(connectionStringToDB);
            connection.Open();

            string updateStr = "UPDATE Users" +
                " SET bio = '" + bio + "'" +
                " WHERE user_id = " + user_Id;

            MySqlCommand command = new MySqlCommand(updateStr, connection);
            int result = command.ExecuteNonQuery();
            return result == 1;
        }

        
    }
}
