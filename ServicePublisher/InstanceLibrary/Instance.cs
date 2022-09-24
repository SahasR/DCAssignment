using Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace InstanceLibrary
{
    public class Instance
    {
        private static AuthInterface authInterface;

        //CREATES A SINGLE INSTANCE OF AN AUTHENTICATOR INTERFACE THROUGH WHICH SERVICES MAY CONNECT
        public static AuthInterface getInterface()
        {
            ChannelFactory<AuthInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/AuthenticatorService";
            foobFactory = new ChannelFactory<AuthInterface>(tcp, URL);
            authInterface = foobFactory.CreateChannel();
            return authInterface;
        }
    }
   
}
