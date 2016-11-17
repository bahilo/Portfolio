using System.Web;
using System.Web.Optimization;

namespace DagoWebPorfolio
{
    public class BundleConfig 
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js", 
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Content/Scripts/Carousel.js",
                      "~/Content/Scripts/Site.js",
                      "~/Content/Scripts/scripts.js",
                      "~/Content/Scripts/waterbubble.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/courousel").Include(
                      "~/Content/Scripts/Carousel.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                      "~/Content/Themes/AdminTheme/js/jquery.js",
                      "~/Content/Themes/AdminTheme/js/bootstrap.js",
                      "~/Content/Themes/AdminTheme/js/jquery.scrollTo.min.js",
                      "~/Content/Themes/AdminTheme/js/jquery.nicescroll.js",
                      "~/Content/Themes/AdminTheme/js/gritter.js",
                      "~/Content/Themes/AdminTheme/js/scripts.js",
                      "~/Content/Themes/AdminTheme/assets/chart-master/Chart.js",
                      "~/Content/Themes/AdminTheme/js/chartjs-custom.js",
                      "~/Content/Themes/Modules/tinymce/js/tinymce/tinymce.min.js"
                      ));

            bundles.Add(new StyleBundle("~/bundles/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Css/animate.css",
                      "~/Content/Css/styles.css"));

            bundles.Add(new StyleBundle("~/bundles/admin/css").Include(
                      "~/Content/Themes/bootstrap.css",
                      "~/Content/Themes/AdminTheme/css/bootstrap-theme.css",
                      "~/Content/Themes/AdminTheme/css/elegant-icons-style.css",
                      "~/Content/Themes/AdminTheme/css/font-awesome.css",
                      "~/Content/Themes/AdminTheme/css/style.css",
                      "~/Content/Themes/AdminTheme/css/style-responsive.css"));
        }
    }
}
