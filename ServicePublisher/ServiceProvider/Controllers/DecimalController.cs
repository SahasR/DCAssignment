using ServiceProvider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServiceProvider.Controllers
{
    //Controller that handles Decimals
    [RoutePrefix("Decimal")]
    public class DecimalController : ApiController
    {
        //Just a simple Decimal Add function to test feasability.
        [Route("add/{firstNumber}/{secondNumber}/{thirdNumber}")]
        [Route("add")]
        [HttpGet]
        public IHttpActionResult Add(decimal firstNumber, decimal secondNumber, decimal thirdNumber)
        {
            DoubleResult result = new DoubleResult();
            result.value = (double)(firstNumber + secondNumber + thirdNumber);
            return Ok(result);
        }
    }
}
