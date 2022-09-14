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

        public void  GetFromServer()
        {

            //IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            //var bytes = udpClient.Receive(ref endPoint);
            //string receiveData = Encoding.ASCII.GetString(bytes);
            //return receiveData;

            try
            {
              //  while (true)
              //  {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receiveBytes =  udpClient.Receive(ref endPoint);
                    string returnData = Encoding.ASCII.GetString(receiveBytes);
                    Console.WriteLine("[Client] Download data: {0}\n\n",returnData);
                   // return returnData;
               // }
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
               // return "error";

            }
        }

        public void ShutDownClient()
        {
            //Console.WriteLine("[Client UDP] Client closed!");
            udpClient.Close();
        }
    }
}