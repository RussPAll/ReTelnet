using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using TelnetMock.Repository;
using TelnetMock.TcpAdapter;

namespace TelnetMock.Application
{
    public static class ClientThreadLocator
    {
        private static Type _clientThreadType = typeof(object);

        public static void Register<T>()
        {
            _clientThreadType = typeof(T);
            GetConstructor();
        }

        public static IClientThread Resolve()
        {
            var constructor = GetConstructor();
            var service = (IClientThread)constructor.Invoke(new object[0]);
            return service;
        }

        private static ConstructorInfo GetConstructor()
        {
            ConstructorInfo constructor = _clientThreadType.GetConstructor(new Type[0]);
            if (constructor == null)
                throw new ArgumentException("Type " + _clientThreadType + " does not have a valid constructor.");
            return constructor;
        }
    }
}
