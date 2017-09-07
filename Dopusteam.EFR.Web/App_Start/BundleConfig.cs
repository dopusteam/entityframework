using System.Web.Optimization;

namespace Dopusteam.EFR.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/styles/bootstrap")
                .Include("~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/src/bootstrap")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/jquery-1.10.2.js"));

            bundles.Add(new ScriptBundle("~/src/angular")
                .Include("~/Scripts/angular.js")
                .Include("~/Scripts/angular-ui-router.js"));

            bundles.Add(new ScriptBundle("~/app/js")
                .IncludeDirectory("~/App/", "*.js", true));
        }
    }
}
