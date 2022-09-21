using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServiceProvider.Controllers
{
    [RoutePrefix("ThreeNumbers")]
    public class ThreeNumbersController : ApiController
    {
        [Route("add/{firstNumber}/{secondNumber}/{thirdNumber}")]
        [Route("add")]
        [HttpGet]
        public int Add(int firstNumber, int secondNumber, int thirdNumber)
        {
            return firstNumber + secondNumber + thirdNumber;
        }

        [Route("multiply/{firstNumber}/{secondNumber}/{thirdNumber}")]
        [Route("multiply")]
        [HttpGet]
        public int Multiply(int firstNumber, int secondNumber, int thirdNumber)
        {
            return firstNumber * secondNumber * thirdNumber;
        }
    }
}
