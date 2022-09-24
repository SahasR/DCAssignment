using Authenticator;
using InstanceLibrary;
using Newtonsoft.Json;
using RestSharp;
using ServiceProviderBusinessTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;

namespace ServiceProviderBusinessTier.Controllers
{
    //BusinessLayer for API for two variable controller
    [RoutePrefix("TwoNumbers")]
    public class TwoNumbersController : ApiController
    {
        RestClient restClient = new RestClient("http://localhost:56201/");
        //Simple addition between two numbers
        [Route("add/{token}/{firstNumber}/{secondNumber}")]
        [Route("add")]
        [HttpGet]
        public IHttpActionResult Add(int token, int firstNumber, int secondNumber)
        {
            if (!checkToken(token))
            {
                BadToken badToken = new BadToken();
                return Content(HttpStatusCode.Unauthorized, badToken);
            } else
            {
                RestRequest request = new RestRequest("TwoNumbers/add/" + firstNumber.ToString() + "/" + secondNumber.ToString());
                RestResponse response = restClient.Get(request);
                IntResult result = JsonConvert.DeserializeObject<IntResult>(response.Content);
                return Content(HttpStatusCode.OK, result);
            }
        }

        //Simple multiplication between two numbers
        [Route("multiply/{token}/{firstNumber}/{secondNumber}")]
        [Route("multiply")]
        [HttpGet]
        public IHttpActionResult Multiply(int token, int firstNumber, int secondNumber)
        {
            if (!checkToken(token))
            {
                BadToken badToken = new BadToken();
                return Content(HttpStatusCode.Unauthorized, badToken);
            } else
            {
                RestRequest request = new RestRequest("TwoNumbers/multiply/" + firstNumber.ToString() + "/" + secondNumber.ToString());
                RestResponse response = restClient.Get(request);
                IntResult result = JsonConvert.DeserializeObject<IntResult>(response.Content);
                return Content(HttpStatusCode.OK, result);
            }
        }

        //Checks if the token passed through is true
        private Boolean checkToken(int token)
        {
            AuthInterface foob = Instance.getInterface();
            if (foob.Validate(token).Equals("Validated")){
                return true;
            } else
            {
                return false;
            }
        }
    }
}
