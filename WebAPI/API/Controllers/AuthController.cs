using API.Models;
using API.Service;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using API.Validation;
using FirebaseAdmin.Auth;
using Google.Api.Gax;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class AuthController : ApiController
    {
        private readonly AuthService service = new AuthService(new ApplicationDbContext());

        [HttpPost]
        public HttpResponseMessage Login([FromBody] string fireBaseToken)
        {

            FirebaseToken decodedToken = service.LoginTokenAsync(fireBaseToken).Result;
            ApplicationUser currentUser = service.GetUser(decodedToken.Uid);
            CookieHeaderValue cookie = new CookieHeaderValue("token", currentUser.Token)
            {
                Expires = DateTimeOffset.Now.AddYears(1),
                Path = "/",
                Domain = Request.RequestUri.Host,
                Secure = false,
                HttpOnly = false,
                
            };
            UserDTO user = new UserDTO(currentUser);

            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<UserDTO>(user, GlobalConfiguration.Configuration.Formatters.JsonFormatter),
            };
            resp.Headers.AddCookies(new[] {cookie});

            return resp;
        }

        [HttpGet]
        public HttpResponseMessage Logout()
        {
            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);

            CookieHeaderValue cookie = new CookieHeaderValue("token", "")
            {
                Expires = DateTimeOffset.Now.AddDays(-1),
                Path = "/",
                Domain = Request.RequestUri.Host,
                Secure = false,
                HttpOnly = false,
            };

            resp.Headers.AddCookies(new[] {cookie});
            resp.Content = new StringContent("logged out");
            return resp;
        }

        [CookieValidation]
        [HttpPost]
        public string TestCookie()
        {
            ApplicationUser user = service.GetUserWithToken(Request);

            return user.Name;
        }
    }
}
