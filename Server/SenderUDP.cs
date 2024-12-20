using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Sender
{
    public class SenderUDP
    {

        private static string _code = GenerateConnectionCode();
        private static string _fullCode;

        private static UdpClient udpClient;
        private const int _listenedPort = 8888;

        private static IPEndPoint _remoteEndPoint;
        public static IPEndPoint clientEP;

        public static void RegenerateCode()
        {
            _code = GenerateConnectionCode();
            _fullCode = GetConnectionCode();
        }

        public static async Task<bool> ConnectionListenerAsync(CancellationToken cancellationToken)
        {
            // Start listening with udp client for udp broadcasts on port 8888
            Console.WriteLine("Starting udp client.");
            udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, _listenedPort));

            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for broadcast");

                    // Wait for connection for a maximum of 8 minutes.
                    var receiveTask = udpClient.ReceiveAsync();
                    var completedTask = await Task.WhenAny(receiveTask, Task.Delay(500_000, cancellationToken));

                    if(completedTask == receiveTask)
                    {
                        // Read received broadcast
                        UdpReceiveResult result = receiveTask.Result;
                        _remoteEndPoint = result.RemoteEndPoint;
                        byte[] bytes = result.Buffer;
                        string receivedData = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                        Console.WriteLine($"Received broadcast from {_remoteEndPoint.Address} : {receivedData}");

                        // Check if the broadcast code matches
                        if (receivedData == _code || receivedData == _fullCode)
                        {
                            // Save the broadcast sender's IP for later use.
                            Console.WriteLine("Code match");
                            clientEP = new IPEndPoint(_remoteEndPoint.Address, _listenedPort);

                            // Connect to broadcast sender and send an acknowledgement
                            udpClient.Connect(clientEP);

                            var ackMessage = "ACK";
                            bytes = Encoding.UTF8.GetBytes(ackMessage);
                            await udpClient.SendAsync(bytes, bytes.Length);

                            udpClient.Close();
                            return true;
                        } else if(receivedData != _code || receivedData != _fullCode)
                        {
                            Console.WriteLine("Code doesn't match");
                        } else
                        {
                            Console.WriteLine("Something went wrong");
                        }

                    }
                    else
                    {
                        Console.WriteLine("UDP Cancelled.");
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                udpClient.Close();
                udpClient.Dispose();
            }

            return false;
        }

        private static string GenerateConnectionCode()
        {
            Random random = new();
            return random.Next(10000000, 99999999).ToString();
        }

        public static string GetConnectionCode()
        {
            _code ??= GenerateConnectionCode();

            string codePart1 = _code.Substring(0, 4);
            string codePart2 = _code.Substring(4, 4);
            string fulllCode = codePart1 + " " + codePart2;
            return fulllCode;
        }
    }
}
