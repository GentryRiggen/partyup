using PartyUp.Data;
using PartyUp.Models;
using PartyUp.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.AspNet.SignalR;
using System.Net.Http.Headers;

namespace PartyUp.Filters
{
    public class TokenAuthAttribute : AuthorizationFilterAttribute
    {
        private ApplicationDataFactory _dataFactory = new ApplicationDataFactory();

        public string Roles { get; set; }
        private static string tokenName = "partyUp-Token";
        private static string tokenHeader = "Authorization";

        public static bool ValidateToken(JsonWebToken token, ApplicationDataFactory dataFactory, string roles = null)
        {
            // If past expiration
            if (Utilities.GetEpochTimeNow() > token.ExpirationEpoch)
            {
                return false;
            }

            // Ensure this is an existing user
            User dbUser = dataFactory.Users.Find(token.User.Id);
            if (dbUser == null)
            {
                return false;
            }

            var userManager = new UserManager<User>(new UserStore<User>(dataFactory.Context));
            // Check to see if the JWT lied about roles the user has
            foreach (string role in userManager.GetRoles(dbUser.Id))
            {
                if (!token.Claims.Contains(role))
                {
                    return false;
                }
            }



            // Finally Check Roles requested the JWT verify
            if (roles != null && roles.Length > 0 && !String.IsNullOrEmpty(roles))
            {
                bool validClaims = false;
                foreach (string claim in token.Claims)
                {
                    if (roles.Contains(claim))
                    {
                        validClaims = true;
                        break;
                    }
                }
                if (!validClaims)
                {
                    return false;
                }
            }

            // Default is all good!
            return true;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {

            // Get the token from the bearer header
            IEnumerable<string> values;
            string token = String.Empty;
            var headers = actionContext.Request.Headers;

            // If no token, look for cookie
            headers.TryGetValues(tokenHeader, out values);

            if (values == null || values.Count() == 0 || String.IsNullOrEmpty(values.First().ToString()))
            {
                CookieState cookie = headers.GetCookies().Select(c => c[tokenName])
                .FirstOrDefault();
                if (cookie == null || String.IsNullOrEmpty(cookie.ToString()))
                {
                    HandleUnauthorized(actionContext);
                    return;
                }
                else
                {
                    token = cookie.ToString();
                }
            }
            else if (values != null && values.Count() > 0)
            {
                token = values.First().Split(' ')[1];
            }


            if (!String.IsNullOrEmpty(token))
            {
                // Decode the token and verfiy any roles and expirations
                try
                {

                    JsonWebToken userJWT = new JsonWebToken(token, Utilities.GetSetting("JWTSecret"), true);

                    bool valid = TokenAuthAttribute.ValidateToken(userJWT, _dataFactory, this.Roles);

                    string[] claims = new string[0];
                    if (userJWT.Claims != null)
                    {
                        claims = userJWT.Claims.ToArray();
                    }
                    if (valid)
                    {
                        // Set user on Thread and contexts
                        var currentPrinciple = new GenericPrincipal(new GenericIdentity(userJWT.User.Id), claims);
                        Thread.CurrentPrincipal = currentPrinciple;
                        HttpContext.Current.User = currentPrinciple;
                        return;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    HandleUnauthorized(actionContext);
                    return;
                }
            }

            HandleUnauthorized(actionContext);
            return;
        }

        private void HandleUnauthorized(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }
    }
}