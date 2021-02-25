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
using API.Models.Question;
using Google.Api.Gax;

namespace API.Service
{
    public class AuthService : BaseService
    {
        public AuthService(ApplicationDbContext db) : base(db) { }

        public FirebaseToken LoginToken(string tokenFromUser)
        {
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
                userManager.Create(user);

            }
            user.Token = GenerateCustomToken();
            db.SaveChanges();

            return decodedToken;
        }

        public ApplicationUser GetUser(string id)
        {
            return db.Users.Find(id);
        }

        public virtual ApplicationUser GetUserWithToken(HttpRequestMessage request)
        {
            string token = request.Headers.Authorization.ToString().Split(' ')[1];
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Token == token);

            return user;
        }

        public virtual ApplicationUser GetUserWithToken(string token)
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

        
        public UserDTO GenerateAnonymousUser(string username)
        {
            string token = GenerateCustomToken();
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser anonymous = new ApplicationUser
            {
                UserName = GenerateCustomToken(),
                Name = username,
                Picture = "../../assets/png_64/" + Global.random.Next(1, 15) + ".png",
                Token = token
            };
            //anonymous.Picture = "https://www.minervastrategies.com/wp-content/uploads/2016/03/default-avatar.jpg";
            userManager.Create(anonymous);

            db.SaveChanges();
            return new UserDTO(anonymous);
        }

        public bool DeleteAnonymous(string token)
        {
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Token == token);
            if (user != null && user.Email == null)
            {
                db.QuestionResult.RemoveRange(db.QuestionResult.Where(q => q.User.Id == user.Id).ToList());
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}