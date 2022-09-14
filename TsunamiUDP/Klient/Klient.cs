using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klient
{
    class Klient
    {
        private static bool running = true;
        private static string command;
        private static KlientTCP clientTcp;
        private static KlientUDP clientUdp;
        static string data = null;
        static int numPack;

        static void Main()
        {
            clientUdp = new KlientUDP(12347, 12346);

            while (running)
            {
                Console.Write("[Client] Give the command:");
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
                            Console.WriteLine("[Client] File info: " + fileInfo);

                            numPack = int.Parse(fileInfo.Split()[3]);
                            clientUdp.SentToServer(fileInfo.Split()[4]);// file id
                            Task.Run(async() =>
                            {
                                while (numPack >= 0)
                                {   
                                    data = await clientUdp.GetFromServer();

                                    Console.WriteLine("[Client] Data pack: {0}, Received data: {1}", numPack, data);
                                    if (data != null)
                                    {
                                        numPack--;
                                        data = null;
                                    }
 
                                }
                                 clientTcp.SentToServer("data ok");
                                fileInfo = clientTcp.GetFromServer();
                                Console.WriteLine("[Client] Status:" + fileInfo);
                            });
                        //potwierdzenie otrzymania pliku OK TCP

                        break;
                        case "stop":
                            clientTcp.SentToServer("stop");
                        break;
                        case "help":
                        HelpInfo();
                            break;
                        case "exit":
                            Console.WriteLine("[Client] Closed!\n");
                            running = false;
                            break;
                        default:
                            Console.WriteLine("[Client] Wrong Option! Choose again!");
                            break;
                    }
                
            }
        }

        static void HelpInfo() 
        {
            Console.WriteLine("[Client]-[Help]\n" +
                "list - show available list of files\n" +
                "get - [name-of-file] - get info about file and download it\n" +
                "stop - [id]- stop downloading data from server for current id");        
        }
    }
}