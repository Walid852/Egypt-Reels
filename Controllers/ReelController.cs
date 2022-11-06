using JwtApp.Context;
using JwtApp.Models;
using JwtApp.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JwtApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReelController : ControllerBase
    {
        IWebHostEnvironment webHostEnvironment;
        private readonly Contxt _db;
        [ActivatorUtilitiesConstructor]
        public ReelController(Contxt db)
        {
            _db = db;
        }
        // GET: api/<UserController>
        [HttpGet]
        [Authorize(Roles = "Seller")]
        public IEnumerable<ViewReel> GetReels()
        {
            IEnumerable<Reel> reels = _db.Reels;
            List<ViewReel> Output = GetViewReel(reels);
            return Output;
        }

        public List<ViewReel> GetViewReel(IEnumerable<Reel> reels)
        {
            List<ViewReel> Output = new List<ViewReel>();
            foreach (var reell in reels)
            {
                ViewReel viewReel = new ViewReel();
                viewReel.reel = reell;
                Videos V = _db.videos.SingleOrDefault(x => x.VideoId == reell.VideoId);
                string decoded = EncodedAndDecoded.DecodeFrom64(V.Url);
                viewReel.VideoUrl = decoded;

                Output.Add(viewReel);
            }
            return Output;

        }
       [HttpGet]
        [Authorize(Roles = "Seller")]
        public List<ViewReel> GetReelsForUser(string username)
        {
            var createdReels = _db.Reels.Where(x => x.Username == username);
            List<Reel> reels = createdReels.ToList();
            ShareController shareController = new ShareController(_db);
            var shares=shareController.GetSharesofReelForUser(username);
            if (shares.Any())
            {
                foreach (var share in shares)
                {
                    reels.Add(GetReel(share.ReelId));
                }
            }
            
            return GetViewReel(reels);
        }
        public Reel GetReel(int id)
        {
            return _db.Reels.SingleOrDefault(x => x.ReelId == id);
        }
        // GET api/<UserController>/5
        [HttpGet]
        [Authorize(Roles = "Seller")]
        public ViewReel GetReelview(int id)
        {
            Reel reell= _db.Reels.SingleOrDefault(x => x.ReelId == id);
            ViewReel viewReel = new ViewReel();
            viewReel.reel = reell;
            Videos V = _db.videos.SingleOrDefault(x => x.VideoId == reell.VideoId);
            string decoded = EncodedAndDecoded.DecodeFrom64(V.Url);
            viewReel.VideoUrl = decoded;
            return viewReel;
        }

        [HttpGet]
        [Authorize(Roles = "Seller")]
        public IEnumerable<ViewReel> GetReelsForGovernments(string government) {
            PropertiesOfReels POP = _db.PropertiesOfReels.SingleOrDefault(x=>x.Government== government);
            var reels = _db.Reels.Where(x => x.propertyId == POP.PropertyId);
            return GetViewReel(reels);
        }

         // POST api/<UserController>
        [HttpPost]
        [Authorize(Roles = "Seller")]
        public IActionResult Post([FromForm] Reel reel, [FromForm] FileUpload fileUpload, [FromForm] PropertiesOfReels propertiesOfReels)
        {
            UserModel user=_db.Users.FirstOrDefault(x => x.Username == reel.Username);
            if (user == null) return NotFound("username NotFound");
            FileUploadsController uploadController = new FileUploadsController();
             string url=uploadController.Post(fileUpload);
            string encodedurl = EncodedAndDecoded.EncodeToBase64(url);
            while (true)
            {
                
                Videos video = _db.videos.FirstOrDefault(x => x.Url == encodedurl);
                if (video == null)
                {
                    Videos newVideo = new Videos();
                    newVideo.Url = encodedurl;
                    _db.videos.Add(newVideo);
                    _db.SaveChanges();
                    
                }
                else
                {
                    reel.VideoId = video.VideoId;
                    break;
                }
            }
            while (true)
            {
                PropertiesOfReels properties = _db.PropertiesOfReels.
                    SingleOrDefault(x => x.Government==propertiesOfReels.Government);
                if (properties == null)
                {
                    PropertiesOfReels newproperties = new PropertiesOfReels();
                    newproperties.Government = propertiesOfReels.Government;
                    _db.PropertiesOfReels.Add(newproperties);
                    _db.SaveChanges();

                }
                else
                {
                    reel.propertyId = properties.PropertyId;
                    break;
                }
            }
            
            _db.Reels.Add(reel);
            _db.SaveChanges();
            return Ok("Hello " +reel.Username + ",Now you Creat Reel have Reel ID"+reel.ReelId+" in EgyptReel");
        }

        // PUT api/<UserController>/5
        [HttpPut]
        [Authorize(Roles = "Seller")]
        public IActionResult Put([FromBody] Reel reel)
        {
            _db.Reels.Update(reel);
            _db.SaveChanges();
            return Ok(reel.Username + ",Successfully Will change reel Details");
        }
        
        // DELETE api/<UserController>/5
        [HttpDelete]
        [Authorize(Roles = "Seller")]
        public IActionResult Delete(int  id)
        {
            var reel = _db.Reels.SingleOrDefault(x => x.ReelId == id);
            if (reel != null)
            {
                _db.Reels.Remove(reel);
                _db.SaveChanges();
                return Ok(reel.ReelId + ",Deleted");
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
