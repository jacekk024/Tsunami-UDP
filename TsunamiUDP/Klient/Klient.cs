using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klient
{
    class Klient
    {
        private static bool running;
        static void Main()
        {
            running = true;

            while (running) 
            {
                ReadUserCommand();
            }
        }

        static void ReadUserCommand()
        {
            string command;
            Console.WriteLine("Give the command:");
            command = Console.ReadLine();

     
            switch (command.Split()[0].Trim(' '))
            {
                case "list":
                    KlientTCP clientTcp = new KlientTCP("localhost", 12345);
                    clientTcp.SentToServer("list");
                    string list = clientTcp.GetFromServer();
                    Console.WriteLine("Availible files: {0}",list);
                    break;
                case "get":

                    break;
                case "exit":
                    Console.WriteLine("[Client] Closed!\n");
                    running = false;
                    break;

                case "help":
                    Console.WriteLine("[Client] Basic help!");
                    break;

                default:
                    Console.WriteLine("Wrong Option! Choose again! \n");
                    break;
            }

        }
    }
}
