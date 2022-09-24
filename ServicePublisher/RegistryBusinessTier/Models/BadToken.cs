using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistryBusinessTier.Models
{
    //Incase the token has expired we send a BadToken object back to the user.
    public class BadToken
    {
        public string Status = "Denied";
        public string Reason = "Authentication Error";
    }
}