using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using CustomException;
using System.Threading.Tasks;
using RestSharp;
using static System.Net.WebRequestMethods;

namespace ServicePublisher
{
    internal class Services
    {
        private int token; private static string URL = "http://localhost:57446/";
        public string Registration(string userName, string password)
        {
            return "Ok";
        }

        public int Login(string userName, string password)
        {
            return GetToken();
        }

        public void Publish(string service, string description, string endpoint, string operandNum, string operandType)
        {
            bool isValid; int numOperands;
            try
            {
                //CHECK IF USER INPUT CAN BE CONVERTED TO AN INTEGER
                numOperands = Int32.Parse(operandNum);

                //CHECK IF THE USER INPUT OPERAND TYPE IS EITHER INTEGER OR DOUBLE
                isValid = operandType.ToLower().Equals("integer") || operandType.ToLower().Equals("double");

                if(!isValid)
                {
                    CustomFaults error = new CustomFaults
                    {
                        ExceptionMessage = "Operand types can only be either integer or double",
                        ExceptionDescription = "Error thrown in Publish - Service Publisher"
                    };
                    throw error;
                }

                //CHECK IF THE URL ENTERED BY THE USER IS ACTUALLY A VALID URL
                isValid = CheckURLValid(endpoint);

                if (isValid)
                {
                    RestClient client = new RestClient(URL);
                    RestRequest request = new RestRequest("ADD REGISTRY PUBLISH SERVICE METHOD", Method.Post);
                    RestResponse response = client.Execute(request);

                    if(response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        CustomFaults error = new CustomFaults
                        {
                            ExceptionMessage = "Failed to publish service",
                            ExceptionDescription = "Error thrown in Publish - Service Publisher"
                        };
                        throw error;
                    }

                }else{

                    CustomFaults error = new CustomFaults
                    {
                        ExceptionMessage = "Not a valid base URL format\n eg:- http://localhost:63278/",
                        ExceptionDescription = "Error thrown in Publish - Service Publisher"
                    };
                    throw error;
                }

            }catch(FormatException exception){

                CustomFaults error = new CustomFaults
                {
                    ExceptionMessage = "Input type for number of operands must be an integer",
                    ExceptionDescription = "Error thrown in Publish - Service Publisher" + exception.Message
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
