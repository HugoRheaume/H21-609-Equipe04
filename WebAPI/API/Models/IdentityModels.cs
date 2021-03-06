using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Models.Question;
using API.Models.Quiz;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace API.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
        public string Name { get; set; }

        public string Picture { get; set; }
        
        public virtual List<QuestionResult> QuestionResults { get; set; }

        public string Token { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
                
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Quiz.Quiz> ListQuiz { get; set; }
        public virtual DbSet<Question.Question> Question { get; set; }
        public virtual DbSet<QuestionTrueFalse> QuestionTrueFalse { get; set; }
        public virtual DbSet<QuestionMultipleChoice> QuestionMultiple { get; set; }
        public virtual DbSet<QuestionResult> QuestionResult { get; set; }
        public virtual DbSet<QuestionAssociation> QuestionAssociation { get; set; }
        public virtual DbSet<QuestionCategory> QuestionCategory { get; set; }
        public virtual DbSet<QuizTopScore> QuizTopScore { get; set; }
    }
}