using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using PartyUp.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using PartyUp.Utils;
using PartyUp.Data;
using PartyUp.Filters;

namespace PartyUp.Hubs
{
    [TokenAuth]
    public class BaseHub : Hub
    {
        protected readonly static SignalRConnectionMappingToUser _connections = new SignalRConnectionMappingToUser();
        protected readonly static ApplicationDataFactory appDataFactory = new ApplicationDataFactory();

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
                User u = appDataFactory.Users.GetById(userJWT.User.Id);
                _connections.Add(Context.ConnectionId, u);
                return base.OnConnected();
            }
            catch (Exception)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _connections.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}