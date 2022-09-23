using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Authenticator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool timerSet = false; int timeout = 0;

            while (!timerSet)
            {
                /* QUERY USER FOR THE NO.OF MINUTES FOR CLEAN-UP */
                Console.Write("Enter desired number of minutes for a periodical clean-up: ");
                string input = Console.ReadLine();
                
                try
                {
                    timeout = Int32.Parse(input);
                    if (timeout > 0.0)
                    {
                        timerSet = true;

                    }else{ Console.WriteLine("Time has to be greater than zero!"); }

                }catch (FormatException error) { Console.WriteLine(error.Message); }
            }

            AuthInterfaceImpl impl = AuthInterfaceImpl.getInstance();
           
            //CLEARING TOKENS USING MULTI THREADING

            //SETTING UP THE AUTHENTICATOR
            NetTcpBinding tcp = new NetTcpBinding();
            ServiceHost host = new ServiceHost(typeof(AuthInterfaceImpl));
            host.AddServiceEndpoint(typeof(AuthInterface), tcp, "net.tcp://0.0.0.0:8100/AuthenticatorService");
            host.Open();
            Console.WriteLine("Authenticator is Online");
            Console.ReadLine();
            host.Close();
        }
    }
}
