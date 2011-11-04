using TelnetMock.Repository;
using TelnetMock.TcpAdapter;

namespace TelnetMock.Application
{
    public interface IClientThread
    {
        void Start(ITelnetConnection client, ITelnetConnection targetServer);
    }
}