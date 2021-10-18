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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SportConnect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //ProfilePage window1 = new();
            //window1.Show();

        }

        //test
        //Trevor

        //test
        //Sharad
        public void test()
        {
            // Sharad able to get project 
            // Dalton Finally figure it out
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GoToSignIn(object sender, RoutedEventArgs e)
        {
            SignIn signIn = new SignIn();
            signIn.Show();
        }
    }
}
