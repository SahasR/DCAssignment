using Authenticator;
using InstanceLibrary;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        private static int currToken;
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

            Task<string> task = new Task<string>(Register);
            task.Start();
            string result = await task;
            System.Windows.Forms.MessageBox.Show(result, "Registration", MessageBoxButtons.OK, MessageBoxIcon.Information);

            GUIEnable();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            username = UserNameBox.Text;
            password = PasswordBox.Text;

            GUIDisable();

            Task<int> task = new Task<int>(Login);
            task.Start();
            int result = await task;

            TokenBox.Text = result.ToString();
            GUIEnable();
        }

        private int Login()
        {
            int token = authenticator.Login(username, password);
            currToken = token;
            return token;
        }

        private string Register()
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

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            RestClient restClient = new RestClient("http://localhost:57446/");
            RestRequest restRequest = new RestRequest("Registry/search/" + currToken + "/" + SearchBox.Text);
            RestResponse restResponse = restClient.Get(restRequest);

            if (restResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                System.Windows.Forms.MessageBox.Show("Token invalid (might be expired), login again", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                System.Windows.Forms.MessageBox.Show("API Service Seems to be down", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else if (restResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.Windows.Forms.MessageBox.Show("Worked!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void getAllButton_Click(object sender, RoutedEventArgs e)
        {
            RestClient restClient = new RestClient("http://localhost:57446/");
            RestRequest restRequest = new RestRequest("Registry/getall/" + currToken);
            RestResponse restResponse = restClient.Get(restRequest);

            if (restResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                System.Windows.Forms.MessageBox.Show("Token invalid (might be expired), login again", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                System.Windows.Forms.MessageBox.Show("API Service Seems to be down", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (restResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.Windows.Forms.MessageBox.Show("Worked!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
