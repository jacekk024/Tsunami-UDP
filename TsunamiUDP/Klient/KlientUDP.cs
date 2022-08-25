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



        public KlientUDP(int localPort,int remotePort) 
        {
            this.remotePort = remotePort;
            udpClient = new UdpClient(localPort);
        }

        public string SentToServer(string command)
        {

            byte[] sendBytes = Encoding.ASCII.GetBytes(command);
            udpClient.Connect("localhost", remotePort);
            udpClient.Send(sendBytes, sendBytes.Length);
            return "[Client UDP] Message sent!\n";

        }

        public string GetFromServer(string commmand)
        {
            IPEndPoint RemoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] receiveBytes = udpClient.Receive(ref RemoteIPEndPoint);
            string returnData = Encoding.ASCII.GetString(receiveBytes);
            Console.WriteLine("[Client UDP] Message received!\n");
            return returnData;
        }


    }
}
