using System.Net;
using System.Net.Sockets;
using System.Text;
using TelnetMock.CrossCutting.Validation;
using TelnetMock.TcpAdapter;

namespace TelnetMock.Repository
{
    internal enum Verbs
    {
        Will = 251,
        Wont = 252,
        Do = 253,
        Dont = 254,
        Iac = 255
    }

    internal enum Options
    {
        Sga = 3
    }

    public class TelnetConnection : ITelnetConnection
    {
        private TcpClient _client;

        private const int TimeOutMs = 100;
        private readonly string _hostName;
        private readonly int _portNo;

        public TelnetConnection(string hostName, int portNo)
        {
            #region Parameter Validation
            hostName.RequireArgument("hostName").IsNotNullOrEmpty();
            #endregion
            _hostName = hostName;
            _portNo = portNo;
        }

        public TelnetConnection(TcpClient tcpClient)
        {
            _client = tcpClient;
            _hostName = RemoteIPEndPoint.Address.ToString();
            _portNo = RemoteIPEndPoint.Port;
        }

        public void WriteLine(string cmd)
        {
            Write(cmd + "\n");
        }

        public void Connect()
        {
            _client = new TcpClient(_hostName, _portNo);
        }

        public void Close()
        {
            if (IsConnected)
                _client.Close();
        }

        /// <exception cref="System.ArgumentNullException">cmd is not specified</exception>
        public void Write(string cmd)
        {
            if (!_client.Connected) return;
            byte[] buf = ASCIIEncoding.ASCII.GetBytes(cmd.Replace("\0xFF", "\0xFF\0xFF"));
            _client.GetStream().Write(buf, 0, buf.Length);
        }

        IPEndPoint ITelnetConnection.RemoteIPEndPoint
        {
            get { return (IPEndPoint)_client.Client.RemoteEndPoint; }
        }

        /// <exception cref="System.ArgumentOutOfRangeException">Sleep timeout const is set to an invalid value</exception>
        /// <exception cref="System.ObjectDisposedException">The System.Net.Sockets.Socket has been closed.</exception>
        /// <exception cref="System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information.</exception>
        public string Read()
        {
            if (!_client.Connected) return null;
            var sb = new StringBuilder();
            do
            {
                ParseTelnet(sb);
                System.Threading.Thread.Sleep(TimeOutMs);
            } while (_client.Available > 0);
            return sb.ToString();
        }

        public bool IsConnected
        {
            get { return _client.Connected; }
        }

        public IPEndPoint RemoteIPEndPoint
        {
            get { return (IPEndPoint) _client.Client.RemoteEndPoint; }
        }

        private void ParseTelnet(StringBuilder sb)
        {
            while (_client.Available > 0)
            {
                int input = _client.GetStream().ReadByte();
                switch (input)
                {
                    case -1:
                        break;
                    case (int) Verbs.Iac:
                        // interpret as command
                        int inputverb = _client.GetStream().ReadByte();
                        if (inputverb == -1) break;
                        switch (inputverb)
                        {
                            case (int) Verbs.Iac:
                                //literal Iac = 255 escaped, so append char 255 to string
                                sb.Append(inputverb);
                                break;
                            case (int) Verbs.Do:
                            case (int) Verbs.Dont:
                            case (int) Verbs.Will:
                            case (int) Verbs.Wont:
                                // reply to all commands with "Wont", unless it is Sga (suppres go ahead)
                                int inputoption = _client.GetStream().ReadByte();
                                if (inputoption == -1) break;
                                _client.GetStream().WriteByte((byte) Verbs.Iac);
                                if (inputoption == (int) Options.Sga)
                                    _client.GetStream().WriteByte(inputverb == (int) Verbs.Do
                                                                        ? (byte) Verbs.Will
                                                                        : (byte) Verbs.Do);
                                else
                                    _client.GetStream().WriteByte(inputverb == (int) Verbs.Do
                                                                        ? (byte) Verbs.Wont
                                                                        : (byte) Verbs.Dont);
                                _client.GetStream().WriteByte((byte) inputoption);
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        sb.Append((char) input);
                        break;
                }
            }
        }
    }
}