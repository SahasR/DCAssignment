using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CustomException;

namespace Authenticator
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class AuthInterfaceImpl : AuthInterface
    {
        private static AuthInterfaceImpl instance = null; private static string myfile; private static string projectDirectory;
        private AuthInterfaceImpl()
        {
            /* SPECIFYING A FILE PATH */
            projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            myfile = projectDirectory.Substring(0, projectDirectory.IndexOf("ServicePublisher"));
            myfile += "Information.txt";

            using (StreamWriter sw = File.CreateText(myfile)){}
        }

        /* Since the authenticator is going to be called by multiple services all accessing the same local text file 
         * we made the authenticator a singleton */
        public static AuthInterfaceImpl getInstance()
        {
            if (instance == null)
            {
                instance = new AuthInterfaceImpl();
            }
            return instance;
        }

        /**/
        public String Register(String name, String password)
        {
            WriteFile(name, password);
            return "Successfully registered!";
        }

        public int Login(String name, String password)
        {
            if (ReadFile(name, password))
            {
                Random random = new Random();
                int number = random.Next(10000000, 99999999); 
                return number;
            }

            /*EXCEPTION*/
            return -1;
        }

        public String Validate(int token)
        {
            return "Validated";
        }

        private bool ReadFile(String name, String password)
        {
            bool isFound = false;

            using (StreamReader sr = File.OpenText(myfile))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var values = line.Split(',');

                    if (name.Equals(values[0]) && password.Equals(values[1]))
                    {
                        isFound = true;
                        break;

                    }
                }
            }
            return isFound;
        }

        /*SAVES NAME AND PASSWORD INTO A .CSV FILE*/
        private void WriteFile(String name, String password)
        {
            using (StreamWriter sw = File.AppendText(myfile))
            {
               
            }
        }
    }
}
