using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
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
            StartDatePic.SelectedDate = DateTime.Now;
            EndDatePic.SelectedDate = DateTime.Now;
        }

        private void AddEntryButOnClick(object sender, RoutedEventArgs e)
        {
            string eventName = MySqlHelper.EscapeString(EventText.Text);
            string sport = MySqlHelper.EscapeString(SportText.Text);
            int maxPlayers = int.Parse(MaxPlayersText.Text);
            string skillLevel = MySqlHelper.EscapeString(SkillLevelText.Text);
            string location = MySqlHelper.EscapeString(SkillLevelText.Text);
            DateTime startDate = GetStartDate();
            DateTime endDate = GetEndDate();
            if (startDate != DateTime.MinValue)
            {
                if (endDate != DateTime.MinValue)
                {
                    NewEvent = new Event(eventName,
                        sport,
                        startDate,
                        endDate,
                        maxPlayers,
                        skillLevel,
                        location,
                        lat,
                        lng);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show(this, "Please enter a valid end date (hh:mm).");
                }
            }
            else
            {
                MessageBox.Show(this, "Please enter a valid start date (hh:mm).");
            }
            
            
        }

        private DateTime GetStartDate()
        {
            return GetDate(StartDatePic, StartText, StartAMPMCom);
        }

        private DateTime GetEndDate()
        {
            return GetDate(EndDatePic, EndText, EndAMPMCom);
        }

        private DateTime GetDate(DatePicker datePicker, TextBox timeOfDayBox, ComboBox amPmCombo)
        { ///^(0?[1-9]|1[0-2]):[0-5][0-9]$/   
            DateTime? selectedDate = datePicker.SelectedDate;
            TimeSpan timeOfDay;
            bool hasValidTime = TimeSpan.TryParse(timeOfDayBox.Text, out timeOfDay);
            if (selectedDate.HasValue && hasValidTime)
            {
                const int am = 0;
                const int pm = 1;
                const int noon = 12;
                if (amPmCombo.SelectedIndex == pm)
                {
                    timeOfDay += new TimeSpan(noon, 0, 0);
                }
                else if (amPmCombo.SelectedIndex == am && timeOfDay.Hours == noon)
                {
                    timeOfDay -= new TimeSpan(noon, 0, 0);
                }
                return selectedDate.Value.Add(timeOfDay);
            }

            return DateTime.MinValue;
        }

        private void ExitButOnClose(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }


        private void StartPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex digitRegex = new Regex("[^0-9:]");
            e.Handled = digitRegex.IsMatch(e.Text);
        }

        private void MaxPlayersTextOnPreviewTextChange(object sender, TextCompositionEventArgs e)
        {
            Regex r = new Regex("[^0-9]");
            e.Handled = r.IsMatch(e.Text);
        }
    }
}
