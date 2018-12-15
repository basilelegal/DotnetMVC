using System.Web.Mvc;
using System.Security.Claims;
using SitePartage.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Host.SystemWeb;
using System.Web;
using System.Linq;

namespace SitePartage.Controllers
{
    public class AuthenticationController : Controller
    {
        private SitePartageEntities db = new SitePartageEntities();

        // Page de connexion
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validation des identifiant et mot de passe de l'utilisateur
            User currentUser = ValidateUser(model.Login, model.Password);
            if (currentUser == null)
            {
                ModelState.AddModelError("Login", "Le nom d'utilisateur ou le mot de passe est incorrect.");
                return View(model);
            }

            // L'authentification est réussie : on injecte l'identifiant utilisateur dans le cookie d'authentification
            var loginClaim = new Claim(ClaimTypes.NameIdentifier, currentUser.UserID.ToString());
            var claimsIdentity = new ClaimsIdentity(new[] { loginClaim }, DefaultAuthenticationTypes.ApplicationCookie);
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignIn(claimsIdentity);

            // Rediriger vers l'URL d'origine
            if (Url.IsLocalUrl(ViewBag.ReturnUrl))
            {
                //return Redirect(ViewBag.ReturnUrl);
            }

            // Par défaut, rediriger vers la page d'accueil
            return RedirectToAction("Index", "Home", new { idUser = currentUser.UserID.ToString() } );
        }

        // Validation des identifiant et mot de passe de l'utilisateur
        private User ValidateUser(string login, string password)
        {
            User currentUser = db.Users.SingleOrDefault(user => user.Email == login && user.IsValid == true);
            if (currentUser == null)
            {
                return null;
            }

            // TODO chiffrer le Password ?
            if (currentUser.Password != password)
            {
                return null;
            }

            return currentUser;
        }

        [HttpGet]
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();

            // Rediriger vers la page d'accueil :
            return RedirectToAction("Index", "Home");
        }


    }
}