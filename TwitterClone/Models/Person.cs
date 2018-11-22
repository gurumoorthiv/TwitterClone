using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterClone.Models
{
    using System.ComponentModel;

    public class Person
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime Joined { get; set; }

        [DisplayName("Delete Account")]
        public bool Active { get; set; }
        public List<Person> Following { get; set; }
        public List<Person> Followers { get; set; }
        public List<Tweet> Tweets { get; set; } 
    }
}