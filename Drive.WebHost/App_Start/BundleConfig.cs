using System.Web.Optimization;

namespace Drive.WebHost
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/bundles/app").Include(
                "~/Scripts/App/"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/Scripts/bundles/libs").Include(
                "~/Scripts/Bundles/libs.js"));

            bundles.Add(new ScriptBundle("~/Scripts/bundles/main").Include(
                "~/Scripts/Bundles/main.js"));

            bundles.Add(new StyleBundle("~/Content/bundles/styles").Include(
                "~/Content/Bundles/styles.css", new CssRewriteUrlTransform()));
        }
    }
}