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
        private static System.Threading.Timer clearTimer = null; 
        private static AuthInterfaceImpl impl; private static int timeout;

        static void Main(string[] args)
        {
            bool timerSet = false; 
           
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

                }catch(FormatException error) { Console.WriteLine(error.Message); }
            }

            //TURNING UP THE AUTHENTICATOR
            impl = AuthInterfaceImpl.getInstance();
           
            //START CLEARING TOKENS USING MULTI THREADING
            CreateStartTimer();

            //SETTING UP THE AUTHENTICATOR
            NetTcpBinding tcp = new NetTcpBinding();
            ServiceHost host = new ServiceHost(typeof(AuthInterfaceImpl));
            host.AddServiceEndpoint(typeof(AuthInterface), tcp, "net.tcp://0.0.0.0:8100/AuthenticatorService");
            host.Open();
            Console.WriteLine("Authenticator is Online");
            Console.ReadLine();
            StopTimer();
            host.Close();
        }
        private static void CreateStartTimer()
        {
            TimeSpan InitialInterval = new TimeSpan(0, timeout, 0);
            TimeSpan RegularInterval = new TimeSpan(0, timeout, 0);
            clearTimer = new System.Threading.Timer(ClearFiles, null, InitialInterval, RegularInterval);
        }
        private static void ClearFiles(object state)
        {
            impl.ClearTokens();
            Console.WriteLine("Clearing Tokens...");
        }
        private static void StopTimer()
        {
            clearTimer.Change(System.Threading.Timeout.Infinite,System.Threading.Timeout.Infinite);
            clearTimer.Dispose();
        }
    }
}
