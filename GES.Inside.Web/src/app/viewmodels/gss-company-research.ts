import { UNGuidingPrinciple } from "./un-guiding-principles";
import { CommentDetailModel} from "./comment-details-model";
import { GssPrincipleAssessment } from "./gss-principle-assessment";
import { GssPrincipleGeneral } from "./gss-principle-general";
import { GssPrincipleUpgradeCriteria } from "./gss-principle-upgrade-criteria";
import { GssPrincipleCompanyContact } from "./gss-principle-company-contact";
import { GssSource } from './gss-source';
import { GssInternalComment } from './gss-internal-comment';
import { Helper } from '../helper/Helper';
import { GssPrinciple } from './gss-principle';

export class GssCompanyResearch {
    
  //constructor(Id, Name, CompanyStatus, EngagementStatus, GssResearchCompanyUnGuidingPrinciplesViewModels, InternalCommentDetails, GssOutlookComments){
  //      this.Id = Id;
  //      this.Name = Name;
  //      this.CompanyStatus = CompanyStatus;
  //      this.EngagementStatus = EngagementStatus;
  //      this.GssResearchCompanyUnGuidingPrinciplesViewModels = [];
  //      this.InternalCommentDetails = [];
  //      this.GssOutlookComments = [];
   //     this.GssPrincipleUpgradeCriteriaViewModel = GssPrincipleUpgradeCriteriaViewModel;
   //     this.GssPrincipleCompanyContactViewModel = GssPrincipleCompanyContactViewModel;
   //     this.GssSourceViewModels = [];
   //     this.GssInternalCommentViewModels = []

  //    GssResearchCompanyUnGuidingPrinciplesViewModels.forEach((myObject, index) => {

  //      let uNGuidingPrinciple = new UNGuidingPrinciple();
  //      uNGuidingPrinciple.name = myObject.Name;
  //      uNGuidingPrinciple.id = myObject.Id;
  //      this.GssResearchCompanyUnGuidingPrinciplesViewModels.push(uNGuidingPrinciple);
  //  });

  //  InternalCommentDetails.forEach((comment, index) => {

  //    let internalComment = new CommentDetailModel();
  //    internalComment.description = comment.Description;
  //    internalComment.id = comment.Id;
  //    internalComment.created = comment.Created;
  //    this.InternalCommentDetails.push(internalComment);
  //  });

  //  GssOutlookComments.forEach((outlook, index) => {
  //    let gssOutlookComments = new CommentDetailModel();
  //    gssOutlookComments.description = outlook.Description;
  //    gssOutlookComments.id = outlook.Id;
  //    gssOutlookComments.created = outlook.Created;
  //    this.GssOutlookComments.push(gssOutlookComments);
  //  });

  //  }
            //this.GssInternalCommentViewModels.push(uInternalComment);
  //      });    
    Id: number;
    Name:string;
    CompanyStatus: number;
    EngagementStatus: number;
    GssResearchCompanyUnGuidingPrinciplesViewModels: UNGuidingPrinciple[];
    InternalAnalystComments: CommentDetailModel[];
    GssOutlookComments: CommentDetailModel[];

    OverallAssesment:string;
    CompanyId: string;
    ISIN: string;
    SubpeerGroupName: string;
    GssPrincipleAssessmentViewModel: GssPrincipleAssessment;
    GssPrincipleGeneralViewModel: GssPrincipleGeneral;
    GssPrincipleUpgradeCriteriaViewModel: GssPrincipleUpgradeCriteria;
    GssPrincipleCompanyContactViewModel: GssPrincipleCompanyContact;
    GssSourceViewModels: GssSource[];
    GssInternalCommentViewModels: GssInternalComment[];
    GssResearchIssueIndicatorsViewModels: GssPrinciple[];
    Country: string;
    Website: string;
    BussinessDescription: string;

    OutlookEffectiveSince: string;
    UNGlobalCompact: string;
    UNGuildingPrinceples: string;
    OECDGuideline: string;
    OtherRelatedConventions: string;
    EngagementStatusAsOf: string;
    EngagementStatusName : string;
}
