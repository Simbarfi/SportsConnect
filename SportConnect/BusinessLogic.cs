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

        private enum EventColumnNames {event_id = 0, owner = 1, event_name = 2,
                                      latitude = 3, longitude = 4, sport = 5,
                                      start_date = 6, end_date = 7, max_players = 8,
                                      skill_level = 9, location = 10 };


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

        public bool InsertUser(string Username, string FName, string LName, string Email, string Password, string bio, string DOB)
        {
            string query = dc.InsertUserIntoDatabase( Username, FName, LName, Email, Password, bio, DOB);
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

        /*
         * Trevor Abel
         * Attempts to insert an event into the database, owned by
         * the passed-in user.
         * returns true if the event is successfully inserted, else false.
         */
        public bool InsertEvent(Event newEvent, User currUser)
        {
            string query = dc.InsertEventIntoDatabase(currUser.UserId,
                                newEvent.Name,
                                (float)newEvent.Latitude,
                                (float)newEvent.Longitude,
                                newEvent.Sport,
                                newEvent.Start,
                                newEvent.End,
                                newEvent.MaxPlayers,
                                newEvent.SkillLevel,
                                newEvent.Location);
            try
            {
                MySqlConnection connectionStringToDB = new 
                    MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);
                MySqlCommand cmd = new MySqlCommand(query, connectionStringToDB);
                connectionStringToDB.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                connectionStringToDB.Close();
                return rowsAffected == 1;
            }
            catch (MySqlException ex)
            {
            }
            return false;
        }
        /*
         * Trevor Abel
         * Gets a list of all current events
         */
        public List<Event> GetAllEvents()
        {
            string query = dc.GetAllEvents();
            List<Event> fullEventList = new List<Event>();
            try
            {
                MySqlConnection connectionStringToDB = new
                    MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);
                MySqlCommand cmd = new MySqlCommand(query, connectionStringToDB);
                connectionStringToDB.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    Event nextEvent = new Event(reader.GetInt32((int)EventColumnNames.event_id),
                        reader.GetString((int)EventColumnNames.event_name),
                        reader.GetString((int)EventColumnNames.sport),
                        reader.GetDateTime((int)EventColumnNames.start_date),
                        reader.GetDateTime((int)EventColumnNames.end_date),
                        reader.GetInt32((int)EventColumnNames.max_players),
                        reader.GetString((int)EventColumnNames.skill_level),
                        reader.GetString((int)EventColumnNames.location),
                        (double)reader.GetFloat((int)EventColumnNames.latitude),
                        (double)reader.GetFloat((int)EventColumnNames.longitude),
                        reader.GetInt32((int)EventColumnNames.owner));

                    fullEventList.Add(nextEvent);
                }
                reader.Close();
                connectionStringToDB.Close();
                return fullEventList;
            }
            catch (MySqlException ex)
            {
            }

            return fullEventList;
        }

        /*
         * Trevor Abel
         * Inserts an attendedEvent into the database
         * returns true if the event is successfully attended, else false
         */
        public bool InsertAttendedEvent(int currUserId, int eventToAttendId)
        {
            string query = dc.InsertAttendedEventsIntoDatabase(currUserId, eventToAttendId);
            try
            {
                MySqlConnection connectionStringToDB = new
                    MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);
                MySqlCommand cmd = new MySqlCommand(query, connectionStringToDB);
                connectionStringToDB.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                connectionStringToDB.Close();
                return rowsAffected == 1;
            }
            catch (MySqlException ex)
            {
            }
            return false;

        }

        public Boolean AlreadyAttendingEvent(int curUserId, int eventId)
        {
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);

            connection.Open();
            MySqlCommand command = new MySqlCommand(dc.AlreadyAttendingEvent(curUserId,eventId), connection);
            MySqlDataReader reader = command.ExecuteReader();
            return reader.HasRows;
        }

        /**
         * Trevor Abel
         * Gets a user's name based on their id.
         * returns the user's name if successfull, else null
         */
        public string GetUserName(int userId)
        {
            string query = dc.GetUser(userId);
            string username = null;
            try
            {
                MySqlConnection connectionStringToDB = new
                    MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);
                MySqlCommand cmd = new MySqlCommand(query, connectionStringToDB);
                connectionStringToDB.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                username = reader.GetString("user_name");
                reader.Close();
                connectionStringToDB.Close();
            }
            catch (MySqlException ex)
            {
            }
            return username;
        }

        public void InsertChat(string mes, string username, int eventId)
        {
            MySqlConnection connectionStringToDB = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand(dc.InsertEventChat(mes, username, eventId), connectionStringToDB);
            MySqlDataReader MyReader;
            connectionStringToDB.Open();
            MyReader = cmd.ExecuteReader();
            connectionStringToDB.Close();
        }

        public List<Message> GetMessages(int eventId) 
        {
            List<Message> messages = new List<Message>();
            MySqlConnection connectionStringToDB = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);
            MySqlCommand cmd = new MySqlCommand(dc.GetEventChat(eventId), connectionStringToDB);
            MySqlDataReader MyReader;
            connectionStringToDB.Open();
            MyReader = cmd.ExecuteReader();


            try
            {
                while (MyReader.Read())
                {
                    //username, message, eventId
                    string username = MyReader["username"].ToString();
                    string message = MyReader["message"].ToString();
                    int event_Id = Int16.Parse(MyReader["event_id"].ToString());

                    messages.Add(new Message(username, message, event_Id));
                    
                }
            }
            finally
            {
                MyReader.Close();
                connectionStringToDB.Close();
            }
            return messages;
        }

    }

    
}
