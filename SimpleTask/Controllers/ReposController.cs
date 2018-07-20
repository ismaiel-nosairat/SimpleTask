using Octokit;
using SimpleTask.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SimpleTask.Controllers
{
    [Authorize]
    public class ReposController : ApiController
    {

        GitHubServices GithubServices = new GitHubServices();
        
        [HttpGet]
        [Route("api/{username}/repos")]
        public async Task<IHttpActionResult> GetUsers(string username)
        {
            var requesterUsername = User.Identity.Name;
            if (requesterUsername != username)
            {
                return Unauthorized();
            }
            var repos = await GithubServices.GetRepos(username);
            return Ok(repos);

        }
    }
}
