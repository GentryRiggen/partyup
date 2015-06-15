using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Models
{
    public class SignalRConnectionMapping
    {
        private readonly Dictionary<String, User> _connections =
            new Dictionary<String, User>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(string connectionId, User user)
        {
            lock (_connections)
            {
                User connectedUser;
                if (!_connections.TryGetValue(connectionId, out connectedUser))
                {
                    _connections.Add(connectionId, user);
                }
            }
        }

        public User GetConnectedUser(string connectionId)
        {
            User connectedUser;
            if (_connections.TryGetValue(connectionId, out connectedUser))
            {
                return connectedUser;
            }

            return null;
        }

        public void Remove(string connectionId)
        {
            lock (_connections)
            {
                User connectedUser;
                if (!_connections.TryGetValue(connectionId, out connectedUser))
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
