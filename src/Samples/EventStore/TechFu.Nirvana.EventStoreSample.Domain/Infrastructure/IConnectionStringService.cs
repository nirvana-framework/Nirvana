using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechFu.Nirvana.Domain;

namespace TechFu.Nirvana.EventStoreSample.Domain.Infrastructure
{
    public interface IConnectionStringService
    {
        string GetStringForConnection(ConnectionType connectionType);
        string GetStringForConnection(ConnectionType connectionType, bool cached);
        void SetModeForConnection(ConnectionType connectionType, ConnectionMode mode);
        void Refresh();

        List<Status> CurrentStatus();

    }

    public class ConnectionType : Enumeration<ConnectionType>
    {
        public Func<IApplicationConfiguration, string> PrimaryConnection { get; private set; }
        public Func<IApplicationConfiguration, string> FailoverConnection { get; private set; }



        public ConnectionType(int value, string displayName, Func<IApplicationConfiguration, string> primaryConnection, Func<IApplicationConfiguration, string> failoverConnection) : base(value, displayName)
        {
            PrimaryConnection = primaryConnection;
            FailoverConnection = failoverConnection;
        }

        public static ConnectionType RdbmsContext = new ConnectionType(1, "RdbmsContext", a => a.DataStoreConnectionString, a => a.DataStoreConnectionStringDr);

    }

    public enum ConnectionMode
    {
        Primary,
        DisasterRecovery
    }


    public class Status
    {
        public string Name { get; set; }
        public string Mode { get; set; }
    }

    public class ConnectionStatus
    {
        public ConnectionType Type { get; set; }
        public ConnectionMode Mode { get; set; }
        public string ConnectionString { get; set; }
    }
}