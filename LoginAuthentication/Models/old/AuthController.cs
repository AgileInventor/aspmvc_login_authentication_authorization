using LoginAuthenticationAuthorization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginAuthentication.Controllers
{
    public class AuthController : Controller
    {
        private AuthModel db = new AuthModel();

        //AUTH
        //GET: Auth/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Login,Password")] User user_input)
        {
            try
            {
                user_input.Password = HelperFunctions.CreateMD5(user_input.Password);
                User user = db.User.Where(u => u.Login == user_input.Login && u.Password == user_input.Password && u.Status == true).First();
                if (user != null)
                {
                    Session["user"] = user;
                    return RedirectToAction("Index", "Admin");
                }
            }
            catch (Exception ex)
            {
                ViewBag.MessageError = "Usuario o Password erroneos";
            }
            return View();
        }

        // GET: Auth/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}