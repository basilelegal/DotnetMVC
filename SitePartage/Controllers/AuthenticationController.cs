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
            string currentUser = ValidateUser(model.Login, model.Password);
            if (currentUser == null)
            {
                ModelState.AddModelError(string.Empty, "Le nom d'utilisateur ou le mot de passe est incorrect.");
                return View(model);
            }

            // L'authentification est réussie, 
            // injecter l'identifiant utilisateur dans le cookie d'authentification :
            var loginClaim = new Claim(ClaimTypes.NameIdentifier, model.Login);
            var claimsIdentity = new ClaimsIdentity(new[] { loginClaim }, DefaultAuthenticationTypes.ApplicationCookie);
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignIn(claimsIdentity);

            // Rediriger vers l'URL d'origine :
            if (Url.IsLocalUrl(ViewBag.ReturnUrl))
                return Redirect(ViewBag.ReturnUrl);
            // Par défaut, rediriger vers la page d'accueil :
            return RedirectToAction("Index", "Home");
        }

        private string ValidateUser(string login, string password)
        {
            //User currentUser = db.Users.SingleOrDefault(user => user.Email == login);


            // TODO : insérer ici la validation des identifiant et mot de passe de l'utilisateur...

            // Pour ce tutoriel, j'utilise une validation extrêmement sécurisée...
            /*if ( currentUser.Password == password)
            {
                return currentUser;
            }*/
            return "test";
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