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
    public class LikesController : ControllerBase
    {
        private readonly Contxt _db;
        [ActivatorUtilitiesConstructor]
        public LikesController(Contxt db)
        {
            _db = db;
        }
        // GET: api/<UserController>
        [HttpGet]
        [Authorize(Roles = "Seller")]
        public IEnumerable<Likes> GetLikes()
        {
            return _db.Likes;
        }

        // GET api/<UserController>/5
        [HttpGet]
        [Authorize(Roles = "Seller")]
        public IEnumerable<Likes> GetLikesofReel(int  id)
        {
            return _db.Likes.Where(x => x.ReelId == id);
        }

        // POST api/<UserController>
        [HttpPost]
        [Authorize(Roles = "Seller")]
        public IActionResult AddLikeForReel ([FromBody] Likes Like)
        {
            UserModel user = _db.Users.FirstOrDefault(x => x.Username == Like.username);
            if (user == null) return NotFound("username NotFound");
            Reel reell = _db.Reels.FirstOrDefault(x => x.ReelId == Like.ReelId);
            if (reell == null) return NotFound("this Reel NotFound");
            ReelController reelController=new ReelController(_db);
            var reel=reelController.GetReel(Like.ReelId);
            reel.NumberOfLikes += 1;
            reelController.Put(reel);

            _db.Likes.Add(Like);
            _db.SaveChanges();
            return Ok(Like.username + ",like Reel"+Like.ReelId);
        }

        // PUT api/<UserController>/5
        [HttpPut]
        [Authorize(Roles = "Seller")]
        public IActionResult ChangeStatusLike([FromBody] Likes Like)
        {
            _db.Likes.Update(Like);
            _db.SaveChanges();
            if(Like.IsLike==false) return Ok(Like.username + ",Unlike Reel" + Like.ReelId);
            else return Ok(Like.username + ",like Reel" + Like.ReelId);

        }
        // DELETE api/<UserController>/5
        [HttpDelete]
        [Authorize(Roles = "Seller")]
        public IActionResult DeletLikeFromReel([FromBody] Likes Like)
        {
           
            var LikeDeleted = _db.Likes.SingleOrDefault(x => x.ReelId == Like.ReelId &&
            x.username==Like.username);
            if (LikeDeleted != null)
            {
                ReelController reelController = new ReelController(_db);
                var reel = reelController.GetReel(Like.ReelId);
                reel.NumberOfLikes -= 1;
                reelController.Put(reel);
                _db.Likes.Remove(LikeDeleted);
                _db.SaveChanges();
                return Ok(Like.username + ",Delete Like action in Reel" + Like.ReelId);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
