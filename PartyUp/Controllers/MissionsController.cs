using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PartyUp.Data;
using PartyUp.Models;
using PartyUp.Models.DTO;
using PartyUp.Filters;

namespace PartyUp.Controllers
{
    public class MissionsController : ApiController
    {
        private ApplicationDataFactory _appDataFactory = new ApplicationDataFactory();

        // GET: api/Missions
        public async Task<IHttpActionResult> GetMissions()
        {
            IQueryable<Mission> missions = await _appDataFactory.Missions.GetAllAsync();
            List<MissionDTO> missionDTOs = new List<MissionDTO>();
            foreach (Mission m in missions)
            {
                missionDTOs.Add(new MissionDTO(m));
            }
            return Ok(new
            {
                missions = missionDTOs
            });
        }

        [Route("api/communities/{communityId:int}/missions")]
        public async Task<IHttpActionResult> GetMissionsByCommunity(int communityId)
        {
            IQueryable<Mission> query = await _appDataFactory.Missions.GetAllAsync();
            List<MissionDTO> missionDTOs = new List<MissionDTO>();
            IEnumerable<Mission> missions = query
                            .Include(m => m.Community)
                            .Where(m => m.Community.Id == communityId)
                            .OrderBy(m => m.Name)
                            .ToList();
            foreach (Mission m in missions)
            {
                missionDTOs.Add(new MissionDTO(m));
            }
            return Ok(new
            {
                missions = missionDTOs
            });
        }

        [Route("api/communities/{communityId:int}/missions/search")]
        public async Task<IHttpActionResult> GetMissionsByCommunity(int communityId, [FromUri]string q)
        {
            IQueryable<Mission> query = await _appDataFactory.Missions.GetAllAsync();
            List<MissionDTO> missionDTOs = new List<MissionDTO>();
            IEnumerable<Mission> missions = query
                            .Include(m => m.Community)
                            .Where(m => m.Community.Id == communityId)
                            .Where(m => String.IsNullOrEmpty(q) || m.Name.ToLower().Contains(q.ToLower()))
                            .OrderBy(m => m.Name)
                            .ToList();
            foreach (Mission m in missions)
            {
                missionDTOs.Add(new MissionDTO(m));
            }
            return Ok(new
            {
                missions = missionDTOs
            });
        }

        // GET: api/Missions/5
        [ResponseType(typeof(MissionDTO))]
        public async Task<IHttpActionResult> GetMission(int id)
        {
            Mission mission = await _appDataFactory.Missions.FindAsync(id);
            if (mission == null)
            {
                return NotFound();
            }

            return Ok(new MissionDTO(mission));
        }

        // PUT: api/Missions/5
        [TokenAuth(Roles = "Admin, Moderator")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMission(int id, MissionDTO missionDTO)
        {
            if (id != missionDTO.Id)
            {
                return BadRequest();
            }

            Mission dbMission = await _appDataFactory.Missions.FindAsync(missionDTO.Id);
            if (dbMission == null)
            {
                return NotFound();
            }
            Mission mission = missionDTO.UpdateDbModel(dbMission);

            _appDataFactory.Missions.Update(mission);

            try
            {
                await _appDataFactory.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!MissionExists(id))
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

        // POST: api/Missions
        [TokenAuth(Roles = "Admin, Moderator")]
        [ResponseType(typeof(MissionDTO))]
        [Route("api/communities/{communityId:int}/missions/createnew")]
        public async Task<IHttpActionResult> PostMission(int communityId, MissionDTO missionDTO)
        {
            Mission mission = missionDTO.ToModel();
            Community c = await _appDataFactory.Communities.FindAsync(communityId);
            mission.Community = c;
            _appDataFactory.Missions.Add(mission);
            await _appDataFactory.SaveChangesAsync();

            return Ok(new MissionDTO(mission));
        }

        // DELETE: api/Missions/5
        [TokenAuth(Roles = "Admin, Moderator")]
        [ResponseType(typeof(Mission))]
        public async Task<IHttpActionResult> DeleteMission(int id)
        {
            if (!MissionExists(id))
            {
                return NotFound();
            }

            _appDataFactory.Missions.Delete(id);
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

        private bool MissionExists(int id)
        {
            return _appDataFactory.Missions.Exists(id);
        }
    }
}