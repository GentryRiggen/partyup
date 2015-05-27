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
                "~/app/bower_components/angular/angular.js",
                "~/app/bower_components/angular-animate/angular-animate.js",
                "~/app/bower_components/angular-aria/angular-aria.js",
                "~/app/bower_components/angular-material/angular-material.js",
                "~/app/bower_components/angular-ui-router/release/angular-ui-router.js",
                "~/app/bower_components/jquery/dist/jquery.js",
                "~/app/bower_components/bootstrap/dist/js/bootstrap.js",
                "~/Scripts/jquery.signalR-2.2.0.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/scripts/app").Include(
                // App
                "~/app/app.js",
                "~/app/app.config.js",
                // Services
                "~/app/common/authInterceptor.js",
                "~/app/common/authToken.js",
                "~/app/common/alert.js",
                // Controllers
                "~/app/header/header.ctrl.js",
                "~/app/login/login.ctrl.js",
                "~/app/home/home.ctrl.js"));
           
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/app/bower_components/bootstrap/dist/css/bootstrap.css",
                "~/app/bower_components/angular-material/angular-material.css",
                "~/app/bower_components/animate.css/animate.css",
                "~/app/assets/styles/base.css"
            ));
        }
    }
}
