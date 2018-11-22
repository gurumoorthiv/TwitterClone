using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TwitterClone.Models;

namespace TwitterClone.Controllers
{
    using System.Web.UI;

    [Authorize]
    public class TweetController : Controller
    {
       public TweetController()
        {
        }

        //
        // GET: /Account/Login
        [HttpGet]
        public ActionResult Home(string user)
        {
            DatabaseOperation db = new DatabaseOperation();
            var tweetViewModel = new TweetViewModel();
            tweetViewModel.Tweets = db.RetriveTweet(User.Identity.Name);
            tweetViewModel.Followers = db.RetriveFollowers(User.Identity.Name);
            tweetViewModel.Following = db.RetriveFollowing(User.Identity.Name);
            return View(tweetViewModel);
        }

        [HttpPost]
        public ActionResult Home(TweetViewModel model, FormCollection frm)
        {
            DatabaseOperation db = new DatabaseOperation();
            var message = Convert.ToString(frm["Message"]);
            db.SaveTweet(User.Identity.Name, message);
            model.Tweets = db.RetriveTweet(User.Identity.Name);
            model.Message = string.Empty;
            return View(model);
        }

        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 2)]
        public ActionResult UpdateTweets()
        {
            DatabaseOperation db = new DatabaseOperation();
            var Tweets = db.RetriveTweet(User.Identity.Name);
            return PartialView("_TweetsPartial", Tweets);
        }

        [HttpGet]
        //[HttpPost]
        public ActionResult Search(string search)
        {
            DatabaseOperation db = new DatabaseOperation();
            object userName = search;
            var result = db.SearchUser(Convert.ToString(userName));
            var person = new Person();
            person.UserId = search;
            if (result)
            {
                //ViewBag.User = userName;
                return View(person);
            }
            return RedirectToAction("Home");
        }

        [HttpPost]
        public ActionResult Search(Person model)
        {
            DatabaseOperation db = new DatabaseOperation();
            db.Follow(User.Identity.Name, model.UserId);
            return RedirectToAction("Home");
        }

        [HttpGet]
        public ActionResult Profile(string user)
        {
            DatabaseOperation db = new DatabaseOperation();
            var person = db.RetrivePerson(User.Identity.Name);
            return View(person);
        }

        [HttpPost]
        public ActionResult Profile(Person model)
        {
            DatabaseOperation db = new DatabaseOperation();
            var person = db.UpdateProfile(model);
            return RedirectToAction("Home");
        }
    }
}