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
        [FaultContract(typeof(AuthenticatorFaults))]
        String Register(String name, String password);

        [OperationContract]
        [FaultContract(typeof(AuthenticatorFaults))]
        int Login(String name, String Password);

        [OperationContract]
        String Validate(int token);
    }
}
