﻿using Authenticator;
using CustomException;
using InstanceLibrary;
using Newtonsoft.Json;
using RegistryBusinessTier.Models;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePublisher
{
    internal class Services
    {
        private static int token = -1;
        private static string URL = "http://localhost:57446/";
        static AuthInterface authenticator;
       
        public static void Registration(string userName, string password)
        {
            authenticator = Instance.getInterface();
            string validation = authenticator.Register(userName, password);
            Console.WriteLine(validation);
        }

        public static void Login(string userName, string password)
        {
            authenticator = Instance.getInterface();
            token = authenticator.Login(userName, password);
           
            if (token == -1)
            {
                Console.WriteLine("Login Failed");
            }
            else
            {
                Console.WriteLine("Login Successful. Token: " + token);
            }
        }

        public static void Publish(string serviceName, string description, string endpoint, string operandNum, string operandType)
        {
            bool isValid; int numOperands;
            try
            {
                //CHECK IF USER INPUT CAN BE CONVERTED TO AN INTEGER
                numOperands = Int32.Parse(operandNum);

                //CHECK IF THE USER INPUT OPERAND TYPE IS EITHER INTEGER OR DOUBLE
                isValid = operandType.ToLower().Equals("integer") || operandType.ToLower().Equals("double");
            
                if (!isValid)
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
                    RestRequest request = new RestRequest("Registry/publish", Method.Post);
                    Service service = new Service();

                    //CREATE A NEW SERVICE OBJECT
                    service.Name = serviceName;
                    service.Description = description;
                    service.APIEndpoint = endpoint;
                    service.numOperands = numOperands;
                    service.operandtype = operandType;

                    //ADD TOKEN BEFORE SENDING TO REGISTRY SERVICE
                    addServiceObject serviceObject = new addServiceObject();
                    serviceObject.service = service;
                    serviceObject.token = token;
                  
                    request.AddJsonBody(serviceObject);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        BadToken token = JsonConvert.DeserializeObject<BadToken>(response.Content);
                        Console.WriteLine("Status: " + token.Status + "Reason: " + token.Reason);

                    }else if (response.StatusCode == System.Net.HttpStatusCode.NotFound){

                        Console.WriteLine("Failed to publish service");

                    }else if (response.StatusCode == System.Net.HttpStatusCode.OK){

                        Console.WriteLine("Service successfully published");
                    }

                }
                else
                {

                    CustomFaults error = new CustomFaults
                    {
                        ExceptionMessage = "Not a valid base URL format\n eg:- http://localhost:63278/",
                        ExceptionDescription = "Error thrown in Publish - Service Publisher"
                    };
                    throw error;
                }

            }
            catch (FormatException exception)
            {

                CustomFaults error = new CustomFaults
                {
                    ExceptionMessage = "Input type for number of operands must be an integer",
                    ExceptionDescription = "Error thrown in Publish - Service Publisher" + exception.Message
                };
                throw error;
            }
        }

        public static void Unpublish(string endpoint)
        {
            bool isValid = CheckURLValid(endpoint);

            if (isValid)
            {
                RestClient client = new RestClient(URL);
                RestRequest request = new RestRequest("Registry/delete", Method.Delete);

                EndPoint end = new EndPoint();
                end.Value = endpoint;

                endpointObject endObject = new endpointObject();
                endObject.token = GetToken();
                endObject.endpoint = end;

                request.AddJsonBody(endObject);
                RestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    BadToken token = JsonConvert.DeserializeObject<BadToken>(response.Content);
                    Console.WriteLine("Status: " + token.Status + "Reason: " + token.Reason);

                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {

                    Console.WriteLine("Failed to unpublish service");

                }
                else if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    Console.WriteLine("Service successfully unpublished");
                }

            }
            else
            {

                CustomFaults error = new CustomFaults
                {
                    ExceptionMessage = "Not a valid base URL format eg:- http://localhost:63278/",
                    ExceptionDescription = "Error thrown in Unpublish - Service Publisher"
                };
                throw error;
            }
        }

        //GETTERS AND SETTERS FOR THE TOKEN
        public static int GetToken()
        {
            return token;
        }

        public static void SetToken(int pToken)
        {
            token = pToken;
        }

        //CHECK IF THE INPUT STRING CONFORMS TO URL STYLE
        private static bool CheckURLValid(string source)
        {
            Uri uriResult;
            return Uri.TryCreate(source, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
        }
    }
}
