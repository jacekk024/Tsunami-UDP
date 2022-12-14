using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Klient
{
    class KlientUDP
    {
        private UdpClient udpClient;
        private int remotePort;

        //dane odnosnie transmisji 

        public KlientUDP(int localPort, int remotePort)
        {
            this.remotePort = remotePort;
            udpClient = new UdpClient(localPort);
            udpClient.Connect("127.0.0.1", remotePort);
        }

        public void SentToServer(string command)
        {
            byte[] sendBytes = Encoding.ASCII.GetBytes(command);
            udpClient.Send(sendBytes, sendBytes.Length);
        }

        public async Task<string> GetFromServer()
        {
            try
            {
                while (true)
                {
                    var receiveBytes = await udpClient.ReceiveAsync();
                    string returnData = Encoding.ASCII.GetString(receiveBytes.Buffer);
                    return returnData;
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
                return "error";

            }
        }
    }
}