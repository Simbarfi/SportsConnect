using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportConnect
{
   public class DataConnection
    {

        public string SelectUsers(string username, string password)
        {
            MySqlConnection connectionStringToDB = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);
            string query;
                query = "SELECT * from d6304c5_Team3.Users where user_name='" + username + "' and password='" + password + "';";
            return query;
        }


        public string InsertUserIntoDatabase(string Username, string FName, string LName, string Email, string Password, string bio, string DOB, string image)
        {
            MySqlConnection connectionStringToDB = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);

            string query;
            return query = "INSERT INTO d6304c5_Team3.Users (user_name, first_name, last_name, email, password, bio, DOB, image)" +
                "values ('" + Username + "','" + FName + "','" + LName + "','" + Email + "','" + Password + "', '" + bio + "', '" + DOB + "', '" + image + "');";
        }


        public string InsertEventIntoDatabase(int owner, string eventName, float latitude, float longitude, string sport, DateTime startDate, DateTime endDate, int maxPlayers, string skillLevel, string location)
        {
            MySqlConnection connectionStringToDB = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);

            string query;
            return query = "INSERT INTO d6304c5_Team3.Events (owner, event_name, latitude, longitude, sport, start_date, end_date, max_players, skill_level, location)" +
                "values ('" + owner + "','" + eventName + "','" + latitude + "','" + longitude + "', '" + sport + "', '" + startDate + "', '" + endDate + "','" + maxPlayers + "','" + skillLevel + "','" + location + "');";
        }

        public string InsertChatIntoDatabase(int userID, string dataSent, string dataReceived, string messagelogs, string context)
        {
            MySqlConnection connectionStringToDB = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);

            string query;
            return query = "INSERT INTO d6304c5_Team3.Chats (user_id, data_sent, data_received, message_logs, context)" +
                "values ('" + userID + "','" + dataSent + "','" + dataReceived + "','" + messagelogs + "', '" + context + "');";
        }

        public string InsertAttendedEventsIntoDatabase(int attEventId, int UserId, int Event_Id)
        {
            MySqlConnection connectionStringToDB = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);

            string query;
            return query = "INSERT INTO d6304c5_Team3.AttendedEvents (att_event_id, user_id, event_id)" +
                "values ('" + attEventId + "','" + UserId + "','" + Event_Id + "');";
        }

        public string UpdateUserBioInDatabase(int user_Id, string bio, Byte[] img)
        {
            return "UPDATE Users SET bio = '" + bio + "', image = '" + img + "' WHERE user_id = " + user_Id;
        }

        public string getUpcomingEvents(int user_Id)
        {
            return "SELECT Events.start_date, Events.max_players, Events.sport, Events.location " +
                "FROM AttendedEvents, Events " +
                "WHERE user_id = " + user_Id + " " +
                "AND AttendedEvents.event_id = Events.event_id " +
                "AND Events.start_date > NOW()" +
                " ORDER BY Events.start_date ASC";
        }

        public string GetUserInfo(int user_Id)
        {
            return "SELECT first_name, last_name, image, bio " +
                "FROM Users " +
                "WHERE Users.user_id = " + user_Id;
        }
    }
}
