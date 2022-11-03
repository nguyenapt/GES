using System;
using System.Collections.Generic;
using GES.Inside.Data.Models.CaseProfiles;

namespace GES.Inside.Web.Models
{
    public class CaseReportGesUngpAssessmentFormViewModel
    {
        public Guid GesUNGPAssessmentFormId { get; set; }
        public long I_GesCaseReports_Id { get; set; }
        
        public Guid? TheExtentOfHarmesScoreId { get; set; }
        public string TheExtentOfHarmesScoreValue { get; set; }
        public string TheExtentOfHarmesScoreComment { get; set; }
        
        public Guid? TheNumberOfPeopleAffectedScoreId { get; set; }
        public string TheNumberOfPeopleAffectedScoreValue { get; set; }
        public string TheNumberOfPeopleAffectedScoreComment { get; set; }
        
        public Guid? OverSeveralYearsScoreId { get; set; }
        public string OverSeveralYearsScoreValue { get; set; }
        public string OverSeveralYearsScoreComment { get; set; }
        
        public Guid? SeveralLocationsScoreId { get; set; }
        public string SeveralLocationsScoreValue { get; set; }
        public string SeveralLocationsScoreComment { get; set; }
        
        public Guid? IsViolationScoreId { get; set; }
        public string IsViolationScoreValue{ get; set; }
        public string IsViolationScoreComment { get; set; }
        
        public Guid? GesConfirmedViolationScoreId { get; set; }
        public string GesConfirmedViolationScoreComment { get; set; }
        public string GesConfirmedViolationScoreValue{ get; set; }

        public double? SalientHumanRightsPotentialViolationTotalScore { get; set; }
        public string GesCommentSalientHumanRight { get; set; }
        
        public Guid? HumanRightsPolicyPubliclyDisclosedAddScoreId { get; set; }
        public string HumanRightsPolicyPubliclyDisclosed { get; set; }
        public string HumanRightsPolicyPubliclyDisclosedAddScoreValue{ get; set; }

        public Guid? HumanRightsPolicyCommunicatedScoreId { get; set; }
        public string HumanRightsPolicyCommunicated { get; set; }
        public string HumanRightsPolicyCommunicatedScoreValue{ get; set; }

        public Guid? HumanRightsPolicyStipulatesScoreId { get; set; }
        public string HumanRightsPolicyStipulates { get; set; }
        public string HumanRightsPolicyStipulatesScoreValue{ get; set; }

        public Guid? HumanRightsPolicyApprovedScoreId { get; set; }
        public string HumanRightsPolicyApproved { get; set; }
        public string HumanRightsPolicyApprovedScoreValue{ get; set; }

        public double? HumanRightsPolicyTotalScore { get; set; }
        public Guid? GovernanceCommitmentScoreId { get; set; }
        public string GovernanceCommitmentScoreValue{ get; set; }

        public string GovernanceCommitment { get; set; }
        public Guid? GovernanceExamplesScoreId { get; set; }
        public string GovernanceExamplesScoreValue { get; set; }

        public string GovernanceExamples { get; set; }
        public Guid? GovernanceClearDivisionScoreId { get; set; }
        public string GovernanceClearDivisionScoreValue{ get; set; }

        public string GovernanceClearDivision { get; set; }
        
        public string BusinessPartners { get; set; }
        public Guid? BusinessPartnersAddScoreId { get; set; }
        public string BusinessPartnersAddScoreValue{ get; set; }

        public Guid? IdentificationAndCommitmentScoreId { get; set; }
        public string IdentificationAndCommitment { get; set; }
        public string IdentificationAndCommitmentScoreValue{ get; set; }

        public string StakeholderEngagement { get; set; }
        public Guid? StakeholderEngagementAddScoreId { get; set; }
        public string StakeholderEngagementAddScoreValue{ get; set; }

        public string HumanRightsTraining { get; set; }
        public Guid? HumanRightsTrainingScoreId { get; set; }
        public string HumanRightsTrainingScoreValue{ get; set; }

        public double? TotalScoreForHumanRightsDueDiligence { get; set; }
        public Guid? RemedyProcessInPlaceScoreId { get; set; }
        public string RemedyProcessInPlaceScoreValue{ get; set; }

        public string RemedyProcessInPlace { get; set; }
        public Guid? GrievanceMechanismHasOperationalLevelScoreId { get; set; }
        public string GrievanceMechanismHasOperationalLevelScoreValue{ get; set; }

        public string GrievanceMechanismHasOperationalLevel { get; set; }
        public Guid? GrievanceMechanismExistenceOfOperationalLevelScoreId { get; set; }
        public string GrievanceMechanismExistenceOfOperationalLevelScoreValue{ get; set; }

        public string GrievanceMechanismExistenceOfOperationalLevel { get; set; }
        public Guid? GrievanceMechanismClearProcessScoreId { get; set; }
        public string GrievanceMechanismClearProcessScoreValue{ get; set; }

        public string GrievanceMechanismClearProcess { get; set; }
        public Guid? GrievanceMechanismRightsNormsScoreId { get; set; }
        public string GrievanceMechanismRightsNormsScoreValue{ get; set; }

        public string GrievanceMechanismRightsNorms { get; set; }
        public Guid? GrievanceMechanismFilingGrievanceScoreId { get; set; }
        public string GrievanceMechanismFilingGrievanceScoreValue{ get; set; }

        public string GrievanceMechanismFilingGrievance { get; set; }
        public Guid? GrievanceMechanismReoccurringGrievancesScoreId { get; set; }
        public string GrievanceMechanismReoccurringGrievancesScoreValue{ get; set; }

        public string GrievanceMechanismReoccurringGrievances { get; set; }
        public Guid? GrievanceMechanismFormatAndProcesseScoreId { get; set; }
        public string GrievanceMechanismFormatAndProcesseScoreValue{ get; set; }

        public string GrievanceMechanismFormatAndProcesse { get; set; }
        public double? TotalScoreForRemediationOfAdverseHumanRightsImpacts { get; set; }
        public string GesCommentCompanyPreparedness { get; set; }
        public double? TotalScoreForCompanyPreparedness { get; set; }
        public bool? IsPublished { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Created { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByString { get; set; }
        
        public IEnumerable<GesUNGPAssessmentFormResourcesViewModel> AssessmentFormResourcesViewModels { get; set; }
        public GesUngpAuditViewModel GesUngpAuditViewModel { get; set; }
    }
}