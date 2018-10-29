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
            return View();
        }

        [HttpPost]
        public ActionResult Login()
        {
            var username = Request["Username"];
            var password = Request["Password"];
            if (username == "test" && password == "test")
            {
                FormsAuthentication.SetAuthCookie("test", false);
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