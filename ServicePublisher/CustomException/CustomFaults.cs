using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CustomException 
{
    [DataContract]
    public class CustomFaults : Exception
    {
        [DataMember]
        public string ExceptionMessage { get; set; }
        [DataMember]
        public string ExceptionDescription { get; set; }
    }
}
