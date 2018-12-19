using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using SitePartage.Models;
using SitePartage.Helpers;

namespace SitePartage.Controllers
{
    public class ProductController : Controller
    {
        private SitePartageEntities db = new SitePartageEntities();

        // GET: Product
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.User);
            return View(products.ToList());
        }

        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = product.ProductID;
            return View(product);
        }

        // Publier une annonce
        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "LastName");
            ViewBag.Type = new SelectList(Product.typeLst);

            return View();
        }

        // Publier une annonce
        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,CategoryID,Name,Description,Cost,Picture,Type,Weight,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.UserID = this.User.GetCurrentUserId();
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Account", "User", new { create = 1 });
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "LastName", product.UserID);
            return View(product);
        }

        // Modifier une annonce
        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            int UserID = this.User.GetCurrentUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // On controle que le produit appartient à l'utilisateur connecté
            Product product = db.Products.Find(id);
            if (product == null || product.UserID != UserID)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "LastName", product.UserID);
            ViewBag.Type = new SelectList(Product.typeLst);

            return View(product);
        }

        // Modifier une annonce
        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,Name,Description,Cost,Picture,Type,Weight")] Product product)
        {
            int UserID = this.User.GetCurrentUserId();

            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.Entry(product).Property(x => x.UserID).IsModified = false;
                db.Entry(product).Property(x => x.Status).IsModified = false;
                db.SaveChanges();

                return RedirectToAction("Account", "User", new { update = 1 });
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "LastName", product.UserID);

            return View(product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
