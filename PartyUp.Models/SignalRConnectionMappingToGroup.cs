using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Models
{
    public class SignalRConnectionMappingToGroup
    {
        private readonly Dictionary<String, String> _connections =
            new Dictionary<String, String>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(string connectionId, String groupId)
        {
            lock (_connections)
            {
                String connectedGroupId;
                if (!_connections.TryGetValue(connectionId, out connectedGroupId))
                {
                    _connections.Add(connectionId, groupId);
                }
            }
        }

        public String GetConnectedGroup(string connectionId)
        {
            String connectedGroupId;
            if (_connections.TryGetValue(connectionId, out connectedGroupId))
            {
                return connectedGroupId;
            }

            return null;
        }

        public void Remove(string connectionId)
        {
            lock (_connections)
            {
                String connectedGroupId;
                if (!_connections.TryGetValue(connectionId, out connectedGroupId))
                {
                    return;
                }
                else
                {
                    _connections.Remove(connectionId);
                }
            }
        }
    }
}
