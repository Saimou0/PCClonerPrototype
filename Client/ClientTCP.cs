using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class ClientTCP
    {
        private static IPEndPoint endPoint;
        public static async void Start()
        {
            endPoint = new(IPAddress.Any, 8888);
            using Socket listener = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(endPoint);
            listener.Listen(10);

            var handler = await listener.AcceptAsync();
            while (true)
            {
                var buffer = new byte[1024];
                var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

                var eom = "<|EOM|>";
                if(response.IndexOf(eom) > -1)
                {
                    Console.WriteLine($"Received message: | {response.Replace(eom, "")}");

                    var ackMessage = "<|ACK|>";
                    var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                    await handler.SendAsync(echoBytes, 0);
                    
                    Console.WriteLine($"Sent ACK: | {ackMessage}");
                    break;
                }
            }
        }

    }
}
