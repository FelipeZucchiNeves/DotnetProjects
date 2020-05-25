using System.Net.NetworkInformation;
using System.Text;
using System;
using System.Net.Sockets;
using System.Net;

namespace Sockets
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Socket ListenSocket;
            ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            int port = 65534;
            IPEndPoint LocalIPEndPoint = new IPEndPoint(IPAddress.Any, port);
            ListenSocket.Bind(LocalIPEndPoint);

            //Optional
            System.Console.WriteLine("Server IP Address: "+ LocalIpAdress());
            System.Console.WriteLine("Listening on Port: "+ port);

            //Start Listening
            ListenSocket.Listen(4);

            //Accept incoming connection
            Socket AcceptSocket = ListenSocket.Accept();

            //Star Receiving Can Send too
            byte[] ReceiveBuffer = new byte[1024];
            int ReceiveByteCount;

            ReceiveByteCount = AcceptSocket.Receive(ReceiveBuffer, SocketFlags.None);

            if(ReceiveByteCount > 0)
            {
                string msg = Encoding.ASCII.GetString(ReceiveBuffer, 0, ReceiveByteCount);
                Console.WriteLine(msg);
            }

            AcceptSocket.Shutdown(SocketShutdown.Both);

            AcceptSocket.Close();

        }


        public static string LocalIpAdress()
        {
            IPHostEntry host;
            string localIp = "";
            host = Dns.GetHostEntry(Dns.GetHostName());    //Get the name of current host
            foreach (IPAddress ip in host.AddressList)
            {
                if(ip.AddressFamily == AddressFamily.InterNetwork) // Find the IP which internetwork
                {
                    localIp = ip.ToString();
                    break;
                }
            }
            return localIp;
        }
    }
}









            // IPAddress ip = Dns.GetHostEntry("localhost").AddressList[0];
            // TcpListener server= new TcpListener(ip, 8080);
            // TcpClient client = default(TcpClient);

            // try
            // {
            //     server.Start();
            //     System.Console.WriteLine("Server started...");
            //     Console.Read();

            // }
            // catch (System.Exception ex)
            // {
            //     System.Console.WriteLine(ex.ToString());
            //     Console.Read();
            // }

            // while (true)
            // {
            //     client = server.AcceptTcpClient();

            //     byte[] receivedBuffer = new byte[100];
            //     NetworkStream stream = client.GetStream();
            //     stream.Read(receivedBuffer, 0, receivedBuffer.Length);

            //     string msg = Encoding.ASCII.GetString(receivedBuffer, 0, receivedBuffer.Length);

            // }
