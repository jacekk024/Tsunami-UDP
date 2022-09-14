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
        private static KlientUDP clientUdp;

        static void Main()
        {
            int numPack;
            string data = null;
            bool sendFlag = false;
           // clientUdp = new KlientUDP(12347, 12346);

            running = true;

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
                        Console.WriteLine("[Client] File info:" + fileInfo);

                            numPack = int.Parse(fileInfo.Split()[3]);
                            
                            clientUdp = new KlientUDP(12347, 12346);
                            clientUdp.SentToServer(fileInfo.Split()[4]);// file id
                            // wysylamy id a serwer sam ogarnia ile paczek nam wyslac odczytujac to z bazy 

                            sendFlag = true;
                        //clientUdp.SentToServer("siema");
                        //clientUdp.SentToServer("siema");


                        //// udp client get id_zadania
                        //Task.Run(async () => 
                        //{
                        //    //clientUdp.SentToServer("siema");
                        //    while (true) 
                        //    {

                        //        data = await clientUdp.GetFromServer();
                        //        Console.WriteLine(data);
                        //        break;
                        //    }
                        //});
                        //Task.Delay(1000);                
                        break;
                    case "stop":
                        break;
                    case "help":
                        break;
                    case "exit":
                        Console.WriteLine("[Client] Closed!\n");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("[Client] Wrong Option! Choose again!");
                        break;
                }


                // clientUdp.SentToServer("siema");
                Task.Delay(1000);

                //// udp client get id_zadania
                Task.Run(() =>
                {
                   // = new KlientUDP(12347, 12346);
                  // clientUdp.SentToServer("siema");
                   //clientudp.senttoserver("siema");
                   while (true)
                   {
                       data = clientUdp.GetFromServer().Result;
                       Console.WriteLine(data);

                   }
                   //clientUdp.ShutDownClient();
                });
                // Task.Delay(1000);
                if (sendFlag != false)
                {
                    clientUdp.ShutDownClient();
                    sendFlag = false;
                }

                //data = null;
            }
        }
    }
}