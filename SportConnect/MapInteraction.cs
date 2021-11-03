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
            addWin.ShowDialog();

            //try
            //{
            //    //browser.InvokeScript("AddEvent");
            //}
            //catch (COMException)
            //{

            //}
            return true;
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
