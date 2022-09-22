using Newtonsoft.Json;
using Registry.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace Registry.Controllers
{
    [RoutePrefix("Registry")]
    public class RegistryController : ApiController
    {
        private string registerFile; 
        private string projectDirectory;
      
        [HttpPost]
        [Route("publish")]
        public IHttpActionResult publish(Service service)
        {
            List<Service> services;
            projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            registerFile = projectDirectory + "\\" + "services.txt";

            string data = "";
            if (File.Exists(registerFile))
            {
                string line = "";
                using (StreamReader sr = new StreamReader(registerFile))
                {
                    while((line = sr.ReadLine()) != null)
                    {
                        data += line;
                    }
                }
                services = JsonConvert.DeserializeObject<List<Service>>(data);
            } else
            {
                services = new List<Service>();
            }

            services.Add(service);

            using (StreamWriter sw = File.CreateText(registerFile))
            {
                string json = JsonConvert.SerializeObject(services, Formatting.Indented);
                sw.WriteLine(json);
             
            }
            return Ok(service);
        }

        [HttpGet]
        [Route("search/{searchText}")]
        [Route("search")]
        public IHttpActionResult search(string searchText)
        {
            List<Service> services;
            List<Service> returnList = new List<Service>();
            projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            registerFile = projectDirectory + "\\" + "services.txt";

            if (!File.Exists(registerFile))
            {
                return NotFound();
            } else
            {
                string data = "";
                string line = "";
                using (StreamReader sr = new StreamReader(registerFile))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        data += line;
                    }
                }
                services = JsonConvert.DeserializeObject<List<Service>>(data);
                foreach (Service service in services)
                {
                    if (service.Name.ToLower().Contains(searchText.ToLower()))
                    {
                        returnList.Add(service);
                    }
                }
                if (returnList.Count > 0)
                {
                    return Ok(returnList);
                } else
                {
                    return NotFound();
                }
            }
        }
    }
}
