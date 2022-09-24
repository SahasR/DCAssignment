using Newtonsoft.Json;
using ServiceProvider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;

//As per assignment specification a controller to have functionality for TwoNumbers
namespace ServiceProvider.Controllers
{
    [RoutePrefix("TwoNumbers")]
    public class TwoNumbersController : ApiController
    {
        //Simple function to add two numbers
        [Route("add/{firstNumber}/{secondNumber}")]
        [Route("add")]
        [HttpGet]
        public IHttpActionResult Add(int firstNumber, int secondNumber)
        {
            IntResult result = new IntResult();
            result.value = firstNumber + secondNumber;
            return Ok(result);
        }

        //simple function to multiply two numnbers
        [Route("multiply/{firstNumber}/{secondNumber}")]
        [Route("multiply")]
        [HttpGet]
        public IHttpActionResult Multiply(int firstNumber, int secondNumber)
        {
            IntResult result = new IntResult();
            result.value = firstNumber * secondNumber;
            return Ok(result);
        }
    }
}
