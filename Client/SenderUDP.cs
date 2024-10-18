using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Client
{
    public class SenderUDP
    {
        private static UdpClient udpClient;
        private static IPEndPoint remoteEndPoint;

        public static async void Start(int connectionCode)
        {
            udpClient = new UdpClient();
            await SendUDPBroadcast(connectionCode);
        }

        private static async Task SendUDPBroadcast(int connectionCode)
        {
            udpClient.EnableBroadcast = true;

            byte[] data = Encoding.UTF8.GetBytes(connectionCode.ToString());

            try
            {
                IPEndPoint broadcastEndpoint = new(IPAddress.Broadcast, 8888);
                udpClient.Send(data, data.Length, broadcastEndpoint);
                Console.WriteLine("Message sent via UDP.");

                while(true)
                {
                    udpClient = new UdpClient(8888);
                    UdpReceiveResult result = await udpClient.ReceiveAsync();
                    byte[] bytes = result.Buffer;
                    string receivedData = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    if (receivedData == "<|ACK|>")
                    {
                        ClientTCP.Start();
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Received error message {ex.Message}");
            }

        }
    }
}
