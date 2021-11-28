using System;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace SportConnect
{
    [ComVisible(true)]
    public class MapInteraction
    {
        private Window parentWindow;

        public MapInteraction(Window win)
        {
            parentWindow = win;
        }
        public string CreateEvent(string msg)
        { //"LatLng(44.024483, -88.550062)"
            double latitude = 0;
            double longitude = 0;
            GetLatLngFromMessage(ref latitude, ref longitude, msg);
            AddEventWindow addWin = new AddEventWindow(latitude, longitude);
            addWin.Owner = parentWindow;
            bool? didAddEvent = addWin.ShowDialog(); 

            if (didAddEvent.Value)
            {
                //Add the event to the db
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
        {
            bool success = false;
            string[] editedMessage = msg.Replace("LatLng(", "").Replace(')', ' ').Trim().Split(',');
            success = double.TryParse(editedMessage[0], out lat);
            success = double.TryParse(editedMessage[1], out lng);
            return success;
        }
        
    }
}
