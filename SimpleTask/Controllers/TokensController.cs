using SimpleTask.Models;
using SimpleTask.Models;
using SimpleTask.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace SimpleTask.Controllers
{

    public class TokensController : ApiController
    {
        private SimpleTaskContext db = new SimpleTaskContext();
        private UsersServices uServices = new UsersServices();

        [HttpPost]
        [Route("api/tokens")]
        public IHttpActionResult Authenticate([FromBody] LoginRequest login)
        {
            var loginResponse = new LoginResponse { };
            LoginRequest loginrequest = new LoginRequest { };
            loginrequest.Username = login.Username.ToLower();
            loginrequest.Password = login.Password;

            User foundUser = db.Users.Where(u => u.UserName == loginrequest.Username && u.Password == loginrequest.Password).FirstOrDefault();

            if (foundUser == null)
            {
                return NotFound();
            }

            string token = uServices.createToken(loginrequest.Username);
            //return the token
            var map = new Dictionary<string, string>();
            map.Add("token", token);
            return Ok<Dictionary<string, string>>(map);

        }



    }
}
