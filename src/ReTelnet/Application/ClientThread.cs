using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using TelnetMock.Repository;
using TelnetMock.TcpAdapter;

namespace TelnetMock.Application
{
    public class ClientThread : IClientThread
    {
        private ITelnetConnection _clientSocket;
        private ITelnetConnection _targetClient;
        //private Thread _commsThread;

        public ClientThread()
        {
            //_commsThread = new Thread(HandleClientComm);
        }

        public void Start(ITelnetConnection clientSocket, ITelnetConnection targetClient)
        {
            _clientSocket = clientSocket;
            _targetClient = targetClient;
            //_commsThread.Start();
            HandleClientComms();
        }

        private void HandleClientComms()
        {
            Console.WriteLine("Connecting to target client");
            _targetClient.Connect();

            Console.WriteLine("Waiting data from client ID");
            while (true)
            {
                string message = _clientSocket.Read();

                _targetClient.Write(message);
                string response = _targetClient.Read();
                if (response == null) break;
                _clientSocket.Write(response);
            }

            _targetClient.Close();
            _clientSocket.Close();
        }

    }
}
