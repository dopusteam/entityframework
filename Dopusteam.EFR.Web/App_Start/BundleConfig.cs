using System.Web.Optimization;

namespace Dopusteam.EFR.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/src/angular")
                .Include("~/Scripts/angular.js")
                .Include("~/Scripts/angular-ui-router.js"));

            bundles.Add(new ScriptBundle("~/app/js")
                .IncludeDirectory("~/App/", "*.js", true));
        }
    }
}
