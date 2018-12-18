using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SitePartage.Models;
using SitePartage.Helpers;

namespace SitePartage.Controllers
{
    public class UserController : Controller
    {
        private SitePartageEntities db = new SitePartageEntities();

        // GET: User
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: User/Details/5
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

        // Création compte
        // GET: User/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            ViewBag.Civility = new SelectList(SitePartage.Models.User.civilityLst);

            return View();
        }

        // Création compte
        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,LastName,FirstName,Civility,NickName,Email,Password,Address,PostalCode,City")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login", "Authentication", new {register = 1});
            }

            return View(user);
        }

        // Mes informations
        // GET: User/Edit
        public ActionResult Edit()
        {
            User user = db.Users.Find(this.User.GetCurrentUserId());
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.nbPoint = user.NbPoint;

            return View(user);
        }

        // Mes informations
        // POST: User/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LastName,FirstName,Civility,NickName,Email,Password,Address,PostalCode,City,Role")] User user)
        {
            user.UserID = this.User.GetCurrentUserId();

            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.Entry(user).Property(x => x.IsValid).IsModified = false;
                db.Entry(user).Property(x => x.NbPoint).IsModified = false;
                db.Entry(user).Property(x => x.Role).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Edit", "User", new { update = 1 });
            }

            return View(user);
        }

        // Mes annonces
        // GET: User/Products
        public ActionResult Products()
        {
            User currentUser = this.User.GetCurrentUser();

            var products = db
                .Products
                .Include(p => p.Category)
                .Include(p => p.User)
                .Where(p => p.UserID == currentUser.UserID);

            //ViewData["products"] = products.ToList();

            return View(products.ToList());
        }

        // Mes locations
        // GET: User/Products
        public ActionResult Leasings()
        {
            User currentUser = this.User.GetCurrentUser();
            
            var leasings = db
                .Leasings
                .Include(l => l.Product)
                .Include(l => l.User)
                .Where(l => l.UserID == currentUser.UserID);

            //ViewData["leasings"] = leasings.ToList();

            return View(leasings.ToList());
        }

        // Offres reçues
        // GET: User/Products
        public ActionResult Offers()
        {
            User currentUser = this.User.GetCurrentUser();

            var leasings = db
                .Leasings
                .Include(l => l.Product)
                .Include(l => l.User)
                .Join(
                    db.Products.Where(x => x.UserID == currentUser.UserID),
                    l => l.ProductID,
                    p => p.ProductID,
                    (l, p) => l
                 );

            //ViewData["leasings"] = leasings.ToList();

            return View(leasings.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Vérifie l'unicité de l'email lors de la création ou de la modif
        [AllowAnonymous]
        public JsonResult EmailExists(string email)
        {
            User user;
            if (Request.IsAuthenticated)
            {
                int userId = this.User.GetCurrentUserId();
                user = db.Users.SingleOrDefault(e => e.Email == email && e.UserID != userId);
            } else
            {
                user = db.Users.SingleOrDefault(e => e.Email == email);
            }

            return Json(user == null);
        }
    }
}
