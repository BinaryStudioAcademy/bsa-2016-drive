using System.Web.Optimization;

namespace Drive.WebHost
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/Libs/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/Libs/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/App/"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/libs").Include(
                    "~/Scripts/Bundles/libs.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                    "~/Scripts/Bundles/jquerylibs.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                       "~/Scripts/Bundles/main.js"));

            bundles.Add(new StyleBundle("~/bundles/styles").Include(
                       "~/Content/Bundles/styles.css"));
        }
    }
}