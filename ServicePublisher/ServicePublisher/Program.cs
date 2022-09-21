using ServicePublisher;

int userInput = 0;

Console.WriteLine(" Welcome to the Service Publisher!");

while (userInput != 5)
{
    Thread.Sleep(3000);
    Console.Clear();
    Console.WriteLine(" 1. Register\n 2. Log-in\n 3. Publish a service\n 4. Unpublish a service\n 5. Exit\n");

    try
    {
        Console.Write("Select option: ");
        userInput = Int32.Parse(Console.ReadLine());

        if (userInput > 5 || userInput < 1)
        {
            Usage();

        }else{

            Menu(userInput);
        }

    }catch(FormatException error){

        Console.WriteLine(error.Message);   
        Usage();
    }
}



static void Usage()
{
    Console.WriteLine("Type in the corresponding number for each menu option and press Enter to access a service");
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

            Console.Write("Enter an API endpoint: ");
            endpoint = Console.ReadLine();
            services.Unpublish(endpoint);
            break;

        case 4:
            Console.WriteLine("Unpublish a service");

            Console.Write("Enter an API endpoint: ");
            endpoint = Console.ReadLine();
            services.Unpublish(endpoint);
            break;

        case 5:
            Console.WriteLine("Exiting the Service Publisher!");
            break;
    }
}