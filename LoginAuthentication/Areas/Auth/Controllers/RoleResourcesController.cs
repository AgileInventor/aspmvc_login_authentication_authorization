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
    public class RoleResourcesController : Controller
    {
        private AuthModel db = new AuthModel();

        // GET: Auth/RoleResources
        public ActionResult Index()
        {
            var roleResource = db.RoleResource.Include(r => r.Resource).Include(r => r.Role);
            return View(roleResource.ToList());
        }

        // GET: Auth/RoleResources/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleResource roleResource = db.RoleResource.Find(id);
            if (roleResource == null)
            {
                return HttpNotFound();
            }
            return View(roleResource);
        }

        // GET: Auth/RoleResources/Create
        public ActionResult Create()
        {
            ViewBag.ResourceID = new SelectList(db.Resource, "ResourceID", "ResourceName");
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "RoleName");
            return View();
        }

        // POST: Auth/RoleResources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoleResourceID,RoleID,ResourceID")] RoleResource roleResource)
        {
            if (ModelState.IsValid)
            {
                db.RoleResource.Add(roleResource);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ResourceID = new SelectList(db.Resource, "ResourceID", "ResourceName", roleResource.ResourceID);
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "RoleName", roleResource.RoleID);
            return View(roleResource);
        }

        // GET: Auth/RoleResources/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleResource roleResource = db.RoleResource.Find(id);
            if (roleResource == null)
            {
                return HttpNotFound();
            }
            ViewBag.ResourceID = new SelectList(db.Resource, "ResourceID", "ResourceName", roleResource.ResourceID);
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "RoleName", roleResource.RoleID);
            return View(roleResource);
        }

        // POST: Auth/RoleResources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoleResourceID,RoleID,ResourceID")] RoleResource roleResource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roleResource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ResourceID = new SelectList(db.Resource, "ResourceID", "ResourceName", roleResource.ResourceID);
            ViewBag.RoleID = new SelectList(db.Role, "RoleID", "RoleName", roleResource.RoleID);
            return View(roleResource);
        }

        // GET: Auth/RoleResources/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleResource roleResource = db.RoleResource.Find(id);
            if (roleResource == null)
            {
                return HttpNotFound();
            }
            return View(roleResource);
        }

        // POST: Auth/RoleResources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoleResource roleResource = db.RoleResource.Find(id);
            db.RoleResource.Remove(roleResource);
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
