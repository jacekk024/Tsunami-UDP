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
        private UdpClient udpClient;
       //IPEndPoint RemoteIpEndPoint;


        public SerwerUDP(int localPort) 
        {
            udpClient = new UdpClient(localPort);
           // RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }

        public async Task SentToClient(string command)
        {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12347);
                byte[] sendData = Encoding.ASCII.GetBytes(command);
                await udpClient.SendAsync(sendData, sendData.Length, RemoteIpEndPoint);            
        }


        public string GetFromClient()
        {
            try
            {
                while (true)
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                    var bytes =  udpClient.Receive(ref endPoint);
                    string receiveData = Encoding.ASCII.GetString(bytes);
                    return receiveData;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "[Server UDP] error";
            }
        }
    }
}
