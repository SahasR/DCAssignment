using Newtonsoft.Json;
using ServiceProvider.Models;
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
        public IHttpActionResult Add(int firstNumber, int secondNumber)
        {
            Result result = new Result();
            result.value = firstNumber + secondNumber;
            return Ok(result);
        }

        [Route("multiply/{firstNumber}/{secondNumber}")]
        [Route("multiply")]
        [HttpGet]
        public IHttpActionResult Multiply(int firstNumber, int secondNumber)
        {
            Result result = new Result();
            result.value = firstNumber * secondNumber;
            return Ok(result);
        }
    }
}
