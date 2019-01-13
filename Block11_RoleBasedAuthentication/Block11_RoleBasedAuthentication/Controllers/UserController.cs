using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Block11_RoleBasedAuthentication.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
            var currentUser = (string)Session["username"];
            var userRoles = MvcApplication.UserRoles;
            var currentUserRole = (string)userRoles[currentUser];
            if (currentUserRole != "User")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}