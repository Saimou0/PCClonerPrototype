using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace Receiver
{
    public class ReceiverUDP
    {
        private static UdpClient udpClient;

        public static async Task<UdpResult> SendUDPBroadcastAsync(string connectionCode)
        {
            udpClient = new(8888)
            {
                EnableBroadcast = true
            };

            byte[] data = Encoding.UTF8.GetBytes(connectionCode);

            try
            {
                // Send a udp broadcast to the local network on port 8888 
                IPEndPoint broadcastEndpoint = new(IPAddress.Broadcast, 8888);
                udpClient.Send(data, data.Length, broadcastEndpoint);
                Console.WriteLine("Message sent via UDP.");

                while(true)
                {
                    // Wait for the result of the broadcast and read the response
                    UdpReceiveResult result = await udpClient.ReceiveAsync();
                    byte[] bytes = result.Buffer;
                    string receivedData = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    if (receivedData == "ACK")
                    {
                        // Close udp client
                        udpClient.Close();
                        udpClient.Dispose();

                        // Return from method.
                        return new UdpResult
                        {
                            Success = true,
                            SenderIPAddress = result.RemoteEndPoint.Address
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Received error message {ex.Message}");
            }

            udpClient.Close();
            udpClient.Dispose();

            return new UdpResult
            {
                Success = false,
                SenderIPAddress = null
            };
        }

        public class UdpResult
        {
            public bool Success { get; set; }
            public IPAddress? SenderIPAddress { get; set; }
        }
    }
}
