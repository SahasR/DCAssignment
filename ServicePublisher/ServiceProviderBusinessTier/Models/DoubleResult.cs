using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceProviderBusinessTier.Models
{
    //Object passed back to client if the type is decimal, double has additional range
    public class DoubleResult
    {
        public double Value { get; set; }
    }
}