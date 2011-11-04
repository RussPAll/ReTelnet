using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using TelnetMock.Application;
using TelnetMock.Repository;
using TelnetMock.TcpAdapter;

namespace TelnetMock.Tests.Application
{
    [TestFixture]
    public class ClientThreadTests
    {
        private Mock<ITelnetConnection> _clientMock;
        private Mock<ITelnetConnection> _targetClient;
        private ClientThread _thread;

        [SetUp]
        public void Setup()
        {
            _clientMock = new Mock<ITelnetConnection>();
            _targetClient = new Mock<ITelnetConnection>();
            _thread = new ClientThread();
        }

        [Test]
        public void Start_ReadsFromClient()
        {
            _thread.Start(_clientMock.Object, _targetClient.Object);
            _clientMock.Verify(x => x.Read());
        }

        [Test]
        public void Start_OpensForwardingConnection()
        {
            _thread.Start(_clientMock.Object, _targetClient.Object);
            _targetClient.Verify(x => x.Connect());
        }

        [Test]
        public void Start_ForwardsMessagesReceivedFromClient()
        {
            var message = "AAA";
            _clientMock.Setup(x => x.Read())
                .Returns(message);
            _thread.Start(_clientMock.Object, _targetClient.Object);
            _targetClient.Verify(x => x.Write(message));
        }

        [Test]
        public void Start_RunsUntilTelnetServerDisconnects()
        {
            var commands = new Queue(new string[] {"A", "A", "QUIT"} );
            var responses = new Queue(new string[] {"A", "A", null});
            _clientMock.Setup(x => x.Read())
                .Returns(() => (string)commands.Dequeue());
            _targetClient.Setup(x => x.Read())
                .Returns(() => (string)responses.Dequeue());
            _thread.Start(_clientMock.Object, _targetClient.Object);
            Assert.AreEqual(0, commands.Count);
            Assert.AreEqual(0, responses.Count);
        }

    }
}
