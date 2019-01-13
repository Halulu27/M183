using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Block16_Logging.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var con = new SqlConnection
            //{
            //    ConnectionString =
            //        @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gibz\Documents\logging_instrusion_detection.mdf;Integrated Security=True;Connect Timeout=30"
            //};

            //SqlCommand cmd = new SqlCommand();
            //SqlDataReader reader;

            //cmd.CommandText = "SELECT [Id], [Username], [Password] FROM [dbo].[User];";
            //cmd.Connection = con;

            //reader = cmd.ExecuteReader();
            //if (reader.HasRows)
            //{
            //    while (reader.Read())
            //    {
            //        Console.WriteLine($"{reader.GetInt32(0)}\t{reader.GetString(1)}");
            //    }
            //}
            //{
            //    Console.WriteLine("No rows found.");
            //}


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
        public ActionResult DoLogin()
        {
            var username = Request["username"];
            var password = Request["password"];

            var ip = Request.ServerVariables["REMOTE_ADDR"];
            var platform = Request.Browser.Platform;
            var browser = Request.UserAgent;

            var con = new SqlConnection
            {
                ConnectionString =
                    @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gibz\Documents\logging_instrusion_detection.mdf;Integrated Security=True;Connect Timeout=30"
            };

            var cmd = new SqlCommand
            {
                CommandText = $"SELECT [Id] FROM [dbo].[User] WHERE [Username] = '{username}' AND [Password] = '{password}';",
                Connection = con
            };
            con.Open();
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    var userId = 0;
                    while (reader.Read())
                    {
                        userId = reader.GetInt32(0);
                        break;
                    }
                    con.Close();
                    con.Open();
                    var sqlCommand = new SqlCommand
                    {
                        CommandText = $"SELECT [Id] FROM [dbo].[UserLog] WHERE [UserId] LIKE '{ip.Substring(0, 2)}%' AND [Browser] LIKE '{platform}%';",
                        Connection = con
                    };
                    using (var sqlReader = sqlCommand.ExecuteReader())
                    {
                        if (!sqlReader.HasRows)
                        {
                            con.Close();
                            con.Open();
                            var sqlInsertCommand = new SqlCommand
                            {
                                CommandText = $"INSERT INTO [dbo].[UserLog] (UserId, IP, Action, Result, CreatedOn, Browser, AdditionalInformation) VALUES('{userId}', '{ip}', 'login', 'success', GETDATE(), '{platform}', 'other browser')",
                                Connection = con
                            };
                            sqlInsertCommand.ExecuteReader();
                        }
                        else
                        {
                            con.Close();
                            con.Open();
                            var sqlInsertCommand = new SqlCommand
                            {
                                CommandText = $"INSERT INTO [dbo].[UserLog] (UserId, IP, Action, Result, CreatedOn, Browser) VALUES('{userId}', '{ip}', 'login', 'success', GETDATE(), '{platform}')",
                                Connection = con
                            };
                            sqlInsertCommand.ExecuteReader();
                        }
                    }
                }
                else
                {
                    con.Close();
                    con.Open();
                    var sqlCommand = new SqlCommand
                    {
                        CommandText = $"SELECT [Id] FROM [dbo].[User] WHEE [Username] = '{username}';",
                        Connection = con
                    };
                    using (var sqlReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlReader.HasRows)
                        {
                            var userId = 0;
                            while (sqlReader.Read())
                            {
                                userId = sqlReader.GetInt32(0);
                                break;
                            }
                            con.Close();
                            con.Open();

                            var sqlCountCommand = new SqlCommand
                            {
                                CommandText = $"SELECT COUNT(Id) FROM [dbo].[UserLog] WHERE [UserId] = '{userId}'AND [Result] = 'failed' AND CAST(CreatedOn AS date) = '{System.DateTime.Now.ToShortDateString().Substring(0, 10)}'",
                                Connection = con
                            };

                        }
                        else
                        {
                            
                        }
                    }
                }
            }
            con.Close();

            return RedirectToAction("Logs", "Home");
        }
    }
}