using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MySql.Data.MySqlClient;
namespace SportConnect
{
    /// Trevor Abel
    /// <summary>
    /// Interaction logic for AddEventWindow
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
        /**
         * Trevor Abel
         * AddEntryButOnClick
         * AddEntryButton onclick
         * Tries to create a new event based on the filled out fields.
         */
        private void AddEntryButOnClick(object sender, RoutedEventArgs e)
        {
            //Keeps track of fields in a dictionary for validation
            Dictionary<string, string> fields = new Dictionary<string, string>();
            fields.Add("eventName", MySqlHelper.EscapeString(EventText.Text));
            fields.Add("sport", MySqlHelper.EscapeString(SportText.Text));
            fields.Add("maxPlayers", MaxPlayersText.Text);
            fields.Add("skillLevel", MySqlHelper.EscapeString(SkillLevelText.Text));
            fields.Add("location", MySqlHelper.EscapeString(LocationText.Text));
            if (ValidateFields(fields))
            {
                DateTime startDate = GetStartDate();
                DateTime endDate = GetEndDate();
                if (ValidateDates(startDate, endDate))
                {
                    NewEvent = new Event(fields["eventName"],
                            fields["sport"],
                            startDate,
                            endDate,
                            int.Parse(fields["maxPlayers"]),
                            fields["skillLevel"],
                            fields["location"],
                            lat,
                            lng);
                    DialogResult = true;
                    Close();
                }
            }
        }
        /**
         * Trevor Abel
         * ValidateFields
         * Validates that each text field has content(length > 0).
         */
        private bool ValidateFields(Dictionary<string, string> fieldsDict)
        {
            foreach (string field in fieldsDict.Values)
            {
                if (field.Length == 0)
                {
                    return false;
                }
            }

            return true;
        }
        /**
         * Trevor Abel
         * ValidateDates
         * Checks if the start and end dates are both valid.
         */
        private bool ValidateDates(DateTime start, DateTime end)
        {
            //Bad Datetime value == MinValue
            bool result = false;
            if (start == DateTime.MinValue && end == DateTime.MinValue)
            {
                MessageBox.Show(this, 
                    "Please enter valid start and end dates (hh:mm).");
            }
            else if (start == end)
            {
                MessageBox.Show(this, 
                    "Your start and end times are the same. Please give some time to your event.");
            }
            else if(start > end)
            {
                MessageBox.Show(this, 
                    "Please ensure your start time is before your end time.");
            } 
            else if (start == DateTime.MinValue)
            {
                MessageBox.Show(this, 
                    "Please enter a valid start date (hh:mm).");
            }
            else if (end == DateTime.MinValue)
            {
                MessageBox.Show(this, 
                    "Please enter a valid end date (hh:mm).");
            }
            else
            {
                result = true;
            }

            return result;
        }
        /**
         * Trevor Abel
         * GetStartDate
         * Gets the Start date from the three start date fields.
         */
        private DateTime GetStartDate()
        {
            return GetDate(StartDatePic, StartText, StartAMPMCom);
        }
        /**
         * Trevor Abel
         * GetEndDate
         * Gets the End date from the three end date fields.
         */
        private DateTime GetEndDate()
        {
            return GetDate(EndDatePic, EndText, EndAMPMCom); 
        }
        /**
         * Trevor Abel
         * GetDate
         * Gets a date from three date fields.
         * The DatePicker has the date, the TextBox has the time,
         * and the ComboBox tells you if it's in the AM or PM.
         * The time should be in the format 00:00 and should not
         * be in military time.
         */
        private DateTime GetDate(DatePicker datePicker, TextBox timeOfDayBox, ComboBox amPmCombo)
        { 
            //Matches time in hours with a leading zero
            Regex correctTime = new Regex("^(0?[1-9]|1[0-2]):[0-5][0-9]$");
            DateTime? selectedDate = datePicker.SelectedDate;
            TimeSpan timeOfDay;
            bool hasValidTime = TimeSpan.TryParse(timeOfDayBox.Text, out timeOfDay);

            if (selectedDate.HasValue && hasValidTime && correctTime.IsMatch(timeOfDayBox.Text))
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
                    //Datetimes are in miliary time so 12:00 AM => 00:00
                    timeOfDay -= new TimeSpan(noon, 0, 0);
                }
                return selectedDate.Value.Add(timeOfDay);
            }

            return DateTime.MinValue;
        }
        /**
         * Trevor Abel
         * ExitButOnClose
         * Exit Button onClose
         * Closes the window and sets the result to false
         */
        private void ExitButOnClose(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        /**
         * Trevor Abel
         * StartEndPreviewTextInput
         * StartText EndText PreviewTextInput
         * Ensures only numbers are entered into the text field
         */
        private void StartEndPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex digitRegex = new Regex("[^0-9:]");
            e.Handled = digitRegex.IsMatch(e.Text);
        }
        /**
         * Trevor Abel
         * MaxPlayersTextOnPreviewTextChange
         * MaxPlayersText PreviewTextChange
         * Ensures only numbers are entered into the text field
         */
        private void MaxPlayersTextOnPreviewTextChange(object sender, TextCompositionEventArgs e)
        {
            Regex r = new Regex("[^0-9]");
            e.Handled = r.IsMatch(e.Text);
        }
        /**
         * Trevor Abel
         * TextBoxOnFocus
         * Generic TextBox onFocus
         * Selects all text when the textBox is focused.
         */
        private void TextBoxOnFocus(object sender, RoutedEventArgs e)
        {
            TextBox text = sender as TextBox;
            text.SelectAll();
        }
        /**
         * Trevor Abel
         * TextBoxGotMouseFocus
         * Generic TextBox onMouseFocus
         * Selects all text when the textBox captures the mouse.
         */
        private void TextBoxGotMouseFocus(object sender, MouseEventArgs e)
        {
            TextBox text = sender as TextBox;
            text.SelectAll();
        }
    }
}
