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

        // Home page
        public ActionResult Index(string searchString, string categoryID)
        {
            User currentUser = this.User.GetCurrentUser();
            
            ViewData["randomProducts"] = db
                .Products
                .Include(p => p.Category)
                .Include(p => p.User)
                .Where(s => s.Status == "online")
                .Where(c => c.Cost <= currentUser.NbPoint)
                .OrderBy(r => Guid.NewGuid()).Take(3)
                .ToList();
                
            // Liste des categories
            var categories = db.Categories;
            int categoriesCount = categories.Count();
            Double categorySplit = 12 / categoriesCount;
            ViewData["categorySize"] = (int?) Math.Round(categorySplit);
            ViewData["categories"] = db.Categories.ToList();

            // Liste des produits en ligne et visibles par le membre
            var products = db
                .Products
                .Include(p => p.Category)
                .Include(p => p.User)
                .Where(s => s.Status == "online")
                .Where(c => c.Cost <= currentUser.NbPoint);

            // Recherche
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(n => n.Name.Contains(searchString));
            }
            
            if (!String.IsNullOrEmpty(categoryID))
            {
                int categoryIDSearch = int.Parse(categoryID);
                products = products.Where(c => c.CategoryID == categoryIDSearch);
            }

            ViewData["products"] = products.ToList();

            // Alerte
            if (Request.QueryString["update"] == "1")
            {
                ViewData["alert"] = "Vos modifications ont bien été enregistrées.";
            }

            return View();
        }
    }
}