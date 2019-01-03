using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDS4.AspIdentity.Models
{
    public class Login
    {
        public string Token { get; set; }
        public string Client { get; set; }
        public string Secret { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Scope { get; set; }
    }
}
