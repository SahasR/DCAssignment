using CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServicePublisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int userInput = 0;

            Console.WriteLine("Welcome to the Service Publisher!");

            while (userInput != 5)
            {
                Thread.Sleep(2000);
                Console.Clear();

                //DISPLAY MENU OPTIONS TO THE USER IN THE CONSOLES
                Console.WriteLine("1. Register\n2. Log-in\n3. Publish a service\n4. Unpublish a service\n5. Exit\n");

                try
                {
                    Console.Write("Select option: ");
                    userInput = Int32.Parse(Console.ReadLine());
                    Console.WriteLine();

                    if (userInput > 5 || userInput < 1)
                    {
                        Usage();

                    }else{

                        Menu(userInput);
                    }

                }catch(FormatException error){

                    Console.WriteLine(error.Message);
                    Usage();

                }catch(CustomFaults error){

                    Console.WriteLine(error.ExceptionMessage);
                    Console.WriteLine();
                }
            }
        }

        static void Usage()
        {
            Console.WriteLine("\nType in the corresponding number for each menu option and press Enter to access a service");
            Console.WriteLine("1 - provies register services for new users");
            Console.WriteLine("2 - provides login services for users that are already registered");
            Console.WriteLine("3 - publish new services");
            Console.WriteLine("4 - unpublish a service already registered in the system");
            Console.WriteLine("5 - exit");
            Console.WriteLine();
        }

        static void Menu(int userInput)
        {
            Services services = new Services();
            string userName; string password; string endpoint;

            switch (userInput)
            {
                case 1:
                    Console.WriteLine("Register");

                    Console.Write("Enter user name: ");
                    userName = Console.ReadLine();

                    Console.Write("Enter password: ");
                    password = Console.ReadLine();

                    services.Registration(userName, password);
                    break;

                case 2:
                    Console.WriteLine("Log-in");

                    Console.Write("Enter user name: ");
                    userName = Console.ReadLine();

                    Console.Write("Enter password: ");
                    password = Console.ReadLine();

                    services.Login(userName, password);
                    break;

                case 3:
                    Console.WriteLine("Publish a service");

                    Console.Write("Name of the service you wish to publish: ");
                    string name = Console.ReadLine();

                    Console.Write("Description for the service: ");
                    string description = Console.ReadLine();

                    Console.Write("Enter an API endpoint: ");
                    endpoint = Console.ReadLine();

                    Console.Write("Number of operands in the service: ");
                    string numOperands = Console.ReadLine();

                    Console.Write("Enter an operand type (integer/double): ");
                    string typeOperands = Console.ReadLine();

                    services.Publish(name, description, endpoint, numOperands, typeOperands);
                    break;

                case 4:
                    Console.WriteLine("Unpublish a service");

                    Console.Write("Enter an API endpoint: ");
                    endpoint = Console.ReadLine();
                    services.Unpublish(endpoint);
                    break;

                case 5:
                    Console.WriteLine("Exiting the Service Publisher");
                    break;
            }
        }
    }
}


        


