using CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Authenticator
{
    [ServiceContract]
    public interface AuthInterface
    {
        [OperationContract]
        String Register(String name, String password);

        [OperationContract]
        int Login(String name, String Password);

        [OperationContract]
        String Validate(int token);
    }
}
