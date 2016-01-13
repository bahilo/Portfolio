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
                      "~/Content/Bootstrap/js/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Content/Scripts/Carousel.js",
                      "~/Content/Scripts/Site.js",
                      "~/Content/Scripts/waterbubble.js",
                      "~/Content/Scripts/scripts.js"
                      ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/Css/bootstrap.css",
                      "~/Content/Bootstrap/css/bootstrap.css",
                      "~/Content/Css/animate.css",
                      "~/Content/Css/styles.css"));
        }
    }
}
