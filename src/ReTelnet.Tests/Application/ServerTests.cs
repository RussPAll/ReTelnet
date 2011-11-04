using System.Net;
using System.Net.Sockets;
using Moq;
using NUnit.Framework;
using TelnetMock.Application;
using TelnetMock.Repository;
using TelnetMock.TcpAdapter;

namespace TelnetMock.Tests.Application
{
    [TestFixture]
    public class ServerTests
    {
        private Mock<ITelnetConnection> _clientMock;
        private Mock<ITcpListener> _tcpListenerMock;
        private Mock<ITelnetConnection> _targetServerMock;
        private ReTelnetServer _server;

        [SetUp]
        public void Setup()
        {
            _clientMock = new Mock<ITelnetConnection>();
            _clientMock.SetupGet(x => x.RemoteIPEndPoint)
                .Returns(new IPEndPoint(0, 0));

            _targetServerMock = new Mock<ITelnetConnection>();

            _tcpListenerMock = new Mock<ITcpListener>();
            _tcpListenerMock.Setup(x => x.AcceptTcpClient())
                .Returns(_clientMock.Object);
            ClientThreadLocator.Register<ClientThreadMock>();

            _server = new ReTelnetServer(_tcpListenerMock.Object, _targetServerMock.Object);
        }

        [Test]
        public void Start_StartsTcpListener()
        {
            _server.Start();
            _tcpListenerMock.Verify(x => x.Start());
        }

        [Test]
        public void Start_ListensForClient()
        {
            _server.Start();
            _tcpListenerMock.Verify(x => x.AcceptTcpClient());
        }

        [Test]
        public void Start_ConstructsClientThread()
        {
            ClientThreadMock.Client = null;
            _server.Start();
            Assert.AreSame(_clientMock.Object, ClientThreadMock.Client);
        }

        [Test]
        public void Start_ConnectsToTargetServer()
        {
            _server.Start();
            _targetServerMock.Verify(x => x.Connect());
        }

        [Test]
        public void Start_ConstructsClientThreadWithTargetServer()
        {
            ClientThreadMock.TargetServer = null;
            _server.Start();
            Assert.AreSame(_clientMock.Object, ClientThreadMock.Client);
            Assert.AreSame(ClientThreadMock.TargetServer, _targetServerMock.Object);
        }

        [Test]
        public void Start_StartsClientThread()
        {
            ClientThreadMock.IsStarted = false;
            _server.Start();
            Assert.IsTrue(ClientThreadMock.IsStarted);
        }
    }
}
