using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serwer
{
    class SerwerTCP
    {
        private TcpListener tcpListener;
        private TcpClient client;
        private NetworkStream stream;

        public SerwerTCP(int port)
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
        }

        public void   SentToClient(string command)
        {
            while (client.Connected)
            {
                while (command != string.Empty)
                {
                     byte[] msg = Encoding.ASCII.GetBytes(command);
                      stream.Write(msg, 0, msg.Length);
                     command = string.Empty;
                }
            }
        }

        public async Task<string> GetFromClient()
        {
            int len;
            byte[] bytes = new byte[256];
            string data = string.Empty;

            client = await tcpListener.AcceptTcpClientAsync();
            stream = client.GetStream();
            while (client.Connected)
            {
                if (stream.DataAvailable)
                {
                    len = await stream.ReadAsync(bytes, 0, bytes.Length);
                    data += Encoding.ASCII.GetString(bytes, 0, len);
                }
                return data;
            }
            stream.Close();
            return "Message error!";
        }
    }
}