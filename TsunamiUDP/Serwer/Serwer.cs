using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serwer
{
    class Serwer
    {
        private static bool serverRunning = false;
        public void ThreadDelay()
        {
            while (serverRunning)
                Task.Delay(1000);
        }

        public static string FilesList(string path)
        {
            DirectoryInfo place = new DirectoryInfo(path);

            FileInfo[] Files = place.GetFiles();
            Console.WriteLine("Files are:");
            Console.WriteLine();
            string list = null;

            foreach (FileInfo i in Files)
            {
                Console.WriteLine(i.Name);
                list += i.Name + " ";
            }
            return list;
        }


        static void Main()
        {
            Console.WriteLine("[Server] Running!");
            serverRunning = true;
            var server = new Serwer();
            SerwerTCP serwerTCP = new SerwerTCP(12345);
            SerwerUDP serwerUDP = new SerwerUDP(12346);
            string answer = null;


            while (serverRunning) 
            {
                //Task.Run(() => serwerTCP.StartTCP());
                Task.Run(() => answer = serwerTCP.GetFromClient().Result);   //wait request, nonblocking
                
                switch (answer)
                {
                    case "list":
                        Task.Run(() => serwerTCP.SentToClient(FilesList(@"D:\dokumenty\Studia Infa Stosowana\PROSIKO\Tsunami-UDP\serwer")));
                        break;
                    case "get":
                        //przeslanie za pomoca TCP parametrow do komunikacji przez UDP
                        //przeslanie za pomoca UDP
                        break;
                    default:
                        Console.WriteLine(answer);
                        break;
                }
                server.ThreadDelay();
            }
          //  server.ThreadDelay();
        }
    }
}
