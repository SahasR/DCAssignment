using Authenticator;
using InstanceLibrary;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
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
        private static int currToken = 0;
        private static List<Service> services; private Service service;
        public MainWindow()
        {
            InitializeComponent();
            dynamicTestingUI(null);
            authenticator = Instance.getInterface();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            username = UserNameBox.Text;
            password = PasswordBox.Text;

            GUIDisable();

            Task<string> task = new Task<string>(AsyncRegister);
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

            Task<int> task = new Task<int>(AsyncLogin);
            task.Start();
            int result = await task;

            TokenBox.Text = result.ToString();
            GUIEnable();
        }

        private int AsyncLogin()
        {
            int token = authenticator.Login(username, password);
            currToken = token;
            return token;
        }

        private string AsyncRegister()
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
            if (SearchBox.Text.Equals("") || SearchBox.Text.Equals(" "))
            {
                System.Windows.Forms.MessageBox.Show("Search can't be empty!", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else
            {
                RestClient restClient = new RestClient("http://localhost:57446/");
                RestRequest restRequest = new RestRequest("Registry/search/" + currToken + "/" + SearchBox.Text);
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
                    services = JsonConvert.DeserializeObject<List<Service>>(restResponse.Content);
                    listBox.ItemsSource = services;
                }
            }
            
        }

        private void getAllButton_Click(object sender, RoutedEventArgs e)
        {
            RestClient restClient = new RestClient("http://localhost:57446/");
            RestRequest restRequest = new RestRequest("Registry/getall/" + currToken);

            try
            {
                RestResponse restResponse = restClient.Get(restRequest);
                if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    System.Windows.Forms.MessageBox.Show("API Service Seems to be down", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (restResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    services = JsonConvert.DeserializeObject<List<Service>>(restResponse.Content);
                    listBox.ItemsSource = services;
                }
            } catch (HttpRequestException)
            {
                System.Windows.Forms.MessageBox.Show("Token invalid (might be expired), login again", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            service = listBox.SelectedItem as Service;
            if (service != null)
            {
                NameBox.Text = service.Name;
                DescriptionBox.Text = service.Description;
                APIBox.Text = service.APIEndpoint;
                NumParamsBox.Text = service.numOperands.ToString();
                ParamTypeBox.Text = service.operandtype;
                dynamicTestingUI(service);
            } else
            {
                NameBox.Text = "";
                DescriptionBox.Text = "";
                APIBox.Text = "";
                NumParamsBox.Text = "";
                ParamTypeBox.Text = "";
                dynamicTestingUI(service);
            }
        }

        private void dynamicTestingUI(Service service)
        {
            testButton.IsEnabled = false;
            System.Windows.Controls.TextBox[] array = {inputBox1, inputBox2, inputBox3, inputBox4, inputBox5, inputBox6, inputBox7, inputBox8, inputBox9, inputBox10};
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Visibility = Visibility.Hidden;
            }

            if (service != null)
            {
                testButton.IsEnabled = true;
                for (int i = 0; i < service.numOperands; i++)
                {
                    array[i].Visibility = Visibility.Visible;
                }
            } 
        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox[] array = { inputBox1, inputBox2, inputBox3, inputBox4, inputBox5, inputBox6, inputBox7, inputBox8, inputBox9, inputBox10 };
            if (service != null)
            {
                var uri = new Uri(APIBox.Text);
                string apiClient = uri.Authority;
                apiClient = "http://" + apiClient + "/";
                string path = uri.AbsolutePath;
                path = path.Remove(0, 1);

                if (!path.EndsWith("/"))
                {
                    path = path + "/";
                }

                path = path + currToken.ToString() + "/";

                for (int i = 0; i < service.numOperands; i++)
                {
                    path += array[i].Text.ToString() + "/";
                }

                RestClient restClient = new RestClient(apiClient);
                RestRequest restRequest = new RestRequest(path);


                try
                {
                    RestResponse restResponse = restClient.Get(restRequest);
                    if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        System.Windows.Forms.MessageBox.Show("Function not found!", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (restResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (service.operandtype.Equals("integer"))
                        {
                            int response = JsonConvert.DeserializeObject<IntResult>(restResponse.Content).Value;
                            returnValue.Text = response.ToString();
                        } else
                        {
                            double reponse = JsonConvert.DeserializeObject<DoubleResult>(restResponse.Content).Value;
                            returnValue.Text = reponse.ToString();
                        }
                    }
                }
                catch (HttpRequestException)
                {
                    System.Windows.Forms.MessageBox.Show("Token invalid (might be expired), login again", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }
    }
}
