using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
                var request = (HttpWebRequest) WebRequest.Create("https://rest.nexmo.com/sms/json");
                var secret = "Test_SECRET";
                var postData = "api_key=7e42b8cd";
                postData += "&api_secret=fbkJ6jBjYGS2IkkT";
                postData += "&to=41794517073";
                postData += "&from=\"NEXMO\"";
                postData += "&text=\"Hello from Nexmo. token is: " + secret + "\"";
                var data = Encoding.ASCII.GetBytes((postData));

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.KeepAlive = true;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse) request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                ViewBag.Message = responseString;
            }
            else
            {
                ViewBag.Message = "Wrong Credentials";
            }
            return View();
        }

        [HttpPost]
        public ActionResult TokenLogin()
        {
            var token = Request["token"];
            if (token == "Test_SCRET")
            {

            }
            else
            {
                
            }
            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}