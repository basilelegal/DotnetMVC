using System.Web;
using System.Web.Optimization;

namespace SitePartage
{
    public class BundleConfig
    {
        // Pour plus d'informations sur le regroupement, visitez https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilisez la version de développement de Modernizr pour le développement et l'apprentissage. Puis, une fois
            // prêt pour la production, utilisez l'outil de génération à l'adresse https://modernizr.com pour sélectionner uniquement les tests dont vous avez besoin.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/custom_css").Include(
                        "~/Content/custom/bootstrap.min.css",
                        "~/Content/custom/bootstrap-select.css",
                        "~/Content/custom/style.css",
                        "~/Content/custom/jquery-ui1.css",
                        "~/Content/custom/font-awesome.min.css",
                        "~/Content/custom/menu_sideslide.css",
                        "~/Content/custom/jquery.uls.css",
                        "~/Content/custom/jquery.uls.grid.css",
                        "~/Content/custom/jquery.uls.lcd.css"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/custom_js").Include(
                      "~/Scripts/custom/jquery.min.js",
                        "~/Scripts/custom/bootstrap.js",
                        "~/Scripts/custom/bootstrap-select.js",
                        "~/Scripts/custom/jquery.leanModal.min.js",
                        "~/Scripts/custom/jquery.uls.data.js",
                        "~/Scripts/custom/jquery.uls.data.utils.js",
                        "~/Scripts/custom/jquery.uls.lcd.js",
                        "~/Scripts/custom/jquery.uls.languagefilter.js",
                        "~/Scripts/custom/jquery.uls.regionfilter.js",
                        "~/Scripts/custom/jquery.uls.core.js",
                        "~/Scripts/custom/tabs.js",
                        "~/Scripts/custom/classie.js",
                        "~/Scripts/custom/main.js",
                        "~/Scripts/custom/move-top.js",
                        "~/Scripts/custom/easing.js",
                        "~/Scripts/custom/jquery-ui.js"));
        }
    }
}
