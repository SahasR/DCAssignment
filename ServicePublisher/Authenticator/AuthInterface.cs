using CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Authenticator
{
    [ServiceContract]
    internal interface AuthInterface
    {
        [OperationContract]
        [FaultContract(typeof(CustomFaults))]
        String Register(String name, String password);

        [OperationContract]
        [FaultContract(typeof(CustomFaults))]
        int Login(String name, String Password);

        [OperationContract]
        [FaultContract(typeof(CustomFaults))]
        String Validate(int token);
    }
}
