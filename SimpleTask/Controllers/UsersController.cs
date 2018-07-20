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
using SimpleTask.Models;
using SimpleTask.Services;

namespace SimpleTask.Controllers
{
    public class UsersController : ApiController
    {
        private SimpleTaskContext db = new SimpleTaskContext();
        private UsersServices uServices = new UsersServices();
        GitHubServices GithubServices = new GitHubServices();

        // GET api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        [Authorize]
        // GET api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(long id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize]
        // PUT api/Users/5
        public async Task<IHttpActionResult> PutUser(long id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var found = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (found == null) {
                return NotFound();
            }
            found.FullName = user.FullName;
            found.Password = user.Password;
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool isGithubUsernameValid = await GithubServices.ValidateGithubUserName(user.UserName);
            if (!isGithubUsernameValid)
            {
                return BadRequest("Could not find any Github account for this UserName");
            }
            var found=db.Users.Where(u => u.UserName == user.UserName).FirstOrDefault();
            if (found != null) {
                return BadRequest("UserName is taken");
            }
            db.Users.Add(user);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        [Authorize]
        // DELETE api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(long id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(long id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}