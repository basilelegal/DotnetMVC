using SitePartage.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SitePartage.Controllers
{
    public class HomeController : Controller
    {
        {
        private SitePartageEntities db = new SitePartageEntities();

        public ActionResult Index()
        {
            ViewData["randomProducts"] = db.Products.OrderBy(r => Guid.NewGuid()).Take(3).ToList();
            var categories = db.Categories;
            int categoriesCount = categories.Count();
            Double categorySplit = 12 / categoriesCount;
            ViewData["categorySize"] = (int?) Math.Round(categorySplit);
            ViewData["categories"] = db.Categories.ToList();
            ViewData["products"] = db
                .Products
                .Include(p => p.Category)
                .Include(p => p.User)
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