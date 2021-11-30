using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SportConnect
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddEventWindow : Window
    {
        public Event NewEvent { get; private set; }
        private double lat;
        private double lng;
        public AddEventWindow(double latitude, double longitude){
            InitializeComponent();
            lat = latitude;
            lng = longitude;
            
        }

        private void AddEntryButOnClick(object sender, RoutedEventArgs e)
        {
            //validate input
            NewEvent = new Event(EventText.Text,
                SportText.Text,
                StartText.Text,
                EndText.Text,
                int.Parse(MaxPlayersText.Text),
                SkillLevelText.Text,
                LocationText.Text,
                lat,
                lng);
            DialogResult = true;
            Close();
        }

        private void ExitButOnClose(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
