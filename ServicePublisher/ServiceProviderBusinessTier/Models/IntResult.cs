using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceProviderBusinessTier.Models
{
    //Object passed back to the client if its an integer based variable system
    public class IntResult
    {
        public int value { get; set; }
    }
}