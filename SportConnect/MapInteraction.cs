using System;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;

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
        public bool CreateEvent(string msg)
        {
            AddEventWindow addWin = new AddEventWindow();
            addWin.Owner = parentWindow;
            bool? didAddEvent = addWin.ShowDialog();

            if (didAddEvent.Value)
            {
                //Add the event to the db
                return true; //Creates an event on the map
            }

            return false;
        }
        public string[] ViewEvent()
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
