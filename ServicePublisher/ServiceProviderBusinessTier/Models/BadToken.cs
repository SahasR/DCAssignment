using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceProviderBusinessTier.Models
{
    //Bad token object passed back to the client in case the token doesnt exist or expired.
    public class BadToken
    {
        public string Status = "Denied";
        public string Reason = "Authentication Error";
    }
}