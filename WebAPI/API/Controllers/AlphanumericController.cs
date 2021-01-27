using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace API.Controllers
{
    public class AlphanumericController : ApiController
    {
        private const string ALPHANUMERIC_CHARACTER_LIST = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random random = new Random();

        // api/Alphanumeric/GenerateAlphanumeric
        [HttpGet]
        public string GenerateAlphanumeric()
        {
            StringBuilder code = new StringBuilder();
            char ch;
            for (int i = 0; i < 6; i++)
            {
                ch = ALPHANUMERIC_CHARACTER_LIST[random.Next(0, ALPHANUMERIC_CHARACTER_LIST.Length)];
                code.Append(ch);
            }
            return code.ToString();
        }
    }
}
