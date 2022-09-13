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

        public async Task SentToClient(string command)
        {
          //  while (true)
           // {
                 byte[] sendData = Encoding.ASCII.GetBytes(command);
                 await client.SendAsync(sendData, sendData.Length,RemoteIpEndPoint);
          //  }
        }


        public async Task<string> GetFromClient()
        {
                var bytes = await client.ReceiveAsync();
                string receiveData = Encoding.ASCII.GetString(bytes.Buffer);
                //Console.WriteLine("Received data {0}", receiveData);
                //Console.WriteLine("[Server UDP] Message receive!");
                return receiveData;            
        }
    }
}
