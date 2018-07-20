using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace SimpleTask.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public HttpResponseMessage responseMsg { get; set; }
        public LoginResponse()
        {

            this.Token = "";
            this.responseMsg = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Unauthorized };
        }
    }
}