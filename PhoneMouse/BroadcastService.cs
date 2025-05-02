using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhoneMouse
{
    public class BroadcastService : IDisposable
    {
        private readonly UdpClient _udpClient;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly int _broadcastPort = 5000; // Port for broadcasting
        private readonly int _broadcastInterval = 1000; // Broadcast every second
        private Task _broadcastTask;

        public BroadcastService()
        {
            _udpClient = new UdpClient();
            _udpClient.EnableBroadcast = true;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            _broadcastTask = Task.Run(async () =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        var hostName = Dns.GetHostName();
                        var hostEntry = Dns.GetHostEntry(hostName);
                        var ipAddress = hostEntry.AddressList
                            .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

                        if (ipAddress != null)
                        {
                            var message = $"{hostName}|{ipAddress}";
                            var bytes = Encoding.UTF8.GetBytes(message);
                            
                            // Broadcast to all interfaces
                            foreach (var address in hostEntry.AddressList)
                            {
                                if (address.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    var broadcastAddress = GetBroadcastAddress(address);
                                    await _udpClient.SendAsync(bytes, bytes.Length, new IPEndPoint(broadcastAddress, _broadcastPort));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Broadcast error: {ex.Message}");
                    }

                    await Task.Delay(_broadcastInterval, _cancellationTokenSource.Token);
                }
            }, _cancellationTokenSource.Token);
        }

        private IPAddress GetBroadcastAddress(IPAddress address)
        {
            var bytes = address.GetAddressBytes();
            bytes[3] = 255; // Set last octet to 255 for broadcast
            return new IPAddress(bytes);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _broadcastTask?.Wait();
        }

        public void Dispose()
        {
            Stop();
            _udpClient.Dispose();
            _cancellationTokenSource.Dispose();
        }
    }
} 