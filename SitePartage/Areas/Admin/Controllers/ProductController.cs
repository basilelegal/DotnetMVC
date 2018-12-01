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
    public class ProductController : Controller
    {
        private SitePartageEntities db = new SitePartageEntities();

        // GET: Admin/Product
        public ActionResult Index(string searchString, string status)
        {
            ViewBag.Status = new SelectList(Product.statusLst);

            var products = db.Products.Include(p => p.Category).Include(p => p.User);
            
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(n => n.Name.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(status))
            {
                products = products.Where(s => s.Status == status);
            }

            return View(products.ToList());
        }

        // TODO à virer
        // GET: Admin/Product/ToValidate
        public ActionResult ToValidate()
        {
            //var products = db.Products.Include(p => p.Category).Include(p => p.User);

            var products = from p in db.Products
                           select p;
            products = products.Where(s => s.Status == "to_validate");

            return View(products.ToList());
        }

        // TODO à virer
        // Get: Admin/Product/Validate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Validate(int? id)
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

            //TODO
            product.Status = "validate";
            db.SaveChanges();

            return RedirectToAction("ToValidate"); ;
        }

        // GET: Admin/Product/Details/5
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
            return View(product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            ViewBag.UserID = new SelectList(db.Users, "UserID", "LastName");
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,CategoryID,UserID,Name,Description,Cost,Picture,Type,Weight,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "LastName", product.UserID);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
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

            // Gestion des select box
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "LastName", product.UserID);
            ViewBag.Status = new SelectList(Product.statusLst);

            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,UserID,Name,Description,Cost,Picture,Type,Weight,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", product.CategoryID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "LastName", product.UserID);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
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

        // POST: Admin/Product/Delete/5
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
