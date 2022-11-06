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
    public class CommentController : ControllerBase
    {
        private readonly Contxt _db;
        [ActivatorUtilitiesConstructor]
        public CommentController(Contxt db)
        {
            _db = db;
        }
        // GET: api/<UserController>
        [HttpGet]
        [Authorize(Roles = "Seller")]
        public IEnumerable<Comments> GetComments()
        {
            return _db.Comments;
        }
        [HttpGet]
        [Authorize(Roles = "Seller")]
        public Comments GetComment(int id)
        {
            return _db.Comments.SingleOrDefault(x => x.CommentId == id);
        }

        // GET api/<UserController>/5
        [HttpGet]
        [Authorize(Roles = "Seller")]
        public IEnumerable<Comments> GetCommentsofReel(int id)
        {
            return _db.Comments.Where(x => x.ReelId == id);
        }

        // POST api/<UserController>
        [HttpPost]
        [Authorize(Roles = "Seller")]
        public IActionResult AddCommentForReel([FromBody] Comments comments)
        {
            UserModel user = _db.Users.FirstOrDefault(x => x.Username == comments.username);
            if (user == null) return NotFound("username NotFound");
            Reel reell = _db.Reels.FirstOrDefault(x => x.ReelId == comments.ReelId);
            if (reell == null) return NotFound("this Reel NotFound");
            ReelController reelController = new ReelController(_db);
            var reel = reelController.GetReel(comments.ReelId);
            reel.NumberOfComments += 1;
            reelController.Put(reel);

            _db.Comments.Add(comments);
            _db.SaveChanges();
            return Ok(comments.username + ",comment for Reel" + comments.ReelId);
        }

        // PUT api/<UserController>/5
        [HttpPut]
        [Authorize(Roles = "Seller")]
        public IActionResult Put([FromBody] Comments comments)
        {
            _db.Comments.Update(comments);
            _db.SaveChanges();
             return Ok(comments.username + ",tou Change your Commment in reel" + comments.ReelId);

        }
        [HttpPut]
        [Authorize(Roles = "Seller")]
        public IActionResult ChangeComment(int id,string newcomment)
        {
            Comments comment = GetComment(id);
            comment.Comment=newcomment;
            Put(comment);

            return Ok(comment.username + ",tou Change your Commment in reel" + comment.ReelId);

        }
        // DELETE api/<UserController>/5
        [HttpDelete]
        [Authorize(Roles = "Seller")]
        public IActionResult DeletCommentForReel( int id)
        {

            var CommentDeleted = _db.Comments.SingleOrDefault(x => x.CommentId == id);
            if (CommentDeleted != null)
            {
                ReelController reelController = new ReelController(_db);
                var reel = reelController.GetReel(CommentDeleted.ReelId);
                reel.NumberOfComments -= 1;
                reelController.Put(reel);
                _db.Comments.Remove(CommentDeleted);
                _db.SaveChanges();
                return Ok(CommentDeleted.username + ",Delete Comment  in Reel" + CommentDeleted.ReelId);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
