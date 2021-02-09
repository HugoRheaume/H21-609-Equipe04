using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public string Token { get; set; }

        public UserDTO(ApplicationUser user)
        {
            if (user == null) return;
            
            Name = user.Name;
            Email = user.Email;
            Picture = user.Picture;
            Token = user.Token;
        }
        public UserDTO() { }
    }
}