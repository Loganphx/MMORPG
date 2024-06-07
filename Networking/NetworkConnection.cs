using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Networking
{
    public class NetworkConnection
    {
        public Guid ConnectionId { get; private set; }

        private TcpClient? tcpClient;

        private NetworkStream stream;
    
        public BinaryReader Reader { get; private set; }
        public BinaryWriter Writer { get; private set;}

        public bool NeedsToConnect => tcpClient == null;

        public NetworkConnection(Guid connectionId, TcpClient tcpClient)
        {
            ConnectionId = connectionId;
            this.tcpClient = tcpClient;
            stream = tcpClient.GetStream();
            Reader = new BinaryReader(stream);
            Writer = new BinaryWriter(stream);
        }

        public NetworkConnection()
        {
            
        }
        public int AvailableBytes => tcpClient?.Available ?? -1;
        public bool IsConnection => tcpClient?.Connected ?? false;

        public TcpClient? Client => tcpClient;

        public async Task Connect(string address, int port)
        {
            if (!NeedsToConnect)
            {
                throw new InvalidOperationException("Already connected to a host");
            }

            tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(address, port);

            stream = tcpClient.GetStream();
            Reader = new BinaryReader(stream);
            Writer = new BinaryWriter(stream);
        }
    }
}