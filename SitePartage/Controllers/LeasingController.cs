using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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

        // POST: Leasing/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LeasingID,ProductID,UserID,Date,Duration,TotalCost,Status,HasComment,Message")] Leasing leasing)
        {
            if (ModelState.IsValid)
            {
                db.Leasings.Add(leasing);
                db.SaveChanges();
                return RedirectToAction("Index");
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
