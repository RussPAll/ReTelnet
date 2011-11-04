using System;
using System.Net.Sockets;
using TelnetMock.Repository;
using TelnetMock.TcpAdapter;

namespace TelnetMock.Application
{
    public class ReTelnetServer
    {
        private readonly ITcpListener _tcpListener;
        private ITelnetConnection _targetServer;

        public ReTelnetServer(ITcpListener tcpListener, ITelnetConnection targetServer)
        {
            _tcpListener = tcpListener;
            _targetServer = targetServer;
            Console.WriteLine("Server started");
        }

        public void Start()
        {
            _tcpListener.Start();
            _targetServer.Connect();
            var client = _tcpListener.AcceptTcpClient();
            var remoteClient = client.RemoteIPEndPoint;
            Console.WriteLine("Client ID received from  " + remoteClient.Address + ":" + remoteClient.Port);
            var clientThread = ClientThreadLocator.Resolve();
            clientThread.Start(client, _targetServer);
        }
    }
}
