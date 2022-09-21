using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceProviderBusinessTier.Models
{
    public class BadToken
    {
        public string Status = "Denied";
        public string Reason = "Authentication Error";
    }
}