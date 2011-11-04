using System.Net;
using System.Net.Sockets;

namespace TelnetMock.TcpAdapter
{
    public interface ITcpClient
    {
        //NetworkStream GetStream();
        IPEndPoint RemoteIPEndPoint { get; }
        void Close();
        byte[] Read();
        void Connect();
        void Write(byte[] message);
    }
}