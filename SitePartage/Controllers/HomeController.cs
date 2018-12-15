using SitePartage.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SitePartage.Helpers;

namespace SitePartage.Controllers
{
    public class HomeController : Controller
    {
        private SitePartageEntities db = new SitePartageEntities();

        public ActionResult Index()
        {
            User currentUser = this.User.GetCurrentUser();

            ViewData["randomProducts"] = db.Products.OrderBy(r => Guid.NewGuid()).Take(3).ToList();

            // Liste des categories
            var categories = db.Categories;
            int categoriesCount = categories.Count();
            Double categorySplit = 12 / categoriesCount;
            ViewData["categorySize"] = (int?) Math.Round(categorySplit);
            ViewData["categories"] = db.Categories.ToList();

            Response.Write("NbPoint : " + currentUser.NbPoint);

            // Liste des produits en ligne et visibles par le membre
            ViewData["products"] = db
                .Products
                .Include(p => p.Category)
                .Include(p => p.User)
                .Where(s => s.Status == "online")
                .Where(c => c.Cost <= currentUser.NbPoint)
                .ToList();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}