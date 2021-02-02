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
using System.Threading.Tasks;
using System.Web;

namespace API.Service
{
    public class AuthService : BaseService
    {
        public AuthService(ApplicationDbContext db) : base(db) { }

        public async Task<UserDTO> LoginTokenAsync(string tokenFromUser)
        {
            if (FirebaseApp.GetInstance("[DEFAULT]") == null)
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("C:\\Users\\Nicolas\\Desktop\\QuizPlay\\WebAPI\\API\\quizplay-eq4-firebase-adminsdk-lokv6-c6ae35aade.json"),
                });

            FirebaseToken decodedToken1 = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(tokenFromUser);
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(tokenFromUser);
            string uid = decodedToken.Uid;

            if (db.Users.Find(uid) == null)
            {
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser user = new ApplicationUser();
                user.Id = uid;
                user.Email = decodedToken.Claims["email"].ToString();
                user.UserName = decodedToken.Claims["name"].ToString();
                user.Picture = decodedToken.Claims["picture"].ToString();
                await userManager.CreateAsync(user);
            }
            return new UserDTO(db.Users.Find(uid));
        }
    }
}