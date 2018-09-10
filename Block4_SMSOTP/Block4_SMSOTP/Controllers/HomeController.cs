using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Block4_SMSOTP.Controllers
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

        [HttpPost]
        public ActionResult Login()
        {
            var username = Request["username"];
            var password = Request["password"];

            if (username == "test" && password == "test")
            {
                var request = (HttpWeRequest) WebRequest.Creat("https://rest.nexmo.com/sms/json");
                var secret = "fbkJ6jBjYGS2IkkT";
                var postData = "api_key = 7e42b8cd";
                postData += "&"

                curl - X POST https://rest.nexmo.com/sms/json \
                -d api_key = 7e42b8cd \
                -d api_secret = fbkJ6jBjYGS2IkkT \
                -d to = 41794517073 \
                -d from = "NEXMO" \
                -d text = "Hello from Nexmo"
            }
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}