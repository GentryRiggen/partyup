using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PartyUp.Data;
using PartyUp.Models;
using PartyUp.Models.DTO;
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
    }
}
