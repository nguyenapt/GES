using System.Web.Optimization;

namespace GES.Clients.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //
            // Javascript
            //
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Content/plugins/jquery-ui-1.11.4/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery.unobtrusive").Include("~/Scripts/jquery.unobtrusive-ajax.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/js-plugins").Include());

            bundles.Add(new ScriptBundle("~/bundles/others").Include(
                      "~/Content/plugins/js-cookie/js.cookie.js",
                      "~/Content/js/utils.js",
                      "~/Content/plugins/fastclick/fastclick.js",
                      "~/Content/plugins/slimScroll/jquery.slimscroll.js",
                      "~/Content/plugins/jQuery-dateFormat/jquery-dateFormat.min.js",
                      "~/Content/plugins/bootbox/bootbox.min.js",
                      "~/Content/plugins/perfect-scrollbar/js/min/perfect-scrollbar.jquery.min.js",
                      "~/Content/plugins/iCheck/icheck.js",
                      "~/Content/plugins/toastr/toastr.min.js",
                      "~/Content/plugins/jquery.qtip2/jquery.qtip.js",
                      "~/Content/plugins/handlebars/handlebars-v4.0.5.js",
                      "~/Content/plugins/jquery.dotdotdot/jquery.dotdotdot.js",
                      "~/Content/plugins/daterangepicker/daterangepicker.js",
                      "~/Content/plugins/datepicker/bootstrap-datepicker.js",
                      "~/Content/plugins/timepicker/bootstrap-timepicker.js",
                      "~/Content/plugins/select2/select2.full.js",
                      "~/Content/plugins/bootstrap-select-1.10.0/js/bootstrap-select.js",
                      "~/Content/plugins/jquery.shorten/jquery.shorten.js",
                      "~/Content/plugins/jquery.highlight/jquery.highlight.js",
                      "~/Content/plugins/linkify/linkify.js",
                      "~/Content/plugins/linkify/linkify-jquery.js",

                      "~/Content/plugins/jvectormap/jquery-jvectormap-2.0.3.min.js",
                      "~/Content/plugins/jvectormap/jquery-jvectormap-world-mill.js",

                      // jqGrid
                      "~/Content/plugins/jqGrid/js/grid.locale-en.js",
                      "~/Content/plugins/jqGrid/js/jquery.jqGrid.js",

                      "~/Content/plugins/readmore/readmore.js",                                                        
                      "~/Content/js/main.js",
                      "~/Content/plugins/horizontal-timeline/js/main.js"
                      ));

            //
            // Javascript for Pages
            //

            bundles.Add(new ScriptBundle("~/bundles/page-company-search-list").Include(
                "~/Content/plugins/highcharts/highcharts.src.js",
                "~/Content/js/pages/page-company-search-list.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-stewardship-and-risk-theme").Include(
                "~/Content/plugins/momentjs/moment.js",
                "~/Content/plugins/eventify/js/eventify-timeline.js",
                "~/Content/plugins/highcharts/highcharts.src.js",
                "~/Content/js/pages/page-company-search-list.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/page-user-profile").Include(
                        "~/Content/js/pages/page-user-profile.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-reset-password").Include(
                        "~/Content/js/pages/page-reset-password.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-forgot-password").Include(
                        "~/Content/js/pages/page-forgot-password.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-glossary").Include(
                        "~/Content/plugins/smooth-scroll/jquery.smooth-scroll.js",
                        "~/Content/js/pages/page-glossary.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/page-company-profile").Include(
                        "~/Content/plugins/momentjs/moment.js",
                        "~/Content/plugins/eventify/js/eventify.js",
                        "~/Content/js/pages/page-company-profile.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/page-my-endorsement").Include(
                        "~/Content/js/pages/page-my-endorsement.js"
                        ));
            
            bundles.Add(new ScriptBundle("~/bundles/page-dashboard").Include(
                        "~/Content/plugins/momentjs/moment.js",
                        "~/Content/plugins/momentjs/moment-parseformat.js",
                        "~/Content/plugins/eventify/js/eventify.js",
                        "~/Content/plugins/chartjs/Chart.js",
                        "~/Content/js/pages/page-dashboard.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-homepage").Include(
                        "~/Content/plugins/momentjs/moment.js",
                        "~/Content/plugins/momentjs/moment-parseformat.js",
                        "~/Content/plugins/eventify/js/eventify.js",
                        "~/Content/plugins/chartjs/Chart.js",
                        "~/Content/js/pages/page-homepage.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-case-report-detail").Include(
                        "~/Content/plugins/momentjs/moment.js",
                        "~/Content/plugins/momentjs/moment-parseformat.js",
                        "~/Content/plugins/eventify/js/eventify.js",
                        "~/Content/plugins/highcharts/highcharts.src.js",
                        "~/Content/js/pages/page-case-report-detail.js"));

            bundles.Add(new ScriptBundle("~/bundles/timelineControl-js").Include(
                "~/Content/js/timelineControl.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/page-calendar-list").Include(
                "~/Content/js/pages/page-calendar-list.js"
            ));

            //
            // Stylesheet
            //

            bundles.Add(new ScriptBundle("~/bundles/case-profile-export-pdf").Include(
                "~/Content/plugins/highcharts/highcharts.src.js",
                "~/Content/js/pages/page-case-report.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/css-codyhouse-horizontal-timeline").Include(
                      "~/Content/plugins/horizontal-timeline/css/reset.css",
                      "~/Content/plugins/horizontal-timeline/css/style.css"));

            bundles.Add(new StyleBundle("~/bundles/css-bootstrap").Include(
                      "~/Content/bootstrap/css/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/plugins/jquery-ui-bootstrap/jquery-ui-bundle").Include(
                      "~/Content/plugins/jquery-ui-bootstrap/jquery-ui-1.10.0.custom.css",
                      "~/Content/plugins/jquery-ui-bootstrap/jquery.ui.1.10.0.ie.css"
                      ));
            bundles.Add(new StyleBundle("~/bundles/css-plugins").Include(                
                      "~/Content/plugins/jqGrid/css/ui.jqgrid.css",
                      "~/Content/hrstyles.css",
                      "~/Content/animate.css",
                      "~/Content/plugins/toastr/toastr.min.css",
                      "~/Content/plugins/select2/select2.min.css",
                      "~/Content/plugins/select2/select2-bootstrap.css",

                      "~/Content/plugins/bootstrap-select-1.10.0/css/bootstrap-select.css",
                      "~/Content/plugins/jvectormap/jquery-jvectormap-2.0.3.css",

                      "~/Content/plugins/jquery.qtip2/jquery.qtip.min.css",
                      "~/Content/plugins/perfect-scrollbar/css/perfect-scrollbar.css",
                      "~/Content/plugins/datepicker/datepicker3.css",
                      "~/Content/plugins/timepicker/bootstrap-timepicker.min.css"
                      ));
            bundles.Add(new StyleBundle("~/bundles/css-adminlte-theme").Include(
                      "~/Content/css/AdminLTE.css",
                      "~/Content/css/skins/_all-skins.css",
                      "~/Content/shared/shared.css",
                      "~/Content/css/custom.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/plugins/x-editable-bs3/x-editable-css-bundle").Include(
                      "~/Content/plugins/x-editable-bs3/css/bootstrap-editable.css"
                      ));
            bundles.Add(new StyleBundle("~/Content/plugins/flag-icon-css/flag-css-bundle").Include(
                      "~/Content/plugins/flag-icon-css/flag-icon.css"
                      ));
            bundles.Add(new StyleBundle("~/Content/plugins/iCheck/iCheck-css-bundle").Include(
                      "~/Content/plugins/iCheck/iCheck.css"
                      ));
            bundles.Add(new StyleBundle("~/Content/plugins/eventify/eventify-css-bundle").Include(
                      "~/Content/plugins/eventify/eventify.css"
                      ));
            bundles.Add(new StyleBundle("~/bundles/page-glossary-css").Include(
                      "~/Content/css/page-glossary.css"
                      ));
            bundles.Add(new StyleBundle("~/bundles/page-company-profile-print-css").Include(
                      "~/Content/css/page-company-profile-print.css"
                      ));
            bundles.Add(new StyleBundle("~/bundles/page-legal-css").Include(
                      "~/Content/css/page-legal.css"
                      ));
            bundles.Add(new StyleBundle("~/bundles/page-changlog-css").Include(
                      "~/Content/css/page-changelog.css"
                      ));

            bundles.Add(new StyleBundle("~/bundles/timelineControl-css").Include(
                "~/Content/timelineControl.css"
            ));

            // Case Profile Pdf
            bundles.Add(new StyleBundle("~/bundles/page-case-profile-print-css").Include(
                "~/Content/css/page-case-profile-print.css"
            ));
#if DEBUG
            BundleTable.EnableOptimizations = false;
            #else
                BundleTable.EnableOptimizations = true;
            #endif
        }
    }
}
