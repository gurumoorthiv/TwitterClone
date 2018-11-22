using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterClone.Models
{
    public class Tweet
    {
        public int TweetId { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
    }
}