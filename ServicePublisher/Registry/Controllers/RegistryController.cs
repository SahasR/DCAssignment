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
        [Route("add")]
        public IHttpActionResult Add(Service service)
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
    }
}
