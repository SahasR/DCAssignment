using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Authenticator
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class AuthInterfaceImpl : AuthInterface
    {
        public AuthInterfaceImpl() { }

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

            using (var reader = new StreamReader(@"C:\AuthInfo.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
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
            using (var reader = new StreamWriter(@"C:\AuthInfo.csv"))
            {

            }
        }
    }
}
