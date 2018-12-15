using SitePartage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SitePartage.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private SitePartageEntities db = new SitePartageEntities();

        // GET: Admin/Home
        public ActionResult Index()
        {
            var nbProductToValidate = db.Products.Where(s => s.Status == "to_validate").Count();
            var nbUserToValidate = db.Users.Where(i => i.IsValid == false).Count();

            ViewData.Add("nbProductToValidate", nbProductToValidate);
            ViewData.Add("nbUserToValidate", nbUserToValidate);

            return View();
        }
    }
}