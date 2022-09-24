using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistryBusinessTier.Models
{
    //Service object passed back from the RegistryAPI to the client requesting it.
    public class Service
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string APIEndpoint { get; set; }
        public int numOperands { get; set; }
        public string operandtype { get; set; }
    }
}