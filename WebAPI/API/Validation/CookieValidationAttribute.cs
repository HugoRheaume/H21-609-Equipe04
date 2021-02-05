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
    public class CookieValidationAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            AuthService service = new AuthService(db);

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

            string stringHeader = HttpContext.Current.Request.Headers["Authorization"]?.Split(' ')[1];
            if (service.GetUserWithToken(stringHeader) != null)
                return true;

            return false;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var response = actionContext.Request.CreateResponse(HttpStatusCode.Redirect);
            response.Headers.Location =
                new Uri(actionContext.Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/api/Auth/Logout");
            actionContext.Response = response;
        }
    }
}