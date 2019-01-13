using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Block11_RoleBasedAuthentication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Login()
        {
            var username = Request["username"];
            var password = Request["password"];

            if (username == "admin" && password == "1234")
            {
                Session["username"] = username;
                return RedirectToAction("Dashboard", "Admin");
            }
            if (username == "user" && password == "1234")
            {
                Session["username"] = username;
                return RedirectToAction("Dashboard", "User");
            }
            ViewBag.Message = "Wrong Credentials";
            return RedirectToAction("Index");
        }
    }
}