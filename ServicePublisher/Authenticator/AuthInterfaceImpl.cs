using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CustomException;

namespace Authenticator
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class AuthInterfaceImpl : AuthInterface
    {
        private static AuthInterfaceImpl instance = null;
        private string registerFile; private string tokenFile; private string projectDirectory; private double timer;

        private AuthInterfaceImpl()
        {
            // SPECIFYING A FILE PATH 
            projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            registerFile = projectDirectory + "\\" + "RegisterInfo.txt";
            tokenFile = projectDirectory + "\\" + "TokenInfo.txt";

            //CREATING A TEXT FILE TO STORE USER AND TOKEN INFORMATION
            using (StreamWriter sw = File.CreateText(registerFile)) {}
            using (StreamWriter sw = File.CreateText(tokenFile)) {}
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

        //ADD A NEW USER IF USER WITH EXACT USERNAME DOES NOT EXIST ALREADY
        public String Register(String name, String password)
        {
            if(!CheckUserExists(name))
            {
                WriteFile(name, password, registerFile);
                return "Successfully registered!";

            }else{

                //TO DO : CHECK
                CustomFaults exception = new CustomFaults
                {
                    ExceptionMessage = "Username already exists. Please choose another username",
                    ExceptionDescription = "Fault occured in Register - Authenticator"
                };
                throw new FaultException<CustomFaults>(exception, new FaultReason(exception.ExceptionMessage));
            }
        }

        //ISSUES A TOKEN FOR A VALID REGISTERED USER
        public int Login(String name, String password)
        {
            if(CheckValidUser(name, password))
            {
                Random random = new Random();
                int number = random.Next(10000000, 99999999);
                WriteFile(name, number.ToString(), tokenFile);
                return number;
            }

            //TO DO : CHECK
            CustomFaults exception = new CustomFaults
            {
                ExceptionMessage = "Your username or password do not match",
                ExceptionDescription = "Fault occured in Login - Authenticator"
            };
            throw new FaultException<CustomFaults>(exception, new FaultReason(exception.ExceptionMessage));
        }

        public String Validate(int token)
        {
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

        //GETTERS AND SETTERS FOR THE TIME
        public double GetTimer()
        {
            return timer;
        }

        public void SetTimer(double timer)
        {
            this.timer = timer * 60.0 * 1000.0;
        }

        // CLEARS THE FILE THAT CONTAINS ALL GENERATED TOKENS
        private void ClearToken()
        {
            File.WriteAllText(tokenFile, String.Empty);
        }
    }
}
