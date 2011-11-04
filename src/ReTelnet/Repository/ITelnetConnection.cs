using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TelnetMock.Repository
{
    public interface ITelnetConnection
    {
        bool IsConnected { get; }
        IPEndPoint RemoteIPEndPoint { get; }
        string Read();
        void Write(string cmd);
        void WriteLine(string cmd);
        void Connect();
        void Close();
    }
}