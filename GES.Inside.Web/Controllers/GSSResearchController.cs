using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Common.Models;
using GES.Inside.Web.Models;
using GES.Inside.Data.Models;
using System;

namespace GES.Inside.Web.Controllers
{
    public class GSSResearchController : GesControllerBase
    {
        #region Declaration

        private readonly IGSSResearchCompaniesService _gssResearchCompaniesService;

        #endregion

        #region Constructor

        public GSSResearchController(IGesLogger logger, IGSSResearchCompaniesService gssResearchCompaniesService
        ) : base(logger)
        {
            _gssResearchCompaniesService = gssResearchCompaniesService;
        }

        #endregion

        #region ActionResult

        //[CustomAuthorize(FormKey = "GSSCompanyList", Action = ActionEnum.Read)]
        public ActionResult CompanyList()
        {



            this.SafeExecute(() =>
            {
                ViewBag.Assessments = GetAssessments();
                ViewBag.WorkflowStatues = GetWorkflowStatues();
                ViewBag.Flags = GetFlags();
                ViewBag.GssocReviews = GetGssocReview();
            }, $"Error when getting the viewbag information. Please check inner exception for detail.");

            return View();
        }

        public ActionResult CompanyDetails()
        {

            this.SafeExecute(() =>
            {

            }, $"Error when getting the viewbag information. Please check inner exception for detail.");

            return View();
        }

        //[CustomAuthorize(FormKey = "GSSResourceAllocation", Action = ActionEnum.Read)]
        public ActionResult ResourceAllocation()
        {
            this.SafeExecute(() =>
            {

            }, $"Error when getting the viewbag information. Please check inner exception for detail.");

            return View();
        }

        //[CustomAuthorize(FormKey = "GSSBulkStatusChange", Action = ActionEnum.Read)]
        public ActionResult BulkStatusChange()
        {
            this.SafeExecute(() =>
            {

            }, $"Error when getting the viewbag information. Please check inner exception for detail.");

            return View();
        }

        //[CustomAuthorize(FormKey = "GSSBulkCompliantAssessment", Action = ActionEnum.Read)]
        public ActionResult BulkCompliantAssessment()
        {
            this.SafeExecute(() =>
            {

            }, $"Error when getting the viewbag information. Please check inner exception for detail.");

            return View();
        }

        #endregion

        #region JsonResult

        [HttpPost]
        public JsonResult GetDataForRssResearchCompaniesJqGrid(JqGridViewModel jqGridParams)
        {
            var listCompany = this.SafeExecute(() => _gssResearchCompaniesService.GetGSSResearchCompanies(jqGridParams), "Error when getting the companies {@JqGridViewModel}", jqGridParams);

            return Json(listCompany);
        }

