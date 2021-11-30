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
                connectionStringToDB.Close();
                return true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Trevor Abel
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
        //Trevor Abel
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
                    Event nextEvent = new Event(reader.GetInt32((int)EventColumnNames.owner),
                        reader.GetString((int)EventColumnNames.event_name),
                        reader.GetString((int)EventColumnNames.sport),
                        reader.GetDateTime((int)EventColumnNames.start_date),
                        reader.GetDateTime((int)EventColumnNames.end_date),
                        reader.GetInt32((int)EventColumnNames.max_players),
                        reader.GetString((int)EventColumnNames.skill_level),
                        reader.GetString((int)EventColumnNames.location),
                        (double)reader.GetFloat((int)EventColumnNames.latitude),
                        (double)reader.GetFloat((int)EventColumnNames.longitude));

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
    }

    
}
