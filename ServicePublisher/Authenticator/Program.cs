using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Authenticator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool timerSet = false; ServiceHost host;

            while (!timerSet)
            {
                /* QUERY USER FOR THE NO.OF MINUTES FOR CLEAN-UP */
                Console.Write("Enter desired number of minutes for periodical clean-up: ");
                string input = Console.ReadLine();
                try
                {
                    AuthInterfaceImpl impl = AuthInterfaceImpl.getInstance();
                    int timeout = Int32.Parse(input);
                    if (timeout > 0)
                    {
                        timerSet = true;

                    }else{ Console.WriteLine("Time has to be greater than zero!"); }

                }catch (FormatException error) { Console.WriteLine(error.Message); }
            }

            NetTcpBinding tcp = new NetTcpBinding();
            host = new ServiceHost(typeof(AuthInterfaceImpl));
            host.AddServiceEndpoint(typeof(AuthInterface), tcp, "net.tcp://0.0.0.0:8100/AuthenticatorService");
            host.Open();
            Console.WriteLine("Authenticator is Online");
            Console.ReadLine();
            host.Close();
        }
    }
}
