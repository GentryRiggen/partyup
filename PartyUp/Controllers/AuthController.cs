using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PartyUp.Data;
using PartyUp.Models;
using PartyUp.Models.DTO;
using PartyUp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PartyUp.Controllers
{
    public class AuthController : ApiController
    {
        private ApplicationDataFactory _dataFactory = new ApplicationDataFactory();
        private UserManager<User> _userManager;
        private UserStore<User> _userStore;

        public UserStore<User> UserStore
        {
            get
            {
                return _userStore ?? new UserStore<User>(_dataFactory.Context);
            }
        }

        public UserManager<User> UserManager
        {
            get
            {
                return _userManager ?? new UserManager<User>(this.UserStore);
            }
        }

        public async Task<IHttpActionResult> Get()
        {
            UserDTO currentUser = await _dataFactory.Users.GetDTOById(User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(currentUser);
            }
        }

        [Route("api/auth")]
        public HttpResponseMessage Post([FromBody]LoginUserDTO loginModel)
        {
            User u = UserManager.Find(loginModel.Username, loginModel.Password);
            if (u == null)
            {
                // No user with userName/password exists.
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid username/password");
            }
            else
            {
                User dbUser = _dataFactory.Users.GetById(u.Id);
                // Create JWT payload for user
                // Get user roles
                IEnumerable<string> roles = UserManager.GetRoles(dbUser.Id);
                JsonWebToken userJWT = new JsonWebToken(u.Id, roles);
                string jwt = userJWT.ToString(Utilities.GetSetting("JWTSecret"));
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Bearer = jwt,
                    User = new UserDTO(dbUser),
                    Claims = roles
                });
            }
        }
    }
}
