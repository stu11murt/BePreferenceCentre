using System.Web;
using System.Web.Optimization;

namespace BePreferenceCentre
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/jquery-3.3.1.js",
                         "~/Scripts/jquery.unobtrusive-ajax.js",
                         "~/Scripts/summernote/summernote-bs4.js",
                         "~/Scripts/DataTables/jquery.dataTables.js",
                         "~/Scripts/DataTables/dataTables.bootstrap.js",
                         "~/Scripts/DataTables/dataTables.fixedHeader.js",
                         "~/Scripts/DataTables/dataTables.responsive.js",
                         "~/Scripts/DataTables/responsive.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                     "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/animate.css"));

            bundles.Add(new StyleBundle("~/Content/thirds").Include(
                        "~/Script/summernote/summernote-bs4.css",
                      "~/Content/DataTables/css/jquery.dataTables.css",
                      "~/Content/DataTables/css/dataTables.fixedHeader.css",
                      "~/Content/DataTables/css/responsive.bootstrap.css"));


            bundles.Add(new ScriptBundle("~/bundles/inkey").Include(
                            "~/Scripts/jquery-3.3.1.js",
                            "~/Scripts/bootstrap.min.js",
                            "~/Scripts/popper.js",
                            "~/Scripts/summernote/summernote.min.js",
                            "~/Scripts/summernote/summernote-bs4.js"));
        }
    }
}