        [HttpGet]
        public JsonResult GetGssCompanyDetails(long id)
        {

            var companyUnGuidingPrinciplesViewModels = new List<GssResearchCompanyUNGuidingPrinciplesViewModel>();
            var internalAnalystComment = new List<CommentDetailsViewModel>();
            var gssOutlookComment = new List<CommentDetailsViewModel>();
            var companyUnGuidingPrinciplesViewModel = new GssResearchCompanyUNGuidingPrinciplesViewModel
            {
                Id = 1,
                Name = "Respect for Human Rights"
            };

            companyUnGuidingPrinciplesViewModels.Add(companyUnGuidingPrinciplesViewModel);

            companyUnGuidingPrinciplesViewModel = new GssResearchCompanyUNGuidingPrinciplesViewModel
            {
                Id = 2,
                Name = "Respect Int. recognized Human Rights"
            };
            companyUnGuidingPrinciplesViewModels.Add(companyUnGuidingPrinciplesViewModel);


            internalAnalystComment.Add(new CommentDetailsViewModel()
            {
                Id = 1,
                Created = "2019/06/22",
                Description = "Loren ip sum"
            });
            internalAnalystComment.Add(new CommentDetailsViewModel()
            {
                Id = 2,
                Created = "2019/06/24",
                Description = "Loren ip sum"

            });

            gssOutlookComment.Add(new CommentDetailsViewModel()
            {
                Id = 1,
                Created = "2019/05/23",
                Description = "Loren ipsum...."
            });

            var gssResearchCompanyViewModel = new GssResearchCompanyViewModel
            {
                Id = id,
                Name = "1-800-Flowers.com Inc",
                CompanyStatus = 1,
                EngagementStatus = 2,
                GssResearchCompanyUnGuidingPrinciplesViewModels = companyUnGuidingPrinciplesViewModels,
                InternalAnalystComments = internalAnalystComment.OrderByDescending(d => d.Created).ToList(),
                GssOutlookComments = gssOutlookComment.OrderByDescending(d => d.Created).ToList(),
                OverallAssesment = "Compliant",
                CompanyId = "11920283874",
                ISIN = "CM983038940",
                SubpeerGroupName = "Energy",
                Country = "France",
                Website = "www.1800flowers.com",
                BussinessDescription = "Lorem ipsum ...........",
                OutlookEffectiveSince = "2019/06/25",
                UNGlobalCompact = "Princeple1: Non-Compliant",
                UNGuildingPrinceples = "Non-Compliant",
                GssSourceViewModels = new List<GssResearchSourceViewModel>() {
                    new GssResearchSourceViewModel(){Id = Guid.NewGuid(),GssId=Guid.NewGuid(),DateModified=DateTime.Now,Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure" },
                    new GssResearchSourceViewModel(){Id = Guid.NewGuid(),GssId=Guid.NewGuid(),DateModified=DateTime.Now,Description="At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga" }
                },
                GssInternalCommentViewModels = new List<GssResearchInternalCommentViewModel>() {
                    new GssResearchInternalCommentViewModel(){Id = Guid.NewGuid(),GssId=Guid.NewGuid(),DateModified=DateTime.Now,User_Id=1,UserName="Patrick",Level=0,Comment="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum" },
                    new GssResearchInternalCommentViewModel(){Id = Guid.NewGuid(),GssId=Guid.NewGuid(),DateModified=DateTime.Now,User_Id=1,UserName="Patrick",Level=0,Comment="Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam," },
                    new GssResearchInternalCommentViewModel(){Id = Guid.NewGuid(),GssId=Guid.NewGuid(),DateModified=DateTime.Now,User_Id=1,UserName="Patrick",Level=1,Comment="But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful." },
                    new GssResearchInternalCommentViewModel(){Id = Guid.NewGuid(),GssId=Guid.NewGuid(),DateModified=DateTime.Now,User_Id=1,UserName="Patrick",Level=1,Comment="At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus" },
                    new GssResearchInternalCommentViewModel(){Id = Guid.NewGuid(),GssId=Guid.NewGuid(),DateModified=DateTime.Now,User_Id=1,UserName="Patrick",Level=2,Comment="Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus" },
                    new GssResearchInternalCommentViewModel(){Id = Guid.NewGuid(),GssId=Guid.NewGuid(),DateModified=DateTime.Now,User_Id=1,UserName="Patrick",Level=2,Comment="Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?" }
                },
                OECDGuideline = "Chapter VI: ....",
                OtherRelatedConventions = "Agenda 21, lorem ipsum ......",
                EngagementStatusName = "Ongoing",
                EngagementStatusAsOf = "2019/05/12",
                GssResearchIssueIndicatorsViewModels = new List<GssResearchPrincipleIssueIndicatorViewModel>()
                {
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Labour Rights",Group=1,Assessment = new GssResearchPrincipleAssessmentViewModel(),CompanyContact = new GssResearchPrincipleCompanyContactViewModel(),General = new GssResearchPrincipleGeneralViewModel(),UpgradeCriteria = new GssResearchPrincipleUpgradeCriteriaViewModel()},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Health and Safety",Group=1,Assessment = new GssResearchPrincipleAssessmentViewModel(),CompanyContact = new GssResearchPrincipleCompanyContactViewModel(),General = new GssResearchPrincipleGeneralViewModel(),UpgradeCriteria = new GssResearchPrincipleUpgradeCriteriaViewModel()},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Land Rights",Group=1,Assessment = new GssResearchPrincipleAssessmentViewModel(),CompanyContact = new GssResearchPrincipleCompanyContactViewModel(),General = new GssResearchPrincipleGeneralViewModel(),UpgradeCriteria = new GssResearchPrincipleUpgradeCriteriaViewModel()},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Water Rights",Group=1,Assessment = new GssResearchPrincipleAssessmentViewModel(),CompanyContact = new GssResearchPrincipleCompanyContactViewModel(),General = new GssResearchPrincipleGeneralViewModel(),UpgradeCriteria = new GssResearchPrincipleUpgradeCriteriaViewModel()},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Access to Basic Services",Group=1,Assessment = new GssResearchPrincipleAssessmentViewModel(),CompanyContact = new GssResearchPrincipleCompanyContactViewModel(),General = new GssResearchPrincipleGeneralViewModel(),UpgradeCriteria = new GssResearchPrincipleUpgradeCriteriaViewModel()},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Community Relations",Group=1,Assessment = new GssResearchPrincipleAssessmentViewModel(),CompanyContact = new GssResearchPrincipleCompanyContactViewModel(),General = new GssResearchPrincipleGeneralViewModel(),UpgradeCriteria = new GssResearchPrincipleUpgradeCriteriaViewModel()},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Data Privacy and Security",Group=1,Assessment = new GssResearchPrincipleAssessmentViewModel(),CompanyContact = new GssResearchPrincipleCompanyContactViewModel(),General = new GssResearchPrincipleGeneralViewModel(),UpgradeCriteria = new GssResearchPrincipleUpgradeCriteriaViewModel()},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Employees - Human Rights",Group=1,Assessment = new GssResearchPrincipleAssessmentViewModel(),CompanyContact = new GssResearchPrincipleCompanyContactViewModel(),General = new GssResearchPrincipleGeneralViewModel(),UpgradeCriteria = new GssResearchPrincipleUpgradeCriteriaViewModel()},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Marketing Practices",Group=1,Assessment = new GssResearchPrincipleAssessmentViewModel(),CompanyContact = new GssResearchPrincipleCompanyContactViewModel(),General = new GssResearchPrincipleGeneralViewModel(),UpgradeCriteria = new GssResearchPrincipleUpgradeCriteriaViewModel()},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Occupational Health and Safety",Group=1,Assessment = new GssResearchPrincipleAssessmentViewModel(),CompanyContact = new GssResearchPrincipleCompanyContactViewModel(),General = new GssResearchPrincipleGeneralViewModel(),UpgradeCriteria = new GssResearchPrincipleUpgradeCriteriaViewModel()},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Quality and Safety",Group=1,Assessment = new GssResearchPrincipleAssessmentViewModel(),CompanyContact = new GssResearchPrincipleCompanyContactViewModel(),General = new GssResearchPrincipleGeneralViewModel(),UpgradeCriteria = new GssResearchPrincipleUpgradeCriteriaViewModel()},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Social Impact of Products",Group=1,Assessment = new GssResearchPrincipleAssessmentViewModel(),CompanyContact = new GssResearchPrincipleCompanyContactViewModel(),General = new GssResearchPrincipleGeneralViewModel(),UpgradeCriteria = new GssResearchPrincipleUpgradeCriteriaViewModel()},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Society - Human Rights",Group=1,Assessment = new GssResearchPrincipleAssessmentViewModel(),CompanyContact = new GssResearchPrincipleCompanyContactViewModel(),General = new GssResearchPrincipleGeneralViewModel(),UpgradeCriteria = new GssResearchPrincipleUpgradeCriteriaViewModel()},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Human Rights  - Supply Chain",Group=1,Assessment = new GssResearchPrincipleAssessmentViewModel(),CompanyContact = new GssResearchPrincipleCompanyContactViewModel(),General = new GssResearchPrincipleGeneralViewModel(),UpgradeCriteria = new GssResearchPrincipleUpgradeCriteriaViewModel()},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Consumer Interests",Group=1,Assessment = new GssResearchPrincipleAssessmentViewModel(),CompanyContact = new GssResearchPrincipleCompanyContactViewModel(),General = new GssResearchPrincipleGeneralViewModel(),UpgradeCriteria = new GssResearchPrincipleUpgradeCriteriaViewModel()},

                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Involvement With Entities Violating Human Rights",Group=2},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Anti Personnel Mines",Group=2},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Chemical and Biological weapons",Group=2},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Cluster Weapons",Group=2},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Nuclear Weapons",Group=2},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Weapons",Group=2},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Financing of Controversial Project",Group=2},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Sanctions",Group=2},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Freedom of Expression",Group=2},

                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Freedom of Association",Group=3},

                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Forced Labour",Group=4},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Forced Labour Supply Chain ",Group=4},

                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Child Labour",Group=5},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Child Labour Supply Chain",Group=5},

                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Discrimination & Harassment",Group=6},

                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Degradation & Contamination (Land)",Group=7},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Discharges and Releases (Water)",Group=7},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Emissions to Air",Group=7},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Animal Welfare",Group=7},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Energy Use and Greenhouse Gas Emissions",Group=7},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Spills Resulting in Environmental Impacts",Group=7},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Water Use",Group=7},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Energy Use and Greenhouse Gas Emissions",Group=7},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Emissions, Effluents and Waste",Group=7},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Environmental Impact of Products",Group=7},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Land Use and Biodiversity",Group=7},

                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Promote Environmental Responsibility ",Group=8},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Environmentally Friendly Technologies",Group=9},

                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Bribery and Corruption",Group=10},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Tax Avoidance and Evasion",Group=10},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Business Ethics",Group=10},
                    new GssResearchPrincipleIssueIndicatorViewModel(){Id=Guid.NewGuid(),Title="Accounting Irregularities and Accounting Fraud",Group=10}
                }
            };
            return Json(gssResearchCompanyViewModel, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateOrAddNewGssCompany(GssResearchCompanyViewModel gssResearchCompanyViewModel)
        {

            return Json(new { Id = gssResearchCompanyViewModel.Id, Status = "Success" });
        }

        [HttpPost]
        public JsonResult SaveComment(GssInternalCommentViewModel gssInternalCommentViewModel)
        {

            return Json(new { Id = gssInternalCommentViewModel.Id, Status = "Success" });
        }


        #endregion

        #region Private methods

        private string[] GetAssessments()
        {
            var assessments = new[] { "Compliant", "Non-Compliant", "Watchlist" };
            return assessments.ToArray();
        }

        private string[] GetWorkflowStatues()
        {
            var assessments = new[] { "Draft", "Review", "Editing", "Final", "Published" };
            return assessments.ToArray();
        }

        private string[] GetFlags()
        {
            var assessments = new[] { "GSSOC", "Corporate Tree", "Parked" };
            return assessments.ToArray();
        }
        private string[] GetGssocReview()
        {
            var assessments = new[] { "Yes", "No" };
            return assessments.ToArray();
        }

        [HttpGet]
        public JsonResult GetGssSources()
        {
            var gssSources = new List<GssResearchSourceViewModel>() {
                new GssResearchSourceViewModel(){Id = Guid.NewGuid(),GssId=Guid.NewGuid(),DateModified=DateTime.Now,Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure" },
                new GssResearchSourceViewModel(){Id = Guid.NewGuid(),GssId=Guid.NewGuid(),DateModified=DateTime.Now,Description="At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga" },
            };
            return Json(gssSources, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGssInternalComments()
        {
            var gssSources = new List<GssResearchInternalCommentViewModel>() {
                new GssResearchInternalCommentViewModel(){Id = Guid.NewGuid(),GssId=Guid.NewGuid(),DateModified=DateTime.Now,User_Id=1,UserName="Patrick",Level=0,Comment="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum" },
                new GssResearchInternalCommentViewModel(){Id = Guid.NewGuid(),GssId=Guid.NewGuid(),DateModified=DateTime.Now,User_Id=1,UserName="Patrick",Level=0,Comment="Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam," },
                new GssResearchInternalCommentViewModel(){Id = Guid.NewGuid(),GssId=Guid.NewGuid(),DateModified=DateTime.Now,User_Id=1,UserName="Patrick",Level=1,Comment="But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful." },
                new GssResearchInternalCommentViewModel(){Id = Guid.NewGuid(),GssId=Guid.NewGuid(),DateModified=DateTime.Now,User_Id=1,UserName="Patrick",Level=1,Comment="At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus" },
                new GssResearchInternalCommentViewModel(){Id = Guid.NewGuid(),GssId=Guid.NewGuid(),DateModified=DateTime.Now,User_Id=1,UserName="Patrick",Level=2,Comment="Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus" },
                new GssResearchInternalCommentViewModel(){Id = Guid.NewGuid(),GssId=Guid.NewGuid(),DateModified=DateTime.Now,User_Id=1,UserName="Patrick",Level=2,Comment="Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?" }
            };
            return Json(gssSources, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}