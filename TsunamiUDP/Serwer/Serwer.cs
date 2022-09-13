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
        //private static bool goneoff = false;
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
            int packSize = 65535;

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


        public async Task ProcessReadAsync()
        {
            try
            {
                string filePath = "plik1.txt";
                if (File.Exists(filePath) != false)
                {
                    string text = await ReadTextAsync(path + filePath);
                    Console.WriteLine(text);
                }
                else
                {
                    Console.WriteLine($"file not found: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        async Task<string> ReadTextAsync(string filePath)
        {
            using (var sourceStream =
                new FileStream(
                    filePath,
                    FileMode.Open, FileAccess.Read, FileShare.Read,
                    bufferSize: 4096, useAsync: true))
            {

                var sb = new StringBuilder();

                byte[] buffer = new byte[0x1000];
                int numRead;
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string text = Encoding.Unicode.GetString(buffer, 0, numRead);
                    sb.Append(text);
                }
                return sb.ToString();
            }
        }

        static void Main()
        {
            Console.WriteLine("[Server] Running!");
            serverRunning = true;
            string fileName = null;
            var server = new Serwer();
            SerwerTCP serwerTCP = new SerwerTCP(12345);
            SerwerUDP serwerUDP = new SerwerUDP(12346);
            path = @"D:\dokumenty\Studia Infa Stosowana\PROSIKO\Tsunami-UDP\serwer\";

            while (true)
            {
                answer = serwerTCP.GetFromClient().Result;   //wait request, nonblocking


                Task.Run(async () =>
                {
                    while (serverRunning)
                    {
                        switch (answer.Split()[0])
                        {
                            case "list":
                                await serwerTCP.SentToClient(server.FilesList(path));
                                break;
                            case "get":
                                fileName = answer.Split()[1];
                                await serwerTCP.SentToClient(server.FileInfo(fileName));
                                break;
                            default:
                                break;
                        }
                        serverRunning = false;
                        answer = null;
                    }
                });
              //  Task.Delay(1000);
            //    dataUDP = serwerUDP.GetFromClient().Result;
                Task.Run(async () =>
                {
                    //dataUDP = serwerUDP.GetFromClient().Result;
                    //  dataUDP = serwerUDP.GetFromClient().Result;
                    while (true)
                    {
                        dataUDP = serwerUDP.GetFromClient().Result;
                        Console.WriteLine(dataUDP);
                        await serwerUDP.SentToClient("siema client");
                    }
                });
                Task.Delay(1000);
            }
        }
    }
}