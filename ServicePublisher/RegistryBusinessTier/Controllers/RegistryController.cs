using Newtonsoft.Json;
using RegistryBusinessTier.Models;
using RestSharp;
using Authenticator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EndPoint = RegistryBusinessTier.Models.EndPoint;
using System.ServiceModel;
using System.Diagnostics;
using InstanceLibrary;

namespace RegistryBusinessTier.Controllers
{
    [RoutePrefix("Registry")]
    public class RegistryController : ApiController
    {
        RestClient restClient = new RestClient("http://localhost:64696/");

        [HttpPost]
        [Route("publish")]
        public IHttpActionResult publish(addServiceObject serviceObject)
        {
            if(checkToken(serviceObject.token))
            {
                Service service = serviceObject.service;
                RestRequest restRequest = new RestRequest("Registry/publish");
                restRequest.AddBody(service);
                RestResponse restResponse = restClient.Post(restRequest);

                if(restResponse.IsSuccessStatusCode)
                {
                    return Ok();

                }else{

                    return NotFound();
                }

            }else{

                BadToken badToken = new BadToken();
                return Content(HttpStatusCode.Unauthorized, badToken);
            }
        }

        [HttpGet]
        [Route("search/{token}/{searchText}")]
        [Route("search")]
        public IHttpActionResult search(int token, string searchText)
        {
            if(checkToken(token))
            {
                RestRequest restRequest = new RestRequest("Registry/search/" + searchText);
                RestResponse restResponse = restClient.Get(restRequest);

                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    List<Service> service = JsonConvert.DeserializeObject<List<Service>>(restResponse.Content);
                    return Ok(service);

                }else{

                    return NotFound();
                }

            }else{
                
                BadToken badToken = new BadToken();
                return Content(HttpStatusCode.Unauthorized, badToken);
            }
        }

       [HttpGet]
       [Route("getall/{token}")]
       [Route("getall")]
       public IHttpActionResult getAll(int token)
       {
          if(checkToken(token))
          {
                RestRequest restRequest = new RestRequest("Registry/getall");
                RestResponse restResponse = restClient.Get(restRequest);
                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    List<Service> serviceList = JsonConvert.DeserializeObject<List<Service>>(restResponse.Content);
                    return Ok(serviceList);
                } else
                {
                    return NotFound();
                }

          }else{

            BadToken badToken = new BadToken();
            return Content(HttpStatusCode.Unauthorized, badToken);
          }
       }

        [HttpDelete]
        [Route("delete")]
        public IHttpActionResult Delete(endpointObject epointObject)
        {
            if (checkToken(epointObject.token))
            {
                EndPoint endpoint = epointObject.endpoint;
                RestRequest restRequest = new RestRequest("Registry/delete");
                restRequest.AddBody(endpoint);
                RestResponse restResponse = restClient.Delete(restRequest);

                if (restResponse.IsSuccessStatusCode)
                {
                    return Ok();

                }else{

                    return NotFound();
                }

            }else{

              BadToken badToken = new BadToken();
              return Content(HttpStatusCode.Unauthorized, badToken);
            }
        }

        private Boolean checkToken(int token)
        {
            AuthInterface foob = Instance.getInterface();
      
            if (foob.Validate(token).Equals("Validated"))
            {
                return true;

            }else{

                return false;
            }
            
        }
    }
}
