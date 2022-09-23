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
    [RoutePrefix("ThreeNumbers")]
    public class ThreeNumbersController : ApiController
    {
        RestClient restClient = new RestClient("http://localhost:56201/");

        [Route("add/{token}/{firstNumber}/{secondNumber}/{thirdNumber}")]
        [Route("add")]
        [HttpGet]
        public IHttpActionResult Add(int token, int firstNumber, int secondNumber, int thirdNumber)
        {
            if (!checkToken(token))
            {
                BadToken badToken = new BadToken();
                return Content(HttpStatusCode.Unauthorized, badToken);
            }
            else
            {
                RestRequest request = new RestRequest("ThreeNumbers/add/" + firstNumber.ToString() + "/" + secondNumber.ToString() + "/" + thirdNumber.ToString());
                RestResponse response = restClient.Get(request);
                Result result = JsonConvert.DeserializeObject<Result>(response.Content);
                return Content(HttpStatusCode.OK, result);
            }
        }

        [Route("multiply/{token}/{firstNumber}/{secondNumber}/{thirdNumber}")]
        [Route("multiply")]
        [HttpGet]
        public IHttpActionResult Multiply(int token, int firstNumber, int secondNumber, int thirdNumber)
        {
            if (!checkToken(token))
            {
                BadToken badToken = new BadToken();
                return Content(HttpStatusCode.Unauthorized, badToken);
            }
            else
            {
                RestRequest request = new RestRequest("ThreeNumbers/multiply/" + firstNumber.ToString() + "/" + secondNumber.ToString() + "/" + thirdNumber.ToString());
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
