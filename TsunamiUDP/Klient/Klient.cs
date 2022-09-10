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
        private static string command;
        private static KlientTCP clientTcp;


        static void Main()
        {
            running = true;


            while (running) 
            {
                Console.WriteLine("[Client] Give the command:");
                command = Console.ReadLine();

                

                switch (command.Split()[0].Trim(' '))
                {
                    case "list":
                        clientTcp = new KlientTCP("localhost", 12345);
                        clientTcp.SentToServer("list");
                        string list = clientTcp.GetFromServer();
                        Console.WriteLine("[Client] Availible files: {0}", list);
                        break;
                    case "get":
                        clientTcp = new KlientTCP("localhost", 12345);
                        clientTcp.SentToServer(command);
                        string fileInfo = clientTcp.GetFromServer();
                        Console.WriteLine("[Client] File info:" + fileInfo);
                        break;
                    case "exit":
                        Console.WriteLine("[Client] Closed!\n");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("[Client] Wrong Option! Choose again! \n");
                        break;
                }
            }
        }
    }
}
