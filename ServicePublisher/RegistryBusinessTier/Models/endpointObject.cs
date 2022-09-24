using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistryBusinessTier.Models
{
    //endpoint object to send a enpoint + token to the BusinessTier
    public class endpointObject
    {
        public int token { get; set; }
        public EndPoint endpoint { get; set; }
    }
}