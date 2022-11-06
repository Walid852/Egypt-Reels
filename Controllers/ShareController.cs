using Microsoft.AspNetCore.Mvc;
using JwtApp.Context;
using JwtApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JwtApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShareController : ControllerBase
    {
        private readonly Contxt _db;
        [ActivatorUtilitiesConstructor]
        public ShareController(Contxt db)
        {
            _db = db;
        }
        // GET: api/<UserController>
        [HttpGet]
        [Authorize(Roles = "Seller")]
        public IEnumerable<Share> GetShares()
        {
            return _db.Shares;
        }

        // GET api/<UserController>/5
        [HttpGet]
        [Authorize(Roles = "Seller")]
        public IEnumerable<Share> GetSharesofReel(int id)
        {
            return _db.Shares.Where(x => x.ReelId == id);
        }
        [HttpGet]
        [Authorize(Roles = "Seller")]
        public IEnumerable<Share> GetSharesofReelForUser(string username)
        {
            return _db.Shares.Where(x => x.username == username);
        }
        
        // POST api/<UserController>
        [HttpPost]
        [Authorize(Roles = "Seller")]
        public IActionResult ShareReel([FromBody] Share share)
        {
            UserModel user = _db.Users.FirstOrDefault(x => x.Username == share.username);
            if (user == null) return NotFound("username NotFound");
            Reel reell = _db.Reels.FirstOrDefault(x => x.ReelId == share.ReelId);
            if (reell == null) return NotFound("this Reel NotFound");
            ReelController reelController = new ReelController(_db);
            var reel = reelController.GetReel(share.ReelId);
            reel.NumberOfShare += 1;
            reelController.Put(reel);

            _db.Shares.Add(share);
            _db.SaveChanges();
            return Ok(share.username + ",Share Reel" + share.ReelId);
        }

       
        // DELETE api/<UserController>/5
        [HttpDelete]
        [Authorize(Roles = "Seller")]
        public IActionResult DeletShareForReel([FromBody] Share share)
        {

            var shareDeleted = _db.Shares.SingleOrDefault(x => x.ReelId == share.ReelId &&
            x.username == share.username);
            if (shareDeleted != null)
            {
                ReelController reelController = new ReelController(_db);
                var reel = reelController.GetReel(share.ReelId);
                reel.NumberOfShare -= 1;
                reelController.Put(reel);
                _db.Shares.Remove(shareDeleted);
                _db.SaveChanges();
                return Ok(share.username + ",Delete share action in Reel" + share.ReelId);
            }
            else
            {
                return BadRequest("no");
            }
        }
    }
}
