using API.Models;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Google.Api.Gax;

namespace API.Service
{
    public class AuthService : BaseService
    {
        public AuthService(ApplicationDbContext db) : base(db) { }

        public async Task<FirebaseToken> LoginTokenAsync(string tokenFromUser)
        {
            //string appPath = HttpRuntime.AppDomainAppPath;
            //string path = appPath + "quizplay-eq4-firebase-adminsdk-lokv6-e158b65c6f.json";
            //if (FirebaseApp.DefaultInstance == null)
            //    FirebaseApp.Create(new AppOptions()
            //    {
            //        Credential = GoogleCredential.FromFile(path),
            //    });

            FirebaseToken decodedToken = FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(tokenFromUser).Result;
            string uid = decodedToken.Uid;

            ApplicationUser user = db.Users.Find(uid);
            if (user == null)
            {
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                user = new ApplicationUser
                {
                    Id = uid,
                    Email = decodedToken.Claims["email"].ToString()
                };
                user.UserName = user.Email;
                user.Name = decodedToken.Claims["name"].ToString();
                user.Picture = decodedToken.Claims["picture"].ToString();
                await userManager.CreateAsync(user);

            }
            user.Token = GenerateCustomToken();
            db.SaveChanges();

            return decodedToken;
        }

        public ApplicationUser GetUser(string id)
        {
            return db.Users.Find(id);
        }

        public ApplicationUser GetUserWithToken(HttpRequestMessage request)
        {
            string token = request.Headers.Authorization.ToString().Split(' ')[1];
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Token == token);

            return user;
        }

        public ApplicationUser GetUserWithToken(string token)
        {
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Token == token);

            return user;
        }

        public string GenerateCustomToken()
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 64)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}