using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Klient
{
    class KlientTCP
    {

        private TcpClient client;
        private NetworkStream stream;

        //get rozmiar_pliku rozmiar_paczki id_zadania ilosc_pacek 

        public KlientTCP(string server,int port) 
        {
            try
            {
                client = new TcpClient(server, port);
                stream = client.GetStream();
            }
            catch(Exception e) 
            {
                Console.WriteLine(e.Message.ToString()); // kontrolne do zakomentowania 
                client.Close();
                stream.Close();
                Console.WriteLine("[Client TCP] Client closed!");
            }
        }

        public void SentToServer(string command)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(command);
            stream.Write(data, 0, data.Length);
        }

        public string GetFromServer() 
        {
            byte[] data = new byte[256];
            string odp = string.Empty;
            int bytes;
            try
            {
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    odp += System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                }
                while (stream.DataAvailable);
            }
            catch 
            {
                Console.WriteLine("[Client: Can`t send message!]");
            }
            stream.Close();
            client.Close();
            return odp;
        }
    }
}
