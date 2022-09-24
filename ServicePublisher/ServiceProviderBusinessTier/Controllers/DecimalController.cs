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
using System.Web.Http;

namespace ServiceProviderBusinessTier.Controllers
{
    //Business Tier to handle decimal values
    [RoutePrefix("Decimal")]
    public class DecimalController : ApiController
    {
        RestClient restClient = new RestClient("http://localhost:56201/");
        //Simple decimal addition between three numbers
        [Route("add/{token}/{firstNumber}/{secondNumber}/{thirdNumber}")]
        [Route("add")]
        [HttpGet]
        public IHttpActionResult Add(int token, decimal firstNumber, decimal secondNumber, decimal thirdNumber)
        {
            if (!checkToken(token))
            {
                BadToken badToken = new BadToken();
                return Content(HttpStatusCode.Unauthorized, badToken);
            }
            else
            {
                RestRequest request = new RestRequest("Decimal/add/" + firstNumber.ToString() + "/" + secondNumber.ToString() + "/" + thirdNumber.ToString() + "/");
                RestResponse response = restClient.Get(request);
                DoubleResult result = JsonConvert.DeserializeObject<DoubleResult>(response.Content);
                return Content(HttpStatusCode.OK, result);
            }
        }

        //Checks for a valid token
        private Boolean checkToken(int token)
        {
            AuthInterface foob = Instance.getInterface();
            if (foob.Validate(token).Equals("Validated"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
