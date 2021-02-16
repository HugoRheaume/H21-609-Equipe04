using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using API.Models;
using API.Service;

namespace API.Validation
{
    /// <summary>
    /// Valide les cookies et les headers qui sont envoyés dans la requête.
    /// Si il a des tokens, il va accepter la requête, sinon, la requête sera redirigée vers Logout()
    /// </summary>
    public class TokenAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AuthService service = new AuthService(db);

            // COOKIE (ANDROID)

            // Get token from Cookie
            string token = actionContext.Request.Headers.GetCookies("token").FirstOrDefault()?.Cookies[0].Value;
            DateTimeOffset? expiration = actionContext.Request.Headers.GetCookies("token").FirstOrDefault()?.Expires;


            // If token expired deny access
            if (expiration < DateTimeOffset.Now)
            {
                return false;
            }

            // If a user is linked to this token put the token the header
            if (service.GetUserWithToken(token) != null)
            {
                actionContext.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return true;
            }


            // HEADER (ANGULAR)
            string authorization = HttpContext.Current.Request.Headers["Authorization"];

            if (authorization == null || authorization == "Bearer")
                return false;
            
            string headerToken = authorization.Split(' ')[1];
            
            if (service.GetUserWithToken(headerToken) != null)
                return true;

            return false;
        }
    }
}