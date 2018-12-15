using SitePartage.Models;
using System.Security.Claims;
using System.Security.Principal;

namespace SitePartage.Helpers
{
    public static class UserHelpers
    {
        private static SitePartageEntities db = new SitePartageEntities();

        // Retourne l'identifiant de utilisateur connecté
        public static string GetCurrentUserId(this IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            return claim.Value;
        }

        // Retourne l'utilisateur connecté
        public static User GetCurrentUser(this IPrincipal principal)
        {
            string userId = GetCurrentUserId(principal);
            User currentUser = db.Users.Find(int.Parse(userId));

            return currentUser;
        }
    }
}