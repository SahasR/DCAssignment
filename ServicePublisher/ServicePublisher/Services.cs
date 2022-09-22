using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using CustomException;
using System.Threading.Tasks;
using RestSharp;

namespace ServicePublisher
{
    internal class Services
    {
        private int token; private static string URL = "";
        public string Registration(string userName, string password)
        {
            return "Ok";
        }

        public int Login(string userName, string password)
        {
            return GetToken();
        }

        public void Publish(string service, string description, string endpoint, int operands)
        {
            bool isValid = CheckURLValid(endpoint);

            if(isValid)
            {
                RestClient client = new RestClient(URL);
                RestRequest request = new RestRequest("ADD REGISTRY PUBLISH SERVICE METHOD", Method.Delete);
                RestResponse response = client.Execute(request);

                if(response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    
                }

            }else{

                CustomFaults error = new CustomFaults
                {
                    ExceptionMessage = "Not a valid base URL format\n eg:- http://localhost:63278/",
                    ExceptionDescription = "Error thrown in Publish - Service Publisher"
                };
                throw error;
            }
        }

        public void Unpublish(string endpoint)
        {
            bool isValid = CheckURLValid(endpoint);

            if (isValid)
            {
                RestClient client = new RestClient(URL);
                RestRequest request = new RestRequest("ADD REGISTRY UNPUBLISH SERVICE METHOD", Method.Delete);
                RestResponse response = client.Execute(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    // throw error
                }

            }else{

                CustomFaults error = new CustomFaults
                {
                    ExceptionMessage = "Not a valid base URL format eg:- http://localhost:63278/",
                    ExceptionDescription = "Error thrown in Unpublish - Service Publisher"
                };
                throw error;
            }
        }

        //GETTERS AND SETTERS FOR THE TOKEN
        public int GetToken()
        {
            return token;
        }

        public void SetToken(int pToken)
        {
            token = pToken;
        }

        //CHECK IF THE INPUT STRING CONFORMS TO URL STYLE
        private bool CheckURLValid(string source)
        {
            Uri uriResult;
            return Uri.TryCreate(source, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
        }
    }
}
