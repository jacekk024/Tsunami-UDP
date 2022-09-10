using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serwer
{
    //public struct FileDescription 
    //{
    //    public int length;
    //    public int id;
    //    public int packSize;
    //    public string status;
    //    int packNumber;
    //}

    class Serwer
    {
        private static bool serverRunning = false;
        private static bool goneoff = false;
        private static string answer = null;
        private static string path;
        public static Dictionary<int, string> userBase = new Dictionary<int, string>(); 


        public string FilesList(string path)
        {
            DirectoryInfo place = new DirectoryInfo(path);

            FileInfo[] Files = place.GetFiles();
            string list = null;

            foreach (FileInfo i in Files)
            {           
                list += i.Name + " ";
            }
            return list;
        }

        public string FileInfo(string fileName) 
        {
            //rozmiar_paczki/ilosc_paczek/rozmiar_pliku/id_zadania
            string info = null;
            int packSize = 65535;

            if (fileName == null || fileName.Length == 0)
            {
                return "error Please choose file name!";
            }

            FileInfo file = new FileInfo(path + fileName);

            if (!file.Exists)
            {
                return "error The file was not found!";
            }

            int numPack = (int)Math.Ceiling((double)file.Length / packSize);
            
            int id = (new Random()).Next(1, 100);
            if (userBase.Keys.Count != 0)
                while (!userBase.ContainsKey(id))
                    id = (new Random()).Next(1, 100);
            
            info += "Ok" + file.Length.ToString() + packSize.ToString() + numPack.ToString() + id.ToString();
            userBase.Add(id,info);
            Console.WriteLine("dupa");
            return info;
        }


        static void Main()
        {
            Console.WriteLine("[Server] Running!");
            serverRunning = true;
            var server = new Serwer();
            SerwerTCP serwerTCP = new SerwerTCP(12345);
            SerwerUDP serwerUDP = new SerwerUDP(12346);
            path = @"D:\dokumenty\Studia Infa Stosowana\PROSIKO\Tsunami-UDP\serwer\";

            while (serverRunning) 
            {
                if (answer == null && goneoff ==  false) 
                {
                    goneoff = true;
                    Task.Run(() => answer = serwerTCP.GetFromClient().Result);   //wait request, nonblocking
                }

                if (answer != null)
                {
                    switch (answer.Split()[0])
                    {
                        case "list":
                            Task.Run(() => serwerTCP.SentToClient(server.FilesList(path)));
                            break;
                        case "get":
                            //Console.WriteLine(answer.Split()[1]);
                            //server.FileInfo(answer.Split()[1]);
                            Task.Run(async () =>
                            {
                                await serwerTCP.SentToClient(server.FileInfo(answer.Split()[1]));
                            });
                            //przeslanie za pomoca TCP parametrow do komunikacji przez UDP
                            //przeslanie za pomoca UDP
                            break;
                        default:
                            break;
                    }
                    goneoff = false;
                    answer = null;
                }
                Task.Delay(1000);
            }
        }
    }
}
