using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Block8_Lab1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult Login()
        {
            var username = Request["Username"];
            var password = Request["Password"];
            var stayLoggedInd = Boolean.Parse(Request["stayLoggedIn"]);

            if (username == "odermatt" && password == "1234")
            {
                ViewBag.Message = "Successfully logged in!";
                if (stayLoggedInd)
                {
                    Request.Cookies.Set(new HttpCookie(FormsAuthentication.FormsCookieName, username)
                    {
                        Expires = DateTime.Now.AddDays(14),
                        HttpOnly = true,
                        Secure = true,
                        Domain = FormsAuthentication.CookieDomain,
                        Path = FormsAuthentication.FormsCookiePath
                    });
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                }
                return RedirectToAction("Success");
            }
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}