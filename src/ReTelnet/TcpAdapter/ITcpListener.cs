using System;
using System.Net.Sockets;
using TelnetMock.Repository;

namespace TelnetMock.TcpAdapter
{
    public interface ITcpListener
    {
        ITelnetConnection AcceptTcpClient();
        void Start();
    }
}
