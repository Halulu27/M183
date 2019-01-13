using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Block16_Logging.Models;

namespace Block16_Logging.Controllers
{
    public class HomeController : Controller
    {
        private static string dbConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gibz\Documents\logging_instrusion_detection.mdf;Integrated Security=True;Connect Timeout=30";

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
        public ActionResult DoLogin()
        {
            var username = Request["username"];
            var password = Request["password"];

            var ip = Request.ServerVariables["REMOTE_ADDR"];
            var platform = Request.Browser.Platform;
            var additionalInfo = Request.UserAgent;

            var con = new SqlConnection
            {
                ConnectionString = dbConnectionString
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
                                CommandText = $"INSERT INTO [dbo].[UserLog] (UserId, IP, Action, Result, CreatedOn, Browser, AdditionalInformation) VALUES('{userId}', '{ip}', 'login', 'success', GETDATE(), '{platform}', '{additionalInfo}')",
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
                        CommandText = $"SELECT [Id] FROM [dbo].[User] WHERE [Username] = '{username}';",
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
                                CommandText = $"SELECT COUNT(Id) FROM [dbo].[UserLog] WHERE [UserId] = '{userId}' AND [Result] = 'failed' AND CAST(CreatedOn AS date) = '{DateTime.Now.ToShortDateString().Substring(0, 10)}'",
                                Connection = con
                            };
                            using (var failedLoginCount = sqlCountCommand.ExecuteReader())
                            {
                                var attempts = 0;
                                if (failedLoginCount.HasRows)
                                {
                                    while (failedLoginCount.Read())
                                    {
                                        attempts = failedLoginCount.GetInt32(0);
                                        break;
                                    }
                                }
                                if (attempts >= 5 || password.Length < 4 || password.Length > 20)
                                {
                                    // block user
                                }
                            }

                            con.Close();
                            con.Open();

                            var sqlLogCmd = new SqlCommand
                            {
                                CommandText = $"INSERT INTO [dbo].[UserLog] (UserId, IP, Action, Result, CreatedOn, Browser) VALUES('{userId}', '{ip}', 'login', 'failed', GETDATE(), '{platform}')",
                                Connection = con
                            };
                            sqlLogCmd.ExecuteReader();

                            ViewBag.Message = "No user found";

                        }
                        else
                        {
                            con.Close();
                            con.Open();

                            var sqlLogCmd = new SqlCommand
                            {
                                CommandText = $"INSERT INTO [dbo].[UserLog] (UserId, IP, Action, Result, CreatedOn, AdditionalInformation, Browser) VALUES(0, '{ip}', 'login', 'failed', GETDATE(), 'No User Found', '{platform}')",
                                Connection = con
                            };
                            sqlLogCmd.ExecuteReader();
                            ViewBag.Message = "No user found";
                        }
                    }
                }
            }
            con.Close();

            return RedirectToAction("Logs", "Home");
        }

        public ActionResult Logs()
        {
            var con = new SqlConnection
            {
                ConnectionString = dbConnectionString
            };

            var sqlCredentialsCmd = new SqlCommand
            {
                CommandText = "SELECT * FROM [dbo].[UerLog] ul JOIN [dbo].[User] u ON ul.UserId = u.Id ORDER BY ul.CreatedOn DESC;",
                Connection = con
            };
            con.Open();
            using (var sqlReader = sqlCredentialsCmd.ExecuteReader())
            {
                var viewModel = new List<HomeControllerViewModel>();
                if (sqlReader.HasRows)
                {
                    while (sqlReader.Read())
                    {
                        viewModel.Add(new HomeControllerViewModel
                        {
                            UserId = sqlReader.GetValue(10).ToString(),
                            LogId = sqlReader.GetValue(0).ToString(),
                            LogCreatedOn = sqlReader.GetValue(7).ToString()
                        });
                    }
                }
                return View(viewModel);
            }
        }
    }
}