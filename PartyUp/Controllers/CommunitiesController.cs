using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PartyUp.Data;
using PartyUp.Models;
using System.Threading.Tasks;
using PartyUp.Models.DTO;
using PartyUp.Filters;

namespace PartyUp.Controllers
{
    public class CommunitiesController : ApiController
    {
        private ApplicationDataFactory _appDataFactory = new ApplicationDataFactory();

        // GET: api/Communities
        public async Task<IHttpActionResult> GetCommunities()
        {
            IEnumerable<Community> communities = await _appDataFactory.Communities.GetAllAsync();
            communities = communities.OrderBy(c => c.Name);
            List<CommunityDTO> communityDTOs = new List<CommunityDTO>();
            foreach (Community c in communities)
            {
                communityDTOs.Add(new CommunityDTO(c));
            }
            return Ok(new 
            {
                communities = communityDTOs
            });
        }

        // GET: api/Communities/5
        [ResponseType(typeof(Community))]
        public async Task<IHttpActionResult> GetCommunity(int id)
        {
            Community community = await _appDataFactory.Communities.FindAsync(id);
            if (community == null)
            {
                return NotFound();
            }

            return Ok(new CommunityDTO(community));
        }

        // PUT: api/Communities/5
        [TokenAuth(Roles = "Admin, Moderator")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCommunity(int id, CommunityDTO communityDTO)
        {
            if (id != communityDTO.Id)
            {
                return BadRequest();
            }

            Community c = communityDTO.ToModel();

            _appDataFactory.Communities.Update(c);

            try
            {
                await _appDataFactory.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Communities
        [TokenAuth(Roles = "Admin, Moderator")]
        [ResponseType(typeof(Community))]
        public async Task<IHttpActionResult> PostCommunity(CommunityDTO communityDTO)
        {
            Community c = communityDTO.ToModel();
            _appDataFactory.Communities.Add(c);
            await _appDataFactory.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = c.Id }, new CommunityDTO(c));
        }

        // DELETE: api/Communities/5
        [TokenAuth(Roles = "Admin, Moderator")]
        public async Task<IHttpActionResult> DeleteCommunity(int id)
        {
            _appDataFactory.Communities.Delete(id);
            await _appDataFactory.SaveChangesAsync();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _appDataFactory.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommunityExists(int id)
        {
            return _appDataFactory.Communities.Exists(id);
        }
    }
}