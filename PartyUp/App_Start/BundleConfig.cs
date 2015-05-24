using System.Web;
using System.Web.Optimization;

namespace PartyUp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts/libs").Include(
                "~/client/bower_components/angular/angular.js",
                "~/client/bower_components/angular-animate/angular-animate.js",
                "~/client/bower_components/angular-aria/angular-aria.js",
                "~/client/bower_components/angular-material/angular-material.js",
                "~/client/bower_components/angular-ui-router/release/angular-ui-router.js"
                //"~/client/bower_components/jquery/dist/jquery.js",
                //"~/client/bower_components/bootstrap/dist/js/bootstrap.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/scripts/app").Include(
                // App
                "~/client/app/app.js",
                "~/client/app/app.config.js",
                // Controllers
                "~/client/app/header/header.ctrl.js",
                "~/client/app/home/home.ctrl.js"));
           
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/client/bower_components/angular-material/angular-material.css"
            ));
        }
    }
}
