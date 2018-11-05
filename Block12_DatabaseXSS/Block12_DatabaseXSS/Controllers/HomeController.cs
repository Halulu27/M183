using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Block12_DatabaseXSS.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index(bool single = true)
        {
            return View();
        }

        public ActionResult Login()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString =
                @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gibz\Documents\sql_xss_injection.mdf;Integrated Security=True;Connect Timeout=30";
            SqlCommand command = new SqlCommand();

            command.CommandText = "SELECT [Id], [Username], [Password] FROM [dbo].[User]";
            command.Connection = con;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader.GetInt32(0)}\t{reader.GetString(1)}");
                    }
                }
                else
                {
                    Console.WriteLine("No rows");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index()
        {
            var username = Request["Username"];
            var password = Request["Password"];


            var con = new SqlConnection();
            con.ConnectionString =
                @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gibz\Documents\sql_xss_injection.mdf;Integrated Security=True;Connect Timeout=30";
            var command = new SqlCommand();
            command.CommandText = $"SELECT [Id], [Username], [Password] FROM [dbo].[User] WHERE [Username] = '{username}' AND [Password] = '{password}';";
            command.Connection = con;
            con.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    ViewBag.Message = "Success";
                    while (reader.Read())
                    {
                        ViewBag.Message += reader.GetInt32(0) + " " + reader.GetString(1) + " " + reader.GetString(2);
                    }
                }
                else
                {
                    ViewBag.Message = "Nothing to read";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoFeedback()
        {
            var feedback = Request["Feedback"];


            var con = new SqlConnection();
            con.ConnectionString =
                @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gibz\Documents\sql_xss_injection.mdf;Integrated Security=True;Connect Timeout=30";
            var command = new SqlCommand();
            command.CommandText = $"INSERT INTO [dbo].[Feedback] SET [Feedback] = '{feedback}';";
            command.Connection = con;
            con.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    ViewBag.Message = "Success";
                    while (reader.Read())
                    {
                        ViewBag.Message += reader.GetInt32(0) + " " + reader.GetString(1);
                    }
                }
                else
                {
                    ViewBag.Message = "Nothing to read";
                }
            }
            return RedirectToAction("Feedback");
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
    }
}