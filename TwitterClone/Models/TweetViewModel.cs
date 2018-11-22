using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterClone.Models
{
    public class TweetViewModel
    {
        public List<Tweet> Tweets { get; set; }
        public string Message { get; set; }
        public int Following { get; set; }
        public int Followers { get; set; }
    }
}