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
    public class ChatHub : Hub
    {
        private readonly static SignalRConnectionMapping _connections = new SignalRConnectionMapping();

        public void SendMessage(string message)
        {
            User user =_connections.GetConnectedUser(Context.ConnectionId);
            Clients.All.newMessage(user.FirstName, message);
        }

        public override Task OnConnected()
        {
            var token = Context.QueryString.Get("token");
            if (String.IsNullOrWhiteSpace(token))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }

            try
            {
                JsonWebToken userJWT = new JsonWebToken(token, Utilities.GetSetting("JWTSecret"), false);
                _connections.Add(Context.ConnectionId, userJWT.User);
                return base.OnConnected();
            }
            catch (Exception e)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }
        }
    }
}