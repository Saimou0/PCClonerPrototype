using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class ServerTCP
    {
        public static async void Start(IPEndPoint clientEP)
        {
            using Socket socket = new(clientEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            await socket.ConnectAsync(clientEP);
            
            while(true)
            {
                var message = "Hello buddy, <|EOM|>";
                var messageBytes = Encoding.UTF8.GetBytes(message);
                _ = await socket.SendAsync(messageBytes, SocketFlags.None);
                Console.Write($"Socket client sent message: | {message} |");

                var buffer = new byte[1024];
                var received = await socket.ReceiveAsync(buffer, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);
                if (response == "<|ACK|>")
                {
                    Console.WriteLine($"Received ACK: | {response} |");
                    break;
                }
            }

            socket.Shutdown(SocketShutdown.Both);
        }

    }
}
