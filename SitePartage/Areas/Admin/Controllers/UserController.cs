using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SitePartage.Models;

namespace SitePartage.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private SitePartageEntities db = new SitePartageEntities();

        // GET: Admin/User
        public ActionResult Index(string searchString, string isValid)
        {
            List<string> isValidLst = new List<string>() { "Oui", "Non" };
            ViewBag.IsValid = new SelectList(isValidLst);

            var users = from u in db.Users
                        select u;

            // Recherche
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(l => l.LastName.Contains(searchString) || l.FirstName.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(isValid))
            {
                if (isValid == "Oui") {
                    users = users.Where(i => i.IsValid == true);
                } else
                {
                    users = users.Where(i => i.IsValid == false);
                }
            }
            
            return View(users.ToList());
        }

        // GET: Admin/User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,LastName,FirstName,Civility,NickName,Email,Password,NbPoint,Address,PostalCode,City,Role,IsValid")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Admin/User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            List<string> roleLst = new List<string>() { "Membre", "Admin" };
            ViewBag.Role = new SelectList(roleLst);

            return View(user);
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,LastName,FirstName,Civility,NickName,Email,Password,NbPoint,Address,PostalCode,City,Role,IsValid")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(user);
        }

        // GET: Admin/User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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

        // Vérifie l'unicité de l'email 
        public JsonResult EmailExists(string email)
        {
            //User user = db.Users.SingleOrDefault(e => e.Email == email);

            return Json(true);
        }
    }
}
