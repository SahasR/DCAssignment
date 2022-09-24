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

//BusinessTier for the ThreeNumber Controller in the ServiceProvider
namespace ServiceProviderBusinessTier.Controllers
{
    [RoutePrefix("ThreeNumbers")]
    public class ThreeNumbersController : ApiController
    {
        RestClient restClient = new RestClient("http://localhost:56201/");
        //Simple addition of three numbers
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
                IntResult result = JsonConvert.DeserializeObject<IntResult>(response.Content);
                return Content(HttpStatusCode.OK, result);
            }
        }
        //Simple multiplication of three numbers
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
                IntResult result = JsonConvert.DeserializeObject<IntResult>(response.Content);
                return Content(HttpStatusCode.OK, result);
            }
        }

        //Checks for a valid token
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
