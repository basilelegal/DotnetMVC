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
        [AllowAnonymous]
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

        // Mon compte
        // GET: User/Account
        public ActionResult Account()
        {
            // Mes informations
            int userId = this.User.GetCurrentUserId();
            User currentUser = db.Users.Find(userId);

            if (currentUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.nbPoint = currentUser.NbPoint;

            // Mes annonces
            var products = db
                .Products
                .Include(p => p.Category)
                .Include(p => p.User)
                .Where(p => p.UserID == currentUser.UserID);

            ViewData["products"] = products.ToList();

            // Mes locations
            var leasings = db
                .Leasings
                .Include(l => l.Product)
                .Include(l => l.User)
                .Where(l => l.UserID == currentUser.UserID);

            ViewData["leasings"] = leasings.ToList();

            // Offres reçues
            var offers = db
                .Leasings
                .Include(l => l.Product)
                .Include(u => u.User)
                .Join(
                    db.Products.Where(x => x.UserID == currentUser.UserID),
                    l => l.ProductID,
                    p => p.ProductID,
                    (l, p) => l
                );

            ViewData["offers"] = offers.ToList();

            var offersList = offers.ToList();

            // Alerte
            if (Request.QueryString["update"] == "1")
            {
                ViewData["alert"] = "Vos modifications ont bien été enregistrées.";
            }
            else if (Request.QueryString["leasing"] == "1")
            {
                ViewData["alert"] = "Votre demande de location a bien été enregistrée.";
            }
            else if (Request.QueryString["accept"] == "1")
            {
                ViewData["alert"] = "La location a bien été enregistrée.";
            }
            else if (Request.QueryString["create"] == "1")
            {
                ViewData["alert"] = "Votre annonce a bien été enregistrée.";
            }

            return View(currentUser);
        }

        // Mes informations
        // POST: User/Account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Account([Bind(Include = "LastName,FirstName,Civility,NickName,Email,Password,Address,PostalCode,City,Role")] User user)
        {
            user.UserID = this.User.GetCurrentUserId();

            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.Entry(user).Property(x => x.IsValid).IsModified = false;
                db.Entry(user).Property(x => x.NbPoint).IsModified = false;
                db.Entry(user).Property(x => x.Role).IsModified = false;
                int nb = db.SaveChanges();

                return RedirectToAction("Account", "User", new { update = 1 });
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

        // Vérifie l'unicité de l'email lors de la création ou de la modification
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
