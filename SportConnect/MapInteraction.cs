using System;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Configuration;

namespace SportConnect
{
    [ComVisible(true)]
    public class MapInteraction
    {
        private MeetupMapWindow parentWindow;

        public MapInteraction(MeetupMapWindow win)
        {
            parentWindow = win;
        }
        public string CreateEvent(string msg)
        { 
            double latitude = 0;
            double longitude = 0;
            GetLatLngFromMessage(ref latitude, ref longitude, msg);
            AddEventWindow addWin = new AddEventWindow(latitude, longitude);
            addWin.Owner = parentWindow;
            addWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            bool? didAddEvent = addWin.ShowDialog();

            if (didAddEvent.Value)
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


        private bool GetLatLngFromMessage(ref double lat, ref double lng, string msg)
        { //"LatLng(44.024483, -88.550062)"
            bool success = false;
            string[] editedMessage = msg.Replace("LatLng(", "").Replace(')', ' ').Trim().Split(',');
            success = double.TryParse(editedMessage[0], out lat);
            success = double.TryParse(editedMessage[1], out lng);
            return success;
        }

        private void AddEventToDB(Event newEvent)
        {
            BusinessLogic dbLogic = new BusinessLogic();
            dbLogic.InsertEvent(newEvent, parentWindow.CurUser);

        }
        
    }
}
