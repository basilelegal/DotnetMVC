using SitePartage.Models;
using System.Security.Claims;
using System.Security.Principal;

namespace SitePartage.Helpers
{
    public static class UserHelpers
    {
        private static SitePartageEntities db = new SitePartageEntities();

        // Retourne l'identifiant de utilisateur connecté
        public static int GetCurrentUserId(this IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity)principal.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(claim.Value);
        }

        // Retourne l'utilisateur connecté
        public static User GetCurrentUser(this IPrincipal principal)
        {
            int userId = GetCurrentUserId(principal);
            User currentUser = db.Users.Find(userId);

            return currentUser;
        }
    }
}