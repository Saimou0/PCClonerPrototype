using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Server
{
    public class ReceiverUDP
    {

        private static string _code = GenerateConnectionCode();
        private static UdpClient udpClient;
        private const int _listenedPort = 8888;
        private static IPEndPoint _remoteEndPoint;
        public static IPEndPoint clientEP;

        public static async void Start()
        {


            await ConnectionListener();
        }

        public static async Task ConnectionListener()
        {
            udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, _listenedPort));
            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for broadcast");

                    UdpReceiveResult result = await udpClient.ReceiveAsync();
                    byte[] bytes = result.Buffer;
                    _remoteEndPoint = result.RemoteEndPoint;
                    string receivedData = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    Console.WriteLine($"Received broadcast from {_remoteEndPoint.Address} : {receivedData}");

                    if (receivedData == _code)
                    {
                        Console.WriteLine("Code match");
                        clientEP = new IPEndPoint(_remoteEndPoint.Address, _listenedPort);

                        udpClient.Connect(clientEP);

                        var ackMessage = "<|ACK|>";
                        bytes = Encoding.UTF8.GetBytes(ackMessage);
                        await udpClient.SendAsync(bytes, bytes.Length);

                        udpClient.Close();

                        ServerTCP.Start(clientEP);
                        break;
                    } else
                    {
                        Console.WriteLine("Code doesn't match");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                udpClient.Close();
            }
        }

        private static string GenerateConnectionCode()
        {
            Random random = new();
            return random.Next(10000000, 99999999).ToString();
        }

        public static string GetConnectionCode()
        {
            string codePart1 = _code.Substring(0, 4);
            string codePart2 = _code.Substring(4, 4);
            string fulllCode = codePart1 + " " + codePart2;
            return fulllCode;
        }
    }
}
