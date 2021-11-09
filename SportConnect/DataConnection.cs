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
        public string InsertUserIntoDatabase(string FName, string LName, string Email, string Password, string bio, DateTime DOB, string image)
        {
            MySqlConnection connectionStringToDB = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDB2"].ConnectionString);

            string query;
            return query = "INSERT INTO d6304c5_Team3.Users (first_name, last_name, email, password, bio, DOB, image)" +
                "values ('" + FName + "','" + LName + "','" + Email + "','" + Password + "', '" + bio + "', '" + DOB + "', '" + image + "');";
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
    }
}
