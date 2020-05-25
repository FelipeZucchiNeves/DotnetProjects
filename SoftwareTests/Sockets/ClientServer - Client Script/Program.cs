using System.Net;
using System;
using System.Net.Sockets;

namespace ClientServer___Client_Script
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create Socket
            //Input IP Adress and port



            System.Console.WriteLine("Enter Server IP Address - dotted quad notation -: ");
            string srvrIP = Console.ReadLine();
            System.Console.WriteLine("Enter Server Port Number: ");
            string srvrPort = Console.ReadLine();
            System.Console.WriteLine("\nSending to: "+ srvrIP + " : " + srvrPort);

            //Create the socket based on data input 
            Socket sendSocket;
            sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Connect to the Endpoint (Server)
            IPAddress destinationIP = IPAddress.Parse(srvrIP);
            int destinationPort = Convert.ToInt32(srvrPort);
            IPEndPoint destinationEndPoint = new IPEndPoint(destinationIP, destinationPort);

            // User Message
            System.Console.WriteLine("\nWaiting to Connect...");

            sendSocket.Connect(destinationEndPoint);

            System.Console.WriteLine("Connected");

            //Send Information
            string msg;
            System.Console.WriteLine("\nEnter Message to Send:");
            msg = Console.ReadLine();
            byte [] data = System.Text.Encoding.ASCII.GetBytes(msg);

            //User Message
            Console.WriteLine("\n Sending Data...");

            sendSocket.Send(data, SocketFlags.None);

            System.Console.WriteLine("Sending Complete...");


        

        }
    }
}
