using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelnetMock.Application;
using TelnetMock.Repository;
using TelnetMock.TcpAdapter;

namespace TelnetMock.Tests.Application
{
    public class ClientThreadMock : IClientThread
    {
        public static ITelnetConnection Client { get; set; }
        public static ITelnetConnection TargetServer { get; set; }
        public static bool IsStarted { get; set; }

        public void Start(ITelnetConnection client, ITelnetConnection targetServer)
        {
            Client = client;
            TargetServer = targetServer;
            IsStarted = true;
        }
    }
}
