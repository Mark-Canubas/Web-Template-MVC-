using LoginMVC.Models;
using LoginMVC.Models.Dtos;
using LoginMVC.Services;
using LoginMVC.Services.Api;
using LoginMVC.Services.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LoginMVC.Controllers
{
    public class LoginController : Controller
    {
        private AuthService Auth => new AuthService(new SecApiClient(), new SessionService(Session));

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(UserModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await Auth.LoginAsync(new UserModel { Username = model.Username, Password = model.Password });

                var user = await Auth.GetUserProfileAsync();

                System.Diagnostics.Debug.WriteLine(user as UserDto);


                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceWarning("Login failed for '{0}'. Error: {1}", model.Username, ex);

                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}