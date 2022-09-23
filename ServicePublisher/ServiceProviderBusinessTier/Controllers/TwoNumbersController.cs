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
    [RoutePrefix("TwoNumbers")]
    public class TwoNumbersController : ApiController
    {
        RestClient restClient = new RestClient("http://localhost:56201/");

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
                Result result = JsonConvert.DeserializeObject<Result>(response.Content);
                return Content(HttpStatusCode.OK, result);
            }
        }

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
                Result result = JsonConvert.DeserializeObject<Result>(response.Content);
                return Content(HttpStatusCode.OK, result);
            }
        }

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
