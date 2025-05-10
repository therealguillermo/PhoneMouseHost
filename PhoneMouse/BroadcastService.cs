using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace PhoneMouse
{
    public class BroadcastService : IDisposable
    {
        private UdpClient? _udpClient;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly int _broadcastPort = 6098; // Port for broadcasting
        private readonly int _broadcastInterval = 1000; // Broadcast every second
        private Task? _broadcastTask;
        private bool _isDisposed;

        public BroadcastService()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void InitializeUdpClient()
        {
            try
            {
                _udpClient?.Dispose();
                _udpClient = new UdpClient();
                _udpClient.EnableBroadcast = true;
                _udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Start()
        {
            if (_broadcastTask != null)
            {
                return;
            }

            InitializeUdpClient();
            _broadcastTask = Task.Run(async () =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        if (_udpClient == null)
                        {
                            InitializeUdpClient();
                        }

                        var hostName = Dns.GetHostName();
                        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                            .Where(ni => ni.OperationalStatus == OperationalStatus.Up &&
                                       ni.NetworkInterfaceType != NetworkInterfaceType.Loopback);

                        foreach (var networkInterface in networkInterfaces)
                        {
                            var ipProperties = networkInterface.GetIPProperties();
                            var ipv4Properties = ipProperties.UnicastAddresses
                                .Where(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork)
                                .ToList();

                            foreach (var ipv4Property in ipv4Properties)
                            {
                                try
                                {
                                    var ipAddress = ipv4Property.Address;
                                    var subnetMask = ipv4Property.IPv4Mask;
                                    var broadcastAddress = GetBroadcastAddress(ipAddress, subnetMask);

                                    var message = $"{hostName}|{ipAddress}";
                                    var bytes = Encoding.UTF8.GetBytes(message);

                                    await _udpClient!.SendAsync(bytes, bytes.Length, new IPEndPoint(broadcastAddress, _broadcastPort));
                                }
                                catch (Exception)
                                {
                                    // Ignore interface errors and continue with next interface
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Try to reinitialize the UDP client on next iteration
                        _udpClient?.Dispose();
                        _udpClient = null;
                    }

                    await Task.Delay(_broadcastInterval, _cancellationTokenSource.Token);
                }
            }, _cancellationTokenSource.Token);
        }

        private IPAddress GetBroadcastAddress(IPAddress address, IPAddress subnetMask)
        {
            var addressBytes = address.GetAddressBytes();
            var maskBytes = subnetMask.GetAddressBytes();

            var broadcastBytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                broadcastBytes[i] = (byte)(addressBytes[i] | ~maskBytes[i]);
            }

            return new IPAddress(broadcastBytes);
        }

        public void Stop()
        {
            if (_isDisposed)
            {
                return;
            }

            _cancellationTokenSource.Cancel();
            _broadcastTask?.Wait();
            _broadcastTask = null;
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            Stop();
            _udpClient?.Dispose();
            _cancellationTokenSource.Dispose();
            _isDisposed = true;
        }
    }
} 