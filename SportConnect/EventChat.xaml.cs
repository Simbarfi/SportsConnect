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
using System.Timers;
using System.Windows.Threading;

namespace SportConnect
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class EventChat : Window
    {
        Window previousWindow;
        private User CurUser;
        private BusinessLogic bl = new BusinessLogic();
        private Event CurEvent;
        private Timer aTimer = new Timer();
        public EventChat()
        {
            InitializeComponent();

        }

        public EventChat(User curUser, Event currentEvent, Window previous)
        {
            InitializeComponent();
            SportName.Content = "Event Name: " + currentEvent.Name.ToString();
            CurUser = curUser;
            CurEvent = currentEvent;
            Username.Content = curUser.UserName;
            previousWindow = previous;
            InsertMessages();

            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            InsertMessages();
        }

        private Boolean Send_Message()
        {
            //get textbox content
            string currentMessage = MessageBox.Text;
            if(currentMessage.Length > 1000)
            {
                //message is too long
                return false;
            } else
            {
                //insert into chat table
                bl.InsertChat(currentMessage, CurUser.UserName, CurEvent.Id);
            }
            return false;
        }

        public void InsertMessages()
        {
            //get all chats with matching eventId
            //For each chat display the message
            List<Message> messages = bl.GetMessages(CurEvent.Id);
            MessageList.ItemsSource = messages;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnBack(object sender, RoutedEventArgs e)
        {
            previousWindow.Show();
            this.Close();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Send_Message();
        }
    }
}
