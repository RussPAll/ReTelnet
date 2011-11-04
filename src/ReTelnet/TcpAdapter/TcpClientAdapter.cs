using System;
using System.Net;
using System.Net.Sockets;

namespace TelnetMock.TcpAdapter
{
    public class TcpClientAdapter : ITcpClient
    {
        private TcpClient Target { get; set; }

        public TcpClientAdapter(TcpClient target)
        {
            Target = target;
        }

        public NetworkStream GetStream()
        {
            return Target.GetStream();
        }

        public IPEndPoint RemoteIPEndPoint
        {
            get { return Target.Client.RemoteEndPoint as IPEndPoint; }
        }

        public void Close()
        {
            Target.Close();
        }

        public byte[] Read()
        {
            throw new NotImplementedException();
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] message)
        {
            throw new NotImplementedException();
        }
    }
}