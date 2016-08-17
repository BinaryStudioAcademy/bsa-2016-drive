using System.Web.Optimization;

namespace Drive.WebHost
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/App/"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css")
                .Include("~/Content/site.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/bundles/libs").Include(
                "~/Scripts/Bundles/libs.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                "~/Scripts/Bundles/main.js"));

            bundles.Add(new StyleBundle("~/bundles/styles").Include(
                "~/Content/Bundles/styles.css"));
        }
    }
}