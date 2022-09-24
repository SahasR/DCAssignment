using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CustomException
{
    //FAULT EXCEPTIONS THROWN BY THE AUTHENTICATOR
    [DataContract]
    public class AuthenticatorFaults
    {
        private string exceptionMessage;
        private string exceptionDescription;

        [DataMember]
        public string ExceptionMessage
        {
            get { return exceptionMessage;  }
            set { exceptionMessage = value; }
        }

        [DataMember]
        public string ExceptionDescription
        {
            get { return exceptionDescription; }
            set { exceptionDescription = value; }
        }
    }
}
