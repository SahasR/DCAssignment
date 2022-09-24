using Newtonsoft.Json;
using ServiceProvider.Models;
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
        public IHttpActionResult Add(int firstNumber, int secondNumber, int thirdNumber)
        {
            IntResult result = new IntResult();
            result.value = firstNumber + secondNumber + thirdNumber;
            return Ok(result);
        }

        [Route("multiply/{firstNumber}/{secondNumber}/{thirdNumber}")]
        [Route("multiply")]
        [HttpGet]
        public IHttpActionResult Multiply(int firstNumber, int secondNumber, int thirdNumber)
        {
            IntResult result = new IntResult();
            result.value = firstNumber * secondNumber * thirdNumber;
            return Ok(result);
        }
    }
}
