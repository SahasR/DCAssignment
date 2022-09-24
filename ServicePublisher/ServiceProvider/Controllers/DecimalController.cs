using ServiceProvider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServiceProvider.Controllers
{
    [RoutePrefix("Decimal")]
    public class DecimalController : ApiController
    {
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
