using Authenticator;
using InstanceLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AuthInterface authenticator; private static string username; private static string password;
        public MainWindow()
        {
            InitializeComponent();
            authenticator = Instance.getInterface();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            username = UserNameBox.Text;
            password = PasswordBox.Text;

            GUIDisable();

            Task<string> task = new Task<string>(AsyncTask);
            task.Start();
            string result = await task;
            System.Windows.Forms.MessageBox.Show(result, "Registration", MessageBoxButtons.OK, MessageBoxIcon.Information);

            GUIEnable();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            username = UserNameBox.Text;
            password = PasswordBox.Text;

            GUIDisable();


            GUIEnable();
        }

        private string AsyncTask()
        {
            string validation = authenticator.Register(username, password);
            return validation;
        }

        private void GUIDisable()
        {
            UserNameBox.IsReadOnly = true;
            PasswordBox.IsReadOnly = true;
            RegisterButton.IsEnabled = false;
            LoginButton.IsEnabled = false;
            Progressbar.IsIndeterminate = true;
        }

        private void GUIEnable()
        {
            UserNameBox.IsReadOnly = false;
            PasswordBox.IsReadOnly = false;
            RegisterButton.IsEnabled = true;
            LoginButton.IsEnabled = true;
            Progressbar.IsIndeterminate = false;
        }
    }
}
