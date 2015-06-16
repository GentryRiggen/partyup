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
                "~/client/bower_components/angular-ui-router/release/angular-ui-router.js",
                "~/client/bower_components/jquery/dist/jquery.js",
                "~/client/bower_components/underscore/underscore.js",
                "~/Scripts/jquery.signalR-2.2.0.js",
                "~/client/bower_components/toastr/toastr.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/scripts/app").Include(
                // App
                "~/client/app/app.js",
                "~/client/app/app.config.js",

                // COMMON
                "~/client/app/common/authToken.service.js",
                "~/client/app/common/authInterceptor.factory.js",
                "~/client/app/common/user.service.js",
                "~/client/app/common/alert.service.js",
                "~/client/app/common/signalR.service.js",
                "~/client/app/common/files.service.js",

                // LAYOUT
                "~/client/app/header/header.ctrl.js",

                // LOGIN/LOGOUT
                "~/client/app/login/login.ctrl.js",
                "~/client/app/login/logout.ctrl.js",

                // COMMUNITIES
                "~/client/app/community/communities.ctrl.js",
                "~/client/app/community/community.ctrl.js",
                "~/client/app/community/communities.service.js",
                "~/client/app/community/communityCard.directive.js",

                // MISSIONS
                "~/client/app/mission/mission.ctrl.js",
                "~/client/app/mission/missions.service.js",
                "~/client/app/mission/missionCard.directive.js",

                // ADMIN
                "~/client/app/admin/admin.ctrl.js",
                    
                    // ADMIN - COMMUNITIES
                    "~/client/app/community/communities.admin.ctrl.js",
                    "~/client/app/community/community.admin.ctrl.js",

                    // ADMIN - MISSIONS
                    "~/client/app/mission/mission.admin.ctrl.js"
            ));
           
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/client/bower_components/bootstrap/dist/css/bootstrap.css",
                "~/client/bower_components/toastr/toastr.css",
                "~/client/bower_components/angular-material/angular-material.css",
                "~/client/bower_components/animate.css/animate.css",
                "~/client/app/styles/base.css"
            ));
        }
    }
}
