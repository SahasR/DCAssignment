﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CustomException;
using System.Xml.Linq;
using System.Reflection;

namespace Authenticator
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class AuthInterfaceImpl : AuthInterface
    {
        private static AuthInterfaceImpl instance = null;
        private string registerFile; private string tokenFile; private string projectDirectory;

        private AuthInterfaceImpl()
        {
            // SPECIFYING A FILE PATH 
            projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            registerFile = projectDirectory + "\\" + "RegisterInfo.txt";
            tokenFile = projectDirectory + "\\" + "TokenInfo.txt";
        }

        //TO ACCESS THE SAME LOCAL TEXT FILES WE DESIGNED THE AUTHENTICATOR AS A SINGLETON
        public static AuthInterfaceImpl getInstance()
        {
            if (instance == null)
            {
                instance = new AuthInterfaceImpl();
            }
            return instance;
        }

        //ADD A NEW USER IF A USER WITH EXACT USERNAME DOES NOT EXIST 
        public String Register(String name, String password)
        {
            if (!File.Exists(registerFile))
            {
                using(StreamWriter sw = File.CreateText(registerFile)){ }
            }

            if(!CheckUserExists(name))
            {
                WriteFile(name, password, registerFile);
                return "Successfully Registered";
            }
            else
            {
                AuthenticatorFaults fault = new AuthenticatorFaults();
                fault.ExceptionMessage = "Registration Failed. User already exists";
                fault.ExceptionDescription = "Authentication Error";
                throw new FaultException<AuthenticatorFaults>(fault);
            }
        }

        //ISSUES A TOKEN FOR A VALID REGISTERED USER
        public int Login(String name, String password)
        {
            if(!File.Exists(tokenFile))
            {
                using (StreamWriter sw = File.CreateText(tokenFile)) { }
            }

            if (CheckValidUser(name, password))
            {
                Random random = new Random();
                int number = random.Next(10000000, 99999999);
                WriteFile(name, number.ToString(), tokenFile);
                return number;
            }
            else
            {
                AuthenticatorFaults fault = new AuthenticatorFaults();
                fault.ExceptionMessage = "Login Failed. Username and password does not match";
                fault.ExceptionDescription = "Authentication Error";
                throw new FaultException<AuthenticatorFaults>(fault);
            }
        }

        public String Validate(int token)
        {
            //CHECK WHETHER THE TOKEN IS VALID AND NOT EXPIRED
            if(CheckToken(token))
            {
                return "Validated";
            }
            return "Not Validated";
        }

        //CHECK WHETHER USER CREDENTIALS CORRESPONDS TO THE INFROMATION SAVED IN THE LOCAL TEXT FIL
        private bool CheckUserExists(String name)
        {
            bool isFound = false;
            using (StreamReader sr = File.OpenText(registerFile))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var values = line.Split(',');

                    if (name.Equals(values[0]))
                    {
                        isFound = true;
                        break;
                    }
                }
            }
            return isFound;
        }

        //CHECK WHETHER USER CREDENTIALS CORRESPONDS TO THE INFROMATION SAVED IN THE LOCAL TEXT FILE
        private bool CheckValidUser(String name, String information)
        {
            bool isFound = false;
            using (StreamReader sr = File.OpenText(registerFile))
            {
                while(!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var values = line.Split(',');

                    if (name.Equals(values[0]) && information.Equals(values[1]))
                    {
                        isFound = true;
                        break;
                    }
                }
            }
            return isFound;
        }

        //CHECK WHETHER A TOKEN IS ALREADY GENERATED
        private bool CheckToken(int token)
        {
            bool isFound = false; 
            using (StreamReader sr = File.OpenText(tokenFile))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var values = line.Split(',');

                    if (token.ToString().Equals(values[1]))
                    {
                        isFound = true;
                        break;
                    }
                }
            }
            return isFound;
        }

        //SAVES NAME AND PASSWORD/TOKEN INTO A LOCAL TEXT FILE
        private void WriteFile(String name, String information, String filePath)
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(name + ","+ information);
            }
        }

        // CLEARS THE FILE THAT CONTAINS ALL GENERATED TOKENS
        public void ClearTokens()
        {
            if(File.Exists(tokenFile))
            {
                File.Delete(tokenFile);
            }
        }
    }
}
