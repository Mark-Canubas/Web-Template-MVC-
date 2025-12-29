using LoginMVC.Models;
using LoginMVC.Models.Dtos;
using LoginMVC.Services;
using LoginMVC.Services.Api;
using LoginMVC.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LoginMVC.Controllers
{
    public class HomeController : Controller
    {
        private AuthService Auth => new AuthService(new SecApiClient(), new SessionService(Session));

        public ActionResult Index()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetNoStore();
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.AppendHeader("Pragma", "no-cache");

            if (!Auth.IsAuthenticated)
            {
                if (Session != null)
                {
                    Session.Clear();
                    Session.Abandon();
                }
                return RedirectToAction("Index", "Login");
            }

            return View();
        }
    }
}