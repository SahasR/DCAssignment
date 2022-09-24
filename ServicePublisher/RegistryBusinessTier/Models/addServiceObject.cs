using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistryBusinessTier.Models
{
    //When sending a ServiceObject you need to also send a Token so we wrap them together
    public class addServiceObject
    {
        public int token { get; set; }
        public Service service { get; set; }
    }
}