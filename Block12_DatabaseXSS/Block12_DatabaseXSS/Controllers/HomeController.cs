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
        public static string dbConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gibz\Documents\sql_xss_injection.mdf;Integrated Security=True;Connect Timeout=30";

        [HttpGet]
        public ActionResult Index(bool single = true)
        {
            return View();
        }

        public ActionResult Login()
        {
            var con = new SqlConnection
            {
                ConnectionString = dbConnectionString
            };
            var command = new SqlCommand
            {
                CommandText = "SELECT [Id], [Username], [Password] FROM [dbo].[User]",
                Connection = con
            };

            using (var reader = command.ExecuteReader())
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


            var con = new SqlConnection
            {
                ConnectionString = dbConnectionString
            };
            var command = new SqlCommand
            {
                CommandText =
                    $"SELECT [Id], [Username], [Password] FROM [dbo].[User] WHERE [Username] = '{username}' AND [Password] = '{password}';",
                Connection = con
            };
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


            var con = new SqlConnection
            {
                ConnectionString = dbConnectionString

            };
            var command = new SqlCommand
            {
                CommandText = $"INSERT INTO [dbo].[Feedback] SET [Feedback] = '{feedback}';",
                Connection = con
            };
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