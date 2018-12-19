using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SitePartage.Helpers;
using SitePartage.Models;

namespace SitePartage.Controllers
{
    public class LeasingController : Controller
    {
        private SitePartageEntities db = new SitePartageEntities();

        // GET: Leasing
        public ActionResult Index()
        {
            var leasings = db.Leasings.Include(l => l.Product).Include(l => l.User);
            return View(leasings.ToList());
        }

        // GET: Leasing/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leasing leasing = db.Leasings.Find(id);
            if (leasing == null)
            {
                return HttpNotFound();
            }
            return View(leasing);
        }

        // GET: Leasing/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Name");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "LastName");
            return View();
        }

        // Création d'une location
        // POST: Leasing/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LeasingID,ProductID,Duration,Message")] Leasing leasing)
        {
            if (ModelState.IsValid)
            {
                leasing.UserID = this.User.GetCurrentUserId();
                Product prod = db.Products.Find(leasing.ProductID);
                leasing.TotalCost = prod.Cost;

                db.Leasings.Add(leasing);
                db.SaveChanges();
                return RedirectToAction("Account", "User", new { leasing = 1 });
            }

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Name", leasing.ProductID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "LastName", leasing.UserID);
            return View(leasing);
        }

        // GET: Leasing/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leasing leasing = db.Leasings.Find(id);
            if (leasing == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Name", leasing.ProductID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "LastName", leasing.UserID);
            return View(leasing);
        }

        // POST: Leasing/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LeasingID,ProductID,UserID,Date,Duration,TotalCost,Status,HasComment,Message")] Leasing leasing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leasing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Name", leasing.ProductID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "LastName", leasing.UserID);
            return View(leasing);
        }

        // Acceptation de la location
        // POST: Leasing/Accept/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Accept([Bind(Include = "LeasingID")] Leasing leasing)
        {
            Leasing leasingModified = db.Leasings
                .Include(l => l.Product)
                .Include(l => l.User)
                .Where(l => l.LeasingID == leasing.LeasingID)
                .First();

            // Controle utilisateur connecté
            int userId = this.User.GetCurrentUserId();
            if (leasingModified.Product.UserID == userId)
            {
                // MAJ de la location
                leasingModified.Status = "in_progress";
                db.Entry(leasingModified).State = EntityState.Modified;
                db.SaveChanges();

                // MAJ du produit
                Product product = db.Products.Find(leasingModified.ProductID);
                product.Status = "leasing";
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();

                // MAJ du nb de points du membre qui fait la location
                User leasingUser = db.Users.Find(leasingModified.UserID);
                leasingUser.NbPoint = leasingUser.NbPoint - leasingModified.TotalCost;
                db.Entry(leasingUser).State = EntityState.Modified;
                db.SaveChanges();

                // MAJ du nb de points du membre qui passe l'annonce
                User currentUser = db.Users.Find(userId);
                currentUser.NbPoint = currentUser.NbPoint + leasingModified.TotalCost;
                db.Entry(currentUser).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Account", "User", new { accept = 1 } );
            }

            return RedirectToAction("Account", "User");
        }

        // Refus de la location
        // POST: Leasing/Refus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Refuse([Bind(Include = "LeasingID")] Leasing leasing)
        {
            Leasing leasingModified = db.Leasings
                .Include(l => l.Product)
                .Include(l => l.User)
                .Where(l => l.LeasingID == leasing.LeasingID)
                .First();

            // Controle utilisateur connecté
            int userId = this.User.GetCurrentUserId();
            if (leasingModified.Product.UserID == userId)
            {
                // MAJ de la location
                leasingModified.Status = "refused";
                db.Entry(leasingModified).State = EntityState.Modified;
                db.SaveChanges();
           
                return RedirectToAction("Account", "User", new { accept = 1 });
            }

            return RedirectToAction("Account", "User");
        }

        // GET: Leasing/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leasing leasing = db.Leasings.Find(id);
            if (leasing == null)
            {
                return HttpNotFound();
            }
            return View(leasing);
        }

        // POST: Leasing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Leasing leasing = db.Leasings.Find(id);
            db.Leasings.Remove(leasing);
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
