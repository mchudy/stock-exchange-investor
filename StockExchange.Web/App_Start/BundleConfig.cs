using System.Collections.Generic;
using System.Web.Optimization;

namespace StockExchange.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts/jquery").NonOrdering().Include(
                "~/bower_components/jquery/dist/jquery.min.js",
                "~/bower_components/jquery-validation/dist/jquery.validate.js",
                "~/bower_components/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js",
                "~/bower_components/select2/dist/js/select2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/jquery-ui").NonOrdering().Include(
                "~/bower_components/jquery-ui/jquery-ui.min.js",
                "~/bower_components/jquery-ui-multiselect-widget/src/jquery.multiselect.js",
                "~/bower_components/jquery-ui-multiselect-widget/src/jquery.multiselect.filter.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/bootstrap").Include(
                "~/bower_components/bootstrap/dist/js/bootstrap.min.js",
                "~/bower_components/Bootflat/bootflat/js/icheck.min.js",
                "~/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/datatables").Include(
                "~/bower_components/datatables.net/js/jquery.dataTables.js",
                "~/bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js",
                "~/bower_components/datatables.net-responsive/js/dataTables.responsive.min.js",
                "~/Scripts/dataTablesExtensions.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/highcharts").Include(
                "~/bower_components/highcharts/highstock.js",
                "~/bower_components/highcharts/modules/exporting.js",
                "~/bower_components/highcharts/modules/offline-exporting.js"));

            bundles.Add(new StyleBundle("~/bundles/styles/vendor")
                .Include("~/bower_components/bootstrap/dist/css/bootstrap.min.css", new CssRewriteUrlTransform())
                .Include("~/bower_components/jquery-ui/themes/base/jquery-ui.min.css", new CssRewriteUrlTransform())
                .Include("~/bower_components/Bootflat/bootflat/css/bootflat.min.css",
                "~/bower_components/select2/dist/css/select2.min.css",
                "~/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker3.css",
                "~/bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css")
                .Include("~/bower_components/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/bundles/styles/jquery-ui-multiselect").Include(
                "~/bower_components/jquery-ui-multiselect-widget/jquery.multiselect.css",
                "~/bower_components/jquery-ui-multiselect-widget/jquery.multiselect.filter.css"));
        }
    }

    internal class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }

    internal static class BundleExtentions
    {
        public static Bundle NonOrdering(this Bundle bundle)
        {
            bundle.Orderer = new NonOrderingBundleOrderer();
            return bundle;
        }
    }
}