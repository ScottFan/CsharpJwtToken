using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JwtToken.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View("/Views/Home/frmLogin.cshtml");
        }
        public ActionResult Token()
        {
            string token = Request.Headers["Authorization"];
            if (token != null)
            {
                Response.Headers.Add("Authorization", token);
            }
            return View("/Views/Home/frmTokenInfo.cshtml");
        }
    }
}
