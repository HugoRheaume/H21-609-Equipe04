using API.Models;
using API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class AuthController : ApiController
    {
        private AuthService service = new AuthService(new ApplicationDbContext());

        [HttpPost]
        public IHttpActionResult Token([FromUri(Name = "token")] string token)
        {
            UserDTO user = service.LoginTokenAsync(token).Result;

            return Ok(user);
        }
    }
}
