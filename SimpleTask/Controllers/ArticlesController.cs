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
using SimpleTask.DTO;


namespace SimpleTask.Controllers
{
    [Authorize]
    [RoutePrefix("api/{username}/articles")]
    public class ArticlesController : ApiController
    {
        private SimpleTaskContext db = new SimpleTaskContext();
        // GET api/{username}/articles
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetArticles(string username)
        {
            var requesterUsername = User.Identity.Name;
            if (requesterUsername != username)
            {
                return Unauthorized();
            }
            var OwnerUser = db.Users.Where(u => u.UserName == username).FirstOrDefault();
            var output = db.Articles.Where(a => a.UserId == OwnerUser.Id).Select(a => new ArticleOutDTO()
            {
                Id=a.Id,
                Title = a.Title,
                Text = a.Text
            });
            return Ok(output);
        }

        // GET api/{username}/articles?id=5
        [Route("", Name = "GetArticle")]
        [ResponseType(typeof(Article))]
        public async Task<IHttpActionResult> GetArticle(string username,[FromUri]long id)
        {
            var requesterUsername = User.Identity.Name;
            if (requesterUsername != username)
            {
                return Unauthorized();
            }
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            var owner = db.Users.Where(u => u.UserName == requesterUsername).FirstOrDefault();
            if (article.UserId != owner.Id) {
                return Unauthorized();
            }
            return Ok(new ArticleOutDTO(){ Id=article.Id, Title=article.Title,Text=article.Text });
        }

        // PUT api/{username}/articles?id=5
        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> PutAtricle(string username, [FromUri] long id, Article article)
        {
            var requesterUsername = User.Identity.Name;
            if (requesterUsername != username)
            {
                return Unauthorized();
            }
            var found = db.Articles.Include(a => a.User).Where(a => a.Id == id).FirstOrDefault();
            if (found == null)
            {
                return NotFound();
            }
            if (found.User.UserName != requesterUsername)
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            found.Title = article.Title;
            found.Text = article.Text;

            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/{username}/articles
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Article))]
        public async Task<IHttpActionResult> PostAtricle(string username, Article article)
        {
            var requesterUsername = User.Identity.Name;
            if (requesterUsername != username)
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            article.UserId = db.Users.Where(u => u.UserName == username).FirstOrDefault().Id;
            db.Articles.Add(article);
            await db.SaveChangesAsync();

            return CreatedAtRoute("GetArticle", new { id = article.Id }, article);
        }

        // DELETE api/{username}/articles?id=5
        [HttpDelete]
        [Route("")]
        [ResponseType(typeof(Article))]
        public async Task<IHttpActionResult> DeleteArticle(string username, [FromUri] long id)
        {
            var requesterUsername = User.Identity.Name;
            if (requesterUsername != username)
            {
                return Unauthorized();
            }
            Article atricle = await db.Articles.FindAsync(id);
            if (atricle == null)
            {
                return NotFound();
            }

            db.Articles.Remove(atricle);
            await db.SaveChangesAsync();

            return Ok(atricle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AtricleExists(long id)
        {
            return db.Articles.Count(e => e.Id == id) > 0;
        }
    }
}