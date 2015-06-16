using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using PartyUp.Filters;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using PartyUp.Models;
using PartyUp.Utils;

namespace PartyUp.Hubs
{
    [HubName("chat")]
    [TokenAuth]
    public class ChatHub : BaseHub
    {
        public void SendMessage(string message)
        {
            User user =_connections.GetConnectedUser(Context.ConnectionId);
            Clients.All.newMessage(user.FirstName, message);
        }
    }
}