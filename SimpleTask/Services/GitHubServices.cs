using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace SimpleTask.Services
{
    public class GitHubServices
    {
        GitHubClient client = new GitHubClient(new ProductHeaderValue("simple-task"));

        public async Task<IReadOnlyList<Repository>> GetRepos(string username)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var user = await client.User.Get(username);
            var allRepos = await client.Repository.GetAllForUser(username);
            return allRepos;
        }
        public async Task<bool> ValidateGithubUserName(string username)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                var foundGithubUser = await client.User.Get(username);
                if (foundGithubUser == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}