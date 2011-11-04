using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TelnetMock.Application;
using TelnetMock.Repository;
using TelnetMock.TcpAdapter;

namespace TelnetMock.Tests.Application
{
    [TestFixture]
    public class ClientThreadLocatorTests
    {
        class ClientThreadInvalidMock : IClientThread
        {
            public ClientThreadInvalidMock(int dummyParam) { }
            public void Start(ITelnetConnection client, ITelnetConnection targetServer) { }
        }

        [Test]
        public void Register_NoEmptyConstructorConstructor_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => ClientThreadLocator.Register<ClientThreadInvalidMock>());
        }
        
        [Test]
        public void Register_ValidConstructor_DoesNotThrowException()
        {
            ClientThreadLocator.Register<ClientThreadMock>();
        }

        [Test]
        public void Resolve_ValidTypeConfigured_ReturnsInstance()
        {
            ClientThreadLocator.Register<ClientThreadMock>();
            var instance = ClientThreadLocator.Resolve();
            Assert.IsInstanceOf<ClientThreadMock>(instance);
        }

        [Test]
        public void Resolve_InvalidTypeConfigured_ThrowsException()
        {
            try
            {
                ClientThreadLocator.Register<ClientThreadInvalidMock>();
            }
            catch (Exception)
            { }
            Assert.Throws<ArgumentException>(() => ClientThreadLocator.Resolve());
        }
    }
}
