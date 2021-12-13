using System.Windows;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace SportConnect
{
    /**
     * Trevor Abel
     * A class for interacting with the meetup map
     */
    [ComVisible(true)]
    public class MapInteraction
    {
        private MeetupMapWindow parentWindow;

        public MapInteraction(MeetupMapWindow win)
        {
            parentWindow = win;
        }
        /**
         * Trevor Abel
         * CreateEvent
         * Opens an AddEventWindow to create a new event.
         * msg is the latitude and longitude in this form: LatLng(00.00, 00.00);
         * returns a Json string of the event or an empty string if failed
         */
        public string CreateEvent(string msg)
        { 
            double latitude = 0;
            double longitude = 0;
            GetLatLngFromMessage(ref latitude, ref longitude, msg);
            AddEventWindow addWin = new AddEventWindow(latitude, longitude);
            addWin.Owner = parentWindow;
            addWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            bool? didAddEvent = addWin.ShowDialog();
            
            if (didAddEvent.HasValue && didAddEvent.Value)
            {
                AddEventToDB(addWin.NewEvent);
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin),
                    WriteIndented = true
                };
                string eventJson = JsonSerializer.Serialize(addWin.NewEvent, options);
                return eventJson;
            }

            return "";
        }

        /**
         * Trevor Abel
         * GetLatLngFromMessage
         * Creates a latitude and longitude from a string in the format: LatLng(00.00, 00.00)
         * returns true if both values are parsed successfully, else false;
         */
        private bool GetLatLngFromMessage(ref double lat, ref double lng, string msg)
        { //"LatLng(44.024483, -88.550062)"
            bool success = false;
            string[] editedMessage = msg.Replace("LatLng(", "").Replace(')', ' ').Trim().Split(',');
            success = double.TryParse(editedMessage[0], out lat);
            success = success && double.TryParse(editedMessage[1], out lng);
            return success;
        }
        /**
         * Trevor Abel
         * AddEventToDB
         * Adds an event to the database.
         */
        private void AddEventToDB(Event newEvent)
        {
            BusinessLogic dbLogic = new BusinessLogic();
            dbLogic.InsertEvent(newEvent, parentWindow.CurUser);
        }
        /**
         * Trevor Abel
         * AttendEvent
         * Lets a user attend an event and stores it in the database
         * returns true if user successfully attends the event, else false
         */
        internal bool AttendEvent(int currUserId, string eventIdAsString)
        {
            if(int.TryParse(eventIdAsString, out int eventId))
            {
                BusinessLogic dbLogic = new BusinessLogic();
                return dbLogic.InsertAttendedEvent(currUserId, eventId);
            }
            else
            {
                return false;
            }
        }
    }
}
