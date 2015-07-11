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
            User currentUser = await _dataFactory.Users.FindAsync(User.Identity.Name);
            if (currentUser == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid username/password");
            }
            else
            {
                // ROLES
                IEnumerable<string> roles = UserManager.GetRoles(currentUser.Id);

                // RECENTLY HOSTED EVENTS
                IEnumerable<Event> recentlyHostedEvents = _dataFactory.Events.GetRecentlyHostedEvents(currentUser.Id);
                List<EventDTO> recentlyHostedEventsDTO = new List<EventDTO>();
                foreach (Event e in recentlyHostedEvents)
                {
                    try
                    {
                        recentlyHostedEventsDTO.Add(new EventDTO(e));
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                }

                // RECENTLY JOINED
                IEnumerable<Event> recentlyJoinedEvents = _dataFactory.Events.GetRecentlyJoinedEvents(currentUser.Id);
                List<EventDTO> recentlyJoinedEventsDTO = new List<EventDTO>();
                foreach (Event e in recentlyJoinedEvents)
                {
                    try
                    {
                        recentlyJoinedEventsDTO.Add(new EventDTO(e));
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    User = new UserDTO(currentUser),
                    Roles = roles,
                    RecentlyHosted = recentlyHostedEventsDTO,
                    RecentlyJoined = recentlyJoinedEventsDTO
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
                User dbUser = await _dataFactory.Users.FindAsync(u.Id);
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
            User currentUser = _dataFactory.Users.Find(User.Identity.Name);
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

        [HttpGet]
        [Route("api/auth/checkusername")]
        public IHttpActionResult CheckUsername([FromUri]string username)
        {
            if (UserWithSameUserName(username))
                return Ok("USED");
            else
                return Ok("OK");
        }

        [HttpGet]
        [Route("api/auth/checkusername/loggedin")]
        [TokenAuth]
        public IHttpActionResult CheckUsernameLoggedIn([FromUri]string username)
        {
            if (UserWithSameUserName(username, User.Identity.Name))
                return Ok("USED");
            else
                return Ok("OK");
        }

        private bool UserWithSameUserName(string username, string userId = "NO_WAY")
        {
            if (String.IsNullOrEmpty(username))
            {
                return true;
            }
            User user = _dataFactory.Users.GetAll()
                .FirstOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (user == null || user.Id == userId)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [HttpGet]
        [Route("api/auth/checkemail")]
        public IHttpActionResult CheckEmail([FromUri]string email)
        {
            if (UserWithSameEmail(email))
                return Ok("USED");
            else
                return Ok("OK");
        }

        [HttpGet]
        [Route("api/auth/checkemail/loggedin")]
        [TokenAuth]
        public IHttpActionResult CheckEmailLoggedIn([FromUri]string email)
        {
            if (UserWithSameEmail(email, User.Identity.Name))
                return Ok("USED");
            else
                return Ok("OK");
        }

        private bool UserWithSameEmail(string email, string userId = "NO_WAY")
        {
            if (String.IsNullOrEmpty(email))
            {
                return true;
            }
            User user = _dataFactory.Users.GetAll()
                .FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user == null || user.Id == userId)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [HttpPost]
        [Route("api/auth/register")]
        public async Task<IHttpActionResult> Register(RegisterUserDTO newUser)
        {
            User user = newUser.ToModel();
            user.CreatedOn = DateTime.UtcNow;
            user.ModifiedOn = DateTime.UtcNow;
            var result = await UserManager.CreateAsync(user, newUser.Password);
            if (result.Succeeded)
            {
                User u = await this.UserManager.FindByNameAsync(newUser.UserName);
                u = newUser.UpdateDbModel(u);
                _dataFactory.Users.Update(u);
                await _dataFactory.SaveChangesAsync();
                await UserManager.AddToRoleAsync(u.Id, "Basic");
                return Ok(new UserDTO(u));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/auth/update")]
        [TokenAuth]
        public async Task<IHttpActionResult> UpdateUser(UserDTO newUser)
        {
            User dbUser = await _dataFactory.Users.FindAsync(newUser.Id);
            if (dbUser == null)
            {
                return NotFound();
            }

            User updatedUser = newUser.UpdateDbModel(dbUser);
            _dataFactory.Users.Update(updatedUser);
            await _dataFactory.SaveChangesAsync();

            return Ok();
        }
    }
}
