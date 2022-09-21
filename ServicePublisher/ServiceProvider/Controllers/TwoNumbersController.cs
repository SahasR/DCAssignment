using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;

namespace ServiceProvider.Controllers
{
    [RoutePrefix("TwoNumbers")]
    public class TwoNumbersController : ApiController
    {
        [Route("add/{firstNumber}/{secondNumber}")]
        [Route("add")]
        [HttpGet]
        public int Add(int firstNumber, int secondNumber)
        {
            return firstNumber + secondNumber;
        }

        [Route("multiply/{firstNumber}/{secondNumber}")]
        [Route("multiply")]
        [HttpGet]
        public int Multiply(int firstNumber, int secondNumber)
        {
            return firstNumber * secondNumber;
        }
    }
}
