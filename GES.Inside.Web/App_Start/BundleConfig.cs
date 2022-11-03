using System.Web.Optimization;

namespace GES.Inside.Web
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
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery.unobtrusive").Include());

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.js",
                        "~/Scripts/ng-file-upload-shim.js",
                        "~/Scripts/ng-file-upload.js",
                        "~/Content/plugins/angular-ui/ui-select2/select2.js",
                        "~/Content/plugins/daterangepicker/daterangepicker.js",
                        "~/Content/plugins/datepicker/bootstrap-datepicker.js",
                        "~/Content/plugins/timepicker/bootstrap-timepicker.js",
                        "~/Content/plugins/wickedpicker/wickedpicker.min.js",
                        "~/Content/plugins/ng-table/ng-table.min.js",
                        "~/Content/plugins/ng-dialog/ngDialog.min.js",
                        "~/Content/plugins/text-angular/angular.min.js",
                        "~/Content/plugins/text-angular/textAngular-rangy.min.js",
                        "~/Content/plugins/text-angular/textAngular-sanitize.min.js",
                        "~/Content/plugins/text-angular/textAngular.min.js",
                        "~/Content/plugins/tree-dropdown/list-to-tree.js",
                        "~/Content/plugins/dropdownMultiselect/isteven-multi-select.js"
                        ));
            bundles.Add(new Bundle("~/bundles/angular2").Include(
                       "~/Scripts/libs/runtime*",
                       "~/Scripts/libs/polyfills*",
                       "~/Scripts/libs/vendor*",
                       "~/Scripts/libs/main*"));        

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/others").Include(
                      "~/Content/plugins/js-cookie/js.cookie.js",
                      "~/Content/js/utils.js",
                      "~/Content/plugins/fastclick/fastclick.js",
                      "~/Content/plugins/slimScroll/jquery.slimscroll.js",
                      "~/Content/plugins/jQuery-dateFormat/jquery-dateFormat.min.js",
                      "~/Content/plugins/bootbox/bootbox.min.js",
                      "~/Content/plugins/perfect-scrollbar/js/min/perfect-scrollbar.jquery.min.js",
                      "~/Content/plugins/iCheck/icheck.js",
                      "~/Content/plugins/daterangepicker/moment.js",
                      "~/Content/plugins/toastr/toastr.min.js",

                      // jqGrid
                      "~/Content/plugins/jqGrid/js/grid.locale-en.js",
                      "~/Content/plugins/jqGrid/js/jquery.jqGrid.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/tinymce").Include(
                      "~/Content/plugins/tinymce/tinymce.min.js",
                      "~/Content/plugins/tinymce/themes/modern/theme.min.js"
                      ));
            //
            // Javascript for Pages
            //
            bundles.Add(new ScriptBundle("~/bundles/page-portfolio-list").Include(
                        "~/Content/plugins/x-editable-bs3/js/bootstrap-editable.min.js",
                        "~/Content/plugins/select2/select2.full.js",
                        "~/Content/plugins/jquery.qtip2/jquery.qtip.min.js",
                        "~/Content/js/pages/page-portfolio-list.js"));
            bundles.Add(new ScriptBundle("~/bundles/page-portfolio-controactiv-presets").Include(
                        "~/Content/js/pages/page-portfolio-controactiv-presets.js"));
            bundles.Add(new ScriptBundle("~/bundles/page-portfolio-details").Include(
                        "~/Content/plugins/x-editable-bs3/js/bootstrap-editable.min.js",
                        "~/Content/plugins/select2/select2.full.js",
                        "~/Content/js/pages/page-portfolio-details.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-client-list").Include(
                        "~/Content/js/pages/page-client-list.js"));
                    
            bundles.Add(new ScriptBundle("~/bundles/page-client-details").Include(
                        "~/Content/plugins/select2/select2.full.js",
                        "~/Content/plugins/confirm-bootstrap/confirm-bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-company-list").Include(
                        "~/Content/js/pages/page-company-list.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-case-profile-list").Include(
                        "~/Content/plugins/select2/select2.full.js",
                        "~/Content/angular/controllers/caseProfile/caseProfileTemplateController.js",
                        "~/Content/js/pages/page-case-profile-list.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-engagement-types-list").Include(
                "~/Content/js/pages/page-engagement-type-list.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-edit-angular").Include(
                        "~/Content/plugins/select2/select2.full.js",
                        "~/Content/plugins/confirm-bootstrap/confirm-bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-company-details").Include(
                "~/Content/plugins/select2/select2.full.js",
                "~/Content/angular/controllers/caseProfile/caseProfileTemplateController.js",
                "~/Content/plugins/confirm-bootstrap/confirm-bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-accountmgmt-list").Include(
                        "~/Content/js/pages/page-accountmgmt-list.js"));
            bundles.Add(new ScriptBundle("~/bundles/page-accountmgmt-details").Include(
                        "~/Content/plugins/select2/select2.full.js",
                        "~/Content/js/pages/page-accountmgmt-details.js",
                        "~/Content/plugins/confirm-bootstrap/confirm-bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-glossary-list").Include(
                        "~/Content/plugins/speakingurl/speakingurl.min.js",
                        "~/Content/plugins/slugify/slugify.min.js",
                        "~/Content/js/pages/page-glossary-list.js"));
            bundles.Add(new ScriptBundle("~/bundles/page-create-glossary").Include(
                        "~/Content/js/pages/page-create-glossary.js"));
            bundles.Add(new ScriptBundle("~/bundles/page-data-index").Include(
                        "~/Content/js/pages/page-data-index.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-utility-format-isins").Include(
                        "~/Content/js/pages/page-utility-format-isins.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-case-report-signup-list").Include(
                        "~/Content/js/pages/page-case-report-signup-list.js"));
            bundles.Add(new ScriptBundle("~/bundles/page-case-report-user-signup-list").Include(
                        "~/Content/js/pages/page-case-report-user-signup-list.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-documentmgmt-list").Include(
                        "~/Content/js/pages/page-documentmgmt-list.js"
                        ));            
            bundles.Add(new ScriptBundle("~/bundles/page-companydocumentmgmt-list").Include(
                        "~/Content/js/pages/page-companydocumentmgmt-list.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/page-documentmgmt-detail").Include(
                        "~/Content/plugins/select2/select2.full.js",
                       "~/Content/js/pages/page-documentmgmt-detail.js"
                       ));            
            bundles.Add(new ScriptBundle("~/bundles/page-companydocumentmgmt-detail").Include(
                        "~/Content/plugins/select2/select2.full.js",
                       "~/Content/js/pages/page-companydocumentmgmt-detail.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/page-engagement-type-details").Include(
                "~/Content/plugins/select2/select2.full.js",
                "~/Content/plugins/ng-table/ng-table.min.js",
                "~/Content/plugins/confirm-bootstrap/confirm-bootstrap.js"));
                    
                    
            bundles.Add(new ScriptBundle("~/bundles/page-ges-sevices-list").Include(
                                "~/Content/js/pages/page-ges-services-list.js"));
                    
            bundles.Add(new ScriptBundle("~/bundles/page-ges-case-profile-ui-template-list").Include(
                    "~/Content/js/pages/page-ges-case-profile-ui-template-list.js"));                    
                    
            bundles.Add(new ScriptBundle("~/bundles/page-ges-services-details").Include(
                    "~/Content/plugins/select2/select2.full.js",
                    "~/Content/plugins/confirm-bootstrap/confirm-bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/page-ges-case-profile-ui-template-details").Include(
                    "~/Content/plugins/select2/select2.full.js",
                    "~/Content/plugins/confirm-bootstrap/confirm-bootstrap.js"));  
                    
            bundles.Add(new ScriptBundle("~/bundles/page-convention-list").Include(
                    "~/Content/js/pages/page-convention-list.js"));   

            bundles.Add(new ScriptBundle("~/bundles/page-convention-details").Include(
                    "~/Content/plugins/select2/select2.full.js",
                    "~/Content/plugins/confirm-bootstrap/confirm-bootstrap.js"));            
                    
            bundles.Add(new ScriptBundle("~/bundles/page-guiderline-list").Include(
                    "~/Content/js/pages/page-guiderlines-list.js"));   

            bundles.Add(new ScriptBundle("~/bundles/page-guiderline-details").Include(
                    "~/Content/plugins/select2/select2.full.js",
                    "~/Content/plugins/confirm-bootstrap/confirm-bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/page-screening-report").Include(
                    "~/Content/plugins/select2/select2.full.js",
                    "~/Content/plugins/dropdownMultiselect/isteven-multi-select.js",
                    "~/Content/plugins/confirm-bootstrap/confirm-bootstrap.js"));   
        

            bundles.Add(new ScriptBundle("~/bundles/page-roles-list").Include(
                "~/Content/js/pages/page-roles-list.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-role-details").Include(
                "~/Content/plugins/select2/select2.full.js",
                "~/Content/plugins/confirm-bootstrap/confirm-bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-gss-research-company-list").Include(
                    "~/Content/js/pages/page-rss-research-company-list.js"));
        

            bundles.Add(new ScriptBundle("~/bundles/angular-module").Include(
                "~/Content/angular/app.js",
                //Controllers
                "~/Content/angular/controllers/caseProfileController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileDialogueController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileCommentaryController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileGSSLinkController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileNewsController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileDiscussionController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileStakeholderController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileKPIController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileSdgController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileGuidelineController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileConventionController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileAssociatedCorporationsController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileSourcesController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileReferencesController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileSupplementaryReadingController.js",                
                "~/Content/angular/controllers/caseProfile/caseProfileAdditionalDocumentController.js",                
                "~/Content/angular/controllers/sdgController.js",
                "~/Content/angular/controllers/companyController.js",
                "~/Content/angular/controllers/company/companyManagementSystemController.js",
                "~/Content/angular/controllers/company/companyPortfolioController.js",
                "~/Content/angular/controllers/company/companyFollowingPortfolioController.js",
                "~/Content/angular/controllers/company/companyFTSEController.js",
                "~/Content/angular/controllers/company/companyEventsController.js",                
                "~/Content/angular/controllers/company/companyMSCIController.js",
                "~/Content/angular/controllers/company/companyAdditionalDocumentController.js",
                "~/Content/angular/controllers/engagementTypeController.js",
                "~/Content/angular/controllers/gesServicesController.js",
                "~/Content/angular/controllers/clientController.js",
                "~/Content/angular/controllers/gesAnnouncementController.js",
                "~/Content/angular/controllers/gesContactController.js",                
                "~/Content/angular/controllers/caseProfile/caseProfileUNGPController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileRecommendationLogController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileMileStoneController.js",
                "~/Content/angular/controllers/caseProfile/caseProfileConclusionLogController.js",
                "~/Content/angular/controllers/caseProfileUITemplateController.js",
                "~/Content/angular/controllers/conventionController.js",
                "~/Content/angular/controllers/guiderlineController.js",
                "~/Content/angular/controllers/screeningReportsController.js",                
                "~/Content/angular/controllers/roleController.js",
                "~/Content/angular/controllers/screeningReportsController.js",                //Directives
                "~/Content/angular/directives/clickToEdit.js",
                "~/Content/angular/directives/ngEnter.js",
                "~/Content/angular/directives/ngConfirm.js",
                "~/Content/angular/directives/clickToEditTextArea.js",
                "~/Content/angular/directives/datePicker.js",
                "~/Content/angular/directives/combobox.js",
                "~/Content/angular/directives/ngJqGrid.js",
                //Services
                "~/Content/angular/services/caseProfileService.js",
                "~/Content/angular/services/modalService.js",                
                "~/Content/angular/services/sdgService.js",
                "~/Content/angular/services/companyService.js",
                "~/Content/angular/services/engagementTypeService.js",
                "~/Content/angular/services/gesServicesService.js",
                "~/Content/angular/services/clientService.js",
                "~/Content/angular/services/gesAnnouncementService.js",
                "~/Content/angular/services/gesCaseProfileUITemplateService.js",
                "~/Content/angular/services/gesContactService.js",
                "~/Content/angular/services/gesOrganizationService.js",
                "~/Content/angular/services/conventionService.js",
                "~/Content/angular/services/guiderlineService.js",
                "~/Content/angular/services/screeningReportsService.js", 
                "~/Content/angular/services/roleService.js"));

            bundles.Add(new ScriptBundle("~/bundles/page-dashboard").Include(
                "~/Content/js/pages/dashboard.js",
                "~/Content/plugins/select2/select2.full.js",
                "~/Content/angular/controllers/caseProfile/caseProfileTemplateController.js",
                "~/Content/plugins/confirm-bootstrap/confirm-bootstrap.js"));

            //
            // Stylesheet
            //
            bundles.Add(new StyleBundle("~/bundles/css-bootstrap").Include(
                      "~/Content/bootstrap/css/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/plugins/jquery-ui-bootstrap/jquery-ui-bundle").Include(
                      "~/Content/plugins/jquery-ui-bootstrap/jquery-ui-1.10.0.custom.css",
                      "~/Content/plugins/jquery-ui-bootstrap/jquery.ui.1.10.0.ie.css"
                      ));
            bundles.Add(new StyleBundle("~/Content/plugins/flag-icon-css/flag-css-bundle").Include(
                "~/Content/plugins/flag-icon-css/flag-icon.css"
            ));
            bundles.Add(new StyleBundle("~/bundles/css-plugins").Include(
                      "~/Content/plugins/jqGrid/css/ui.jqgrid.css",
                      "~/Content/hrstyles.css",
                      "~/Content/plugins/toastr/toastr.min.css",
                      "~/Content/plugins/select2/select2.min.css",
                      "~/Content/plugins/select2/select2-bootstrap.css",
                      "~/Content/plugins/timepicker/bootstrap-timepicker.css",
                      "~/Content/plugins/wickedpicker/wickedpicker.min.css",
                      "~/Content/plugins/jquery.qtip2/jquery.qtip.min.css",
                      "~/Content/plugins/perfect-scrollbar/css/perfect-scrollbar.css",
                      "~/Content/plugins/ng-table/ng-table.min.min.css",
                      "~/Content/plugins/ng-dialog/ngDialog.min.css",
                      "~/Content/plugins/ng-dialog/ngDialog-theme-default.css",
                      "~/Content/plugins/tree-dropdown/tree-dropdown.css",
                      "~/Content/plugins/dropdownMultiselect/isteven-multi-select.css"
                      ));
            bundles.Add(new StyleBundle("~/bundles/css-adminlte-theme").Include(
                      "~/Content/css/AdminLTE.css",
                      "~/Content/css/skins/_all-skins.css",
                      "~/Content/shared/shared.css",
                      "~/Content/css/custom.css",
                      "~/Content/css/indigo-pink.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/plugins/x-editable-bs3/x-editable-css-bundle").Include(
                      "~/Content/plugins/x-editable-bs3/css/bootstrap-editable.css"
                      ));

            //Engagement type
            bundles.Add(new StyleBundle("~/bundles/page-engagement-type-css").Include(
                "~/Content/css/page-engagement-type.css"
            ));
            
            bundles.Add(new StyleBundle("~/bundles/page-client-details-css").Include(
                "~/Content/css/page-client-details.css",
                "~/Content/plugins/flag-icon-css/flag-icon.css"
            ));           
            
            bundles.Add(new StyleBundle("~/bundles/page-gss-research-company-css").Include(
                "~/Content/css/page-gss-research-company.css"
            ));

            

#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
