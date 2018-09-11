using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDS4.MVC
{
    public class TestUser
    {

        public int SubjectId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
    {
        new TestUser
        {
            SubjectId = 1,
            Username = "alice",
            Password = "password"
        },
        new TestUser
        {
            SubjectId = 2,
            Username = "bob",
            Password = "password"
        }
    };

        }
    }
}

