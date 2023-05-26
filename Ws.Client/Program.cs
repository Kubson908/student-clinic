using System;
using System.Net.WebSockets;

namespace Ws.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sfdsfsdf");
            Console.ReadLine();

            using(ClientWebSocket client = new ClientWebSocket())
            {
                Uri serviceUri = new Uri("ws://localhost:7042/send");
            }
        }
    }
}