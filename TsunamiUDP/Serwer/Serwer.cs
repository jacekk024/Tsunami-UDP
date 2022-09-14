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
        private static bool serverRunning = true;
        private static string answer = null;
        private static string dataUDP = null;
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
            StringBuilder info = new StringBuilder();
            int packSize = 65500;

            if (fileName == null || fileName.Length == 0)
            {
                return "error!";
            }

            FileInfo file = new FileInfo(path + fileName);

            if (!file.Exists)
            {
                return "error!";
            }

            int numPack = (int)Math.Ceiling((double)file.Length / packSize);

            int id = (new Random()).Next(1, 100);
            if (userBase.Keys.Count != 0)
                while (userBase.ContainsKey(id))
                    id = (new Random()).Next(1, 100);

            info.Append("Ok ");
            info.AppendFormat("{0} ", file.Length);
            info.AppendFormat("{0} ", packSize);
            info.AppendFormat("{0} ", numPack);
            info.AppendFormat("{0} ", id);

            userBase.Add(id, info.ToString());

            return info.ToString();
        }

        static void Main()
        {
            Console.WriteLine("[Server] Running!");
            string fileName = null;
            var server = new Serwer();
            SerwerTCP serwerTCP = new SerwerTCP(12345);
            SerwerUDP serwerUDP = new SerwerUDP(12346);
            path = @"D:\dokumenty\Studia Infa Stosowana\PROSIKO\Tsunami-UDP\serwer\";

            while (true)
            {
                answer = serwerTCP.GetFromClient().Result;   //wait request, nonblocking
               
                Task.Run(() => 
                {

                    while (serverRunning)
                        {
                            switch (answer.Split()[0])
                            {
                                case "list":
                                    serwerTCP.SentToClient(server.FilesList(path));
                                    break;
                                case "get":
                                    fileName = answer.Split()[1];
                                    serwerTCP.SentToClient(server.FileInfo(fileName));
                                break;

                                case "stop":

                                break;

                                case "data":
                                if (answer.Split()[1] == "ok")
                                {
                                    Console.WriteLine("[Server] ok");
                                    serwerTCP.SentToClient("ok");
                                }
                                    // else
                                    // sprawdzamy ktorej paczki brakuje    
                                break;
                            }        
                            answer = null;
                        }                  
                });


                Task.Run(async() => // 
                {

                    dataUDP = serwerUDP.GetFromClient(); //oczekiwanie na dane od klienta
                    int id = int.Parse(dataUDP);

                    if (userBase.ContainsKey(id))
                    {
                        string[] param = userBase.First(x => x.Key == id).Value.Split();
                        char[] result = new char[int.Parse(param[2])]; //rozmiar paczki
                        string data = null;
                            using (var stream = new FileStream(path + fileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: int.Parse(param[2]), useAsync: true))
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                while(await reader.ReadAsync(result, 0, int.Parse(param[2])) >= 0) 
                                {
                                    data = new string(result);
                                    await serwerUDP.SentToClient(data);
                                }                                  
                            }
                    }
                });
                Task.Delay(1000);
            }
        }
    }
}