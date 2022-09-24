using ServiceProvider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServiceProvider.Controllers
{
    [RoutePrefix("Double")]
    public class DoubleController : ApiController
    {
        [Route("add/{firstNumber}/{secondNumber}/{thirdNumber}")]
        [Route("add")]
        [HttpGet]
        public IHttpActionResult Add(double firstNumber, double secondNumber, double thirdNumber)
        {
            DoubleResult result = new DoubleResult();
            result.value = firstNumber + secondNumber + thirdNumber;
            return Ok(result);
        }
    }
}
