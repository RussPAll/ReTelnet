using System;
using System.Net.Sockets;
using TelnetMock.Application;
using TelnetMock.Repository;

namespace TelnetMock.TcpAdapter
{
    public class TcpListenerAdapter : ITcpListener
    {
        private TcpListener Target { get; set; }

        public TcpListenerAdapter(System.Net.IPAddress iPAddress, int portNo)
        {
            Target = new TcpListener(iPAddress, portNo);
        }

        public ITelnetConnection AcceptTcpClient()
        {
            return new TelnetConnection(Target.AcceptTcpClient());
        }

        public void Start()
        {
            Target.Start();
        }
    }
}
