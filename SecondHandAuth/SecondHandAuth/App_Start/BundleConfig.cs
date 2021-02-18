using System.Web;
using System.Web.Optimization;

namespace SecondHandAuth
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            // my bundle
            // LoginScript
            bundles.Add(new ScriptBundle("~/Login/js").Include(
                      "~/Assets/vendor/jquery/jquery.min.js",
                      "~/Assets/vendor/bootstrap/js/bootstrap.bundle.min.js", 
                      "~/Assets/vendor/jquery-easing/jquery.easing.min.js",
                      "~/Assets/js/sb-admin-2.min.js",
                      "~/Assets/MyScript/CustomValidateBox.js"));
            // login css
            bundles.Add(new StyleBundle("~/Login/css").Include(
                      "~/Assets/vendor/fontawesome-free/css/all.min.css",
                      "~/Assets/css/sb-admin-2.min.css",
                      "~/Assets/MyCss/validate.css"));

            // admin layout css
            bundles.Add(new StyleBundle("~/Admin/Layout/css").Include(
                      "~/Assets/vendor/fontawesome-free/css/all.min.css",
                      "~/Assets/css/sb-admin-2.min.css",
                      "~/Content/fontawesome.min.css"
                      ));
            // admin layout js
            bundles.Add(new ScriptBundle("~/Admin/Layout/js").Include(
                      "~/Assets/vendor/jquery/jquery.min.js",
                      "~/Assets/vendor/bootstrap/js/bootstrap.bundle.min.js",
                      "~/Assets/vendor/jquery-easing/jquery.easing.min.js",
                      "~/Assets/js/sb-admin-2.min.js"
                      ));
            // Product css
            bundles.Add(new StyleBundle("~/Admin/Product/css").Include(
                      "~/Assets/vendor/fontawesome-free/css/all.min.css",
                      "~/Assets/css/sb-admin-2.min.css",
                      "~/Content/fontawesome.min.css"
                      ));
            // Product script
            bundles.Add(new ScriptBundle("~/Admin/Product/js").Include(
                      "~/Assets/vendor/jquery/jquery.min.js",
                      "~/Assets/vendor/bootstrap/js/bootstrap.bundle.min.js",
                      "~/Assets/vendor/jquery-easing/jquery.easing.min.js"
                      ));
        }

    }
}
