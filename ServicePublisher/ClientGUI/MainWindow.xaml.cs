using Authenticator;
using CustomException;
using InstanceLibrary;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace ClientGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AuthInterface authenticator; 
        private static string username; private static string password; private static int currToken = 0;
        private static List<Service> services; private Service service; private static string searchstring; private static string apistring;

        public MainWindow()
        {
            InitializeComponent();
            dynamicTestingUI(null);

            //CONNECT TO THE AUTHENTICATOR INTERFACE
            authenticator = Instance.getInterface();

            searchButton.IsEnabled = false;
            getAllButton.IsEnabled = false;
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            username = UserNameBox.Text;
            password = PasswordBox.Text;

            GUIDisable();
            Task<string> task = new Task<string>(AsyncRegister);
            task.Start();
            string result = await task;

            if (result != null)
            {
                System.Windows.Forms.MessageBox.Show(result, "Registration", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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

            if(result != -1) 
            {
                TokenBox.Text = result.ToString();
                searchButton.IsEnabled = true;
                getAllButton.IsEnabled = true;
            }
            else
            {
                TokenBox.Text = "Failed to generate a token";
            }
            GUIEnable();
        }

        private async void searchButton_Click(object sender, RoutedEventArgs e)
        {
            GUIDisable();
            if (SearchBox.Text.Equals("") || SearchBox.Text.Equals(" "))
            {
                System.Windows.Forms.MessageBox.Show("Search can't be empty!", "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
            else
            {
                searchstring = SearchBox.Text;
                Task<List<Service>> task = new Task<List<Service>>(AsyncSearch);
                task.Start();
                List<Service> result = await task;

                if(result != null)
                {
                    listBox.ItemsSource = services;
                }
            }
            GUIEnable();
            searchButton.IsEnabled = true;
            getAllButton.IsEnabled = true;
        }

        private async void getAllButton_Click(object sender, RoutedEventArgs e)
        {
            GUIDisable();
            Task<List<Service>> task = new Task<List<Service>>(AsyncGetAll);
            task.Start();

            //RETRIEVES ALL PUBLISHED SERVICES TO A LIST
            List<Service> result = await task;

            if (result != null)
            {
                listBox.ItemsSource = services;
            }
            GUIEnable();
            searchButton.IsEnabled = true;
            getAllButton.IsEnabled = true;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GUIDisable();
            service = listBox.SelectedItem as Service;

            if(service != null)
            {
                NameBox.Text = service.Name;
                DescriptionBox.Text = service.Description;
                APIBox.Text = service.APIEndpoint;
                NumParamsBox.Text = service.numOperands.ToString();
                ParamTypeBox.Text = service.operandtype;
                dynamicTestingUI(service);
            } 
            else
            {
                NameBox.Text = "";
                DescriptionBox.Text = "";
                APIBox.Text = "";
                NumParamsBox.Text = "";
                ParamTypeBox.Text = "";
                dynamicTestingUI(service);
            }
            GUIEnable();
            searchButton.IsEnabled = true;
            getAllButton.IsEnabled = true;
        }

        private async void testButton_Click(object sender, RoutedEventArgs e)
        {
            GUIDisable();
            apistring = APIBox.Text;
            Task<string> task = new Task<string>(AsyncTest);
            task.Start();
            string result = await task;
            
            if(result != null)
            {
                returnValue.Text = result;
            }
            GUIEnable();
            searchButton.IsEnabled = true;
            getAllButton.IsEnabled = true;
        }

        private void dynamicTestingUI(Service service)
        {
            //ENABLES TEXT BOXES ACCORDING TO THE NUMBER OF PERANDS REQUIRED BY THE SERVICE
            testButton.IsEnabled = false;
            System.Windows.Controls.TextBox[] array = { inputBox1, inputBox2, inputBox3, inputBox4, inputBox5, inputBox6, inputBox7, inputBox8, inputBox9, inputBox10 };
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

        //DISABLES GUI ELEMENTS IN THE CLIENT
        private void GUIDisable()
        {
            UserNameBox.IsReadOnly = true;
            PasswordBox.IsReadOnly = true;
            SearchBox.IsReadOnly = true;
            RegisterButton.IsEnabled = false;
            LoginButton.IsEnabled = false;
            searchButton.IsEnabled = false;
            getAllButton.IsEnabled = false;
            Progressbar.IsIndeterminate = true;
        }

        //ENABLES GUI ELEMENTS IN THE CLIENT
        private void GUIEnable()
        {
            UserNameBox.IsReadOnly = false;
            PasswordBox.IsReadOnly = false;
            SearchBox.IsReadOnly = false;
            RegisterButton.IsEnabled = true;
            LoginButton.IsEnabled = true;
            Progressbar.IsIndeterminate = false;
        }

        //ASYNCHRONOUS THREADING TASKS 
        private int AsyncLogin()
        {
            try
            {
                int token = authenticator.Login(username, password);
                currToken = token;
                return token;
            }
            catch (EndpointNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("Authenticator Services seems down", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (FaultException<AuthenticatorFaults> error)
            {
                System.Windows.Forms.MessageBox.Show(error.Detail.ExceptionMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return -1;
        }

        private string AsyncRegister()
        {
            string validation = null;

            try
            {
                validation = authenticator.Register(username, password);
            }
            catch (EndpointNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show("Authenticator Services seems down", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (FaultException<AuthenticatorFaults> error)
            {
                System.Windows.Forms.MessageBox.Show(error.Detail.ExceptionMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return validation;
        }

        private List<Service> AsyncSearch()
        {
            services = null;
            RestClient restClient = new RestClient("http://localhost:57446/");
            RestRequest restRequest = new RestRequest("Registry/search/" + currToken + "/" + searchstring);
            RestResponse restResponse = restClient.Get(restRequest);

            if (restResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                System.Windows.Forms.MessageBox.Show("Token invalid (might be expired), login again", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (restResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                System.Windows.Forms.MessageBox.Show("No Results", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (restResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                services = JsonConvert.DeserializeObject<List<Service>>(restResponse.Content);
            }

            return services;
        }

        private List<Service> AsyncGetAll()
        {
            services = null;
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
                }
            }
            catch (HttpRequestException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return services;
        }

        private string AsyncTest()
        {
            string returnString = null; string text = null;

            System.Windows.Controls.TextBox[] array = { inputBox1, inputBox2, inputBox3, inputBox4, inputBox5, inputBox6, inputBox7, inputBox8, inputBox9, inputBox10 };

            if (service != null)
            {
                var uri = new Uri(apistring);
                string apiClient = uri.Authority;
                apiClient = "http://" + apiClient + "/";
                string path = uri.AbsolutePath;
                path = path.Remove(0, 1);

                if (!path.EndsWith("/"))
                {
                    path = path + "/";
                }

                path = path + currToken.ToString() + "/";

                try
                {
                    //CHECKS WHETHER USER INPUT IS VALID AND ADDS THEM TO THE URL
                    for (int i = 0; i < service.numOperands; i++)
                    {
                        array[i].Dispatcher.Invoke(new Action(() => text = array[i].Text));
                        if (service.operandtype.Equals("decimal"))
                        {
                            decimal tempChecker = Convert.ToDecimal(text);
                        } 
                        else
                        {
                            int tempChecker = Convert.ToInt32(text);
                        }
                        
                        path += text.ToString() + "/";
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
                                returnString = response.ToString();
                                return returnString;
                            }
                            else
                            {
                                double reponse = JsonConvert.DeserializeObject<DoubleResult>(restResponse.Content).Value;
                                returnString = reponse.ToString();
                                return returnString;
                            }
                        }
                    }
                    catch (HttpRequestException)
                    {
                        System.Windows.Forms.MessageBox.Show("Token invalid (might be expired), login again", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                } 
                catch (FormatException)
                {
                    System.Windows.Forms.MessageBox.Show("Please add the relevant operand types", "You thought.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }   
            }

            return returnString;
        }
    }
}
