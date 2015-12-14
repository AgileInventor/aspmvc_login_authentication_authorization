using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LoginAuthenticationAuthorization.Models;

namespace LoginAuthenticationAuthorization.Areas.Auth.Controllers
{
    public class UsersController : Controller
    {
        private AuthModel db = new AuthModel();

        // GET: Auth/Users
        public ActionResult Index()
        {
            var user = db.User.Include(u => u.Role);
            return View(user.ToList());
        }

        // GET: Auth/Users/Details/5
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

        // GET: Auth/Users/Create
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "RoleName");
            User user = new User();
            user.Created = DateTime.Now;
            user.Modify = DateTime.Now;
            return View(user);
        }

        // POST: Auth/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserName,Login,Password,Status,RoleID,Created,Modify")] User user)
        {
            user.Password = HelperFunctions.CreateMD5(user.Password);
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        // GET: Auth/Users/Edit/5
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
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        // POST: Auth/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserName,Login,Password,Status,RoleID,Created,Modify")] User user, String OldPassword)
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
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "RoleName", user.RoleID);
            return View(user);
        }

        // GET: Auth/Users/Delete/5
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

        // POST: Auth/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
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
