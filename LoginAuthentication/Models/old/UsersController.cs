using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoginAuthenticationAuthorization.Models;

namespace LoginAuthentication.Controllers
{
    public class UsersController : Controller
    {
        private AuthModel db = new AuthModel();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.User.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            User user = new User();
            user.Created = DateTime.Now;
            user.Modify = DateTime.Now;
            return View(user);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserName,Login,Password,Created,Modify,Status")] User user)
        {
            
            user.Password = HelperFunctions.CreateMD5(user.Password);
            if (ModelState.IsValid)
            {                
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.Modify = DateTime.Now;
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserName,Login,Password,Created,Modify,Status")] User user, String OldPassword)
        {
            if (user.Password != OldPassword)
            {
                user.Password = HelperFunctions.CreateMD5(user.Password);
            }
            user.Modify = DateTime.Now;
            if (ModelState.IsValid)
            {               
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EditUser()
        {           
            User user = @Session["user"] as User;
            user.Modify = DateTime.Now;
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser([Bind(Include = "UserID,UserName,UserLogin,UserPassword,UserStatus,UserAdmin")] User user, String OldPassword)
        {
            if (ModelState.IsValid)
            {
                if (user.Password != OldPassword)
                {
                    user.Password = HelperFunctions.CreateMD5(user.Password);
                }
                user.Modify = DateTime.Now;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                @Session["user"] = user;
                return RedirectToAction("Index", "Admin");                
            }
            return View(user);
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
