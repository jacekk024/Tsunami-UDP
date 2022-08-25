using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serwer
{
    class SerwerUDP
    {
        private UdpClient client;
        IPEndPoint RemoteIpEndPoint;


        public SerwerUDP(int localPort) 
        {
            client = new UdpClient(localPort);
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }

        public string SentToClient()
        {
            while (true) 
            {
                byte[] sendData = Encoding.ASCII.GetBytes(GetFromClient());
                client.Send(sendData, sendData.Length, RemoteIpEndPoint);
                Console.WriteLine("[Server UDP] Message sent!");

            }
        }

        public string GetFromClient()
        {
            while (true)
            {
                byte[] bytes = client.Receive(ref RemoteIpEndPoint);
                string receiveData = Encoding.ASCII.GetString(bytes);
                Console.WriteLine("[Server UDP] Message receive!");
                return receiveData;
            }
        }
    }
}
