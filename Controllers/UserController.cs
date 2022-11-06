using JwtApp.Context;
using JwtApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Contxt _db;
        [ActivatorUtilitiesConstructor]
        public UserController(Contxt db)
        {
            _db = db;
        }
        // GET: api/<UserController>
        [HttpGet]
        [Authorize(Roles = "Seller")]
        public IEnumerable<UserModel> GetUsers()
        {
            return _db.Users;
        }

        // GET api/<UserController>/5
        [HttpGet]
        public UserModel GetUSer(String username)
        {
            return _db.Users.SingleOrDefault(x => x.Username == username);
        }



        // POST api/<UserController>
        [HttpPost]
        public IActionResult Post([FromBody] UserModel user)
        {
            UserModel userr = _db.Users.FirstOrDefault(x => x.Username == user.Username || x.EmailAddress==user.EmailAddress);
            if (userr != null) return BadRequest("username Founded || Email Founded");
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = passwordHash;
            _db.Users.Add(user);
            _db.SaveChanges();
            return Ok("Hello " + user.Username+", Now Have Account in EgyptReel");
        }

        // PUT api/<UserController>/5
        [HttpPut]
        [Authorize(Roles = "Seller")]
        public IActionResult Put([FromBody] UserModel user)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = passwordHash;
            _db.Users.Update(user);
            _db.SaveChanges();
            return Ok( user.Username + ",Successfully Will change account Details");
        }

        // DELETE api/<UserController>/5
        [HttpDelete]
        [Authorize(Roles = "Seller")]
        public IActionResult Delete(String username)
        {
            UserModel userr = _db.Users.FirstOrDefault(x => x.Username == username);
            if (userr == null) return BadRequest("username not Founded");
            var user= _db.Users.SingleOrDefault(x => x.Username == username);
            if (user != null)
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
                return Ok(user.Username + ",see You Soon");
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
