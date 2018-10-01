using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Block6_SSO.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //"ClientID: "
            //"Clientschlüssel: dq8obHAkX2Bsua3X56e1-DeX"
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

        public JsonResult SSOTokenSignin()
        {
            var idToken = Request["idtoken"];
            var request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=" + idToken);
            var postData = "id_token=" + idToken;
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return Json(responseString);
        }
    }
}