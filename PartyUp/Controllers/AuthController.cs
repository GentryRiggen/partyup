using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PartyUp.Data;
using PartyUp.Filters;
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

        [Route("api/auth/user")]
        [TokenAuth]
        public async Task<HttpResponseMessage> Get()
        {
            User currentUser = await _dataFactory.Users.GetByIdAsync(User.Identity.Name);
            if (currentUser == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid username/password");
            }
            else
            {
                IEnumerable<string> roles = UserManager.GetRoles(currentUser.Id);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    User = new UserDTO(currentUser),
                    Roles = roles
                });
            }
        }

        [Route("api/auth")]
        public async Task<HttpResponseMessage> Post([FromBody]LoginUserDTO loginModel)
        {
            User u = UserManager.Find(loginModel.Username, loginModel.Password);
            if (u == null)
            {
                // No user with userName/password exists.
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid username/password");
            }
            else
            {
                User dbUser = await _dataFactory.Users.GetByIdAsync(u.Id);
                // Create JWT payload for user
                // Get user roles
                IEnumerable<string> roles = UserManager.GetRoles(dbUser.Id);
                JsonWebToken userJWT = new JsonWebToken(dbUser, roles);
                string jwt = userJWT.ToString(Utilities.GetSetting("JWTSecret"));
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    Token = jwt,
                    User = new UserDTO(dbUser),
                    Roles = roles
                });
            }
        }

        [HttpPost]
        [TokenAuth]
        [Route("api/auth/passwordreset/")]
        public async Task<IHttpActionResult> ResetPassword(ResetUserPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            User currentUser = _dataFactory.Users.GetById(User.Identity.Name);
            if (currentUser == null)
            {
                return BadRequest();
            }
            // Try logging in as current user with supplied old password
            User u = this.UserManager.Find(currentUser.UserName, model.OldPassword);
            if (u == null)
            {
                // No user with userName/password exists.
                return BadRequest("Could not authenticate");
            }

            try
            {
                String hashedNewPassword = this.UserManager.PasswordHasher.HashPassword(model.Password);
                await this.UserStore.SetPasswordHashAsync(u, hashedNewPassword);
                await this.UserStore.UpdateAsync(u);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Could not reset password");
            }
        }
    }
}
