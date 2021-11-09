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
        private WebBrowser browser;

        public MapInteraction(WebBrowser wb,
                              Window win)
        {
            parentWindow = win;
            browser = wb;
        }
        public string CreateEvent(string msg)
        {
            AddEventWindow addWin = new AddEventWindow();
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

            return null;
        }
        public string[] ViewEvent(double lat, double lng)
        {
            MessageBox.Show("Future Event Information Handling");

            return null;
        }

        public void DisplayEvents()
        {
            throw new NotImplementedException();
        }
    }
}
