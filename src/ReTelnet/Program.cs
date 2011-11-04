using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TelnetMock.Application;
using TelnetMock.Repository;
using TelnetMock.TcpAdapter;

namespace TelnetMock
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientThreadLocator.Register<ClientThread>();
            var listener = new TcpListenerAdapter(IPAddress.Any, 3000);
            var targetServer = new TelnetConnection("server", 7072);
            var server = new ReTelnetServer(listener, targetServer);
            server.Start();
        }
    }
}
