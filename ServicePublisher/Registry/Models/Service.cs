using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Registry.Models
{
    //Service Object that is passed back from the Registrys
    public class Service
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string APIEndpoint { get; set; }
        public int numOperands { get; set; }
        public string operandtype { get; set; }

    }
}