using System;
using System.Collections.Generic;

namespace GES.Inside.Data.Models
{
    public class GesUngpAssessmentScoresViewModels
    {
        public IEnumerable<GesUngpAssessmentScoresViewModel> ScaleExtentOfHarmScore { get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> ScaleNumberOfPeopleAffectedScore { get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> SystematicOverServeralYearsScore { get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> SystematicOverServeralLocationScore{ get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> OngoingViolationOccurringScore{ get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> ConfirmedViolationOfInternationalNormsScore{ get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> HumanRightsPubliclyDisclosedAdditionalScore{ get; set; }
        
        
        public IEnumerable<GesUngpAssessmentScoresViewModel> HumanRightsPubliclyDisclosedCommunicatedScore{ get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> HumanRightsPubliclyDisclosedExpectationsPersonnelScore{ get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> HumanRightsPubliclyDisclosedExpectationsPolicyApproved { get; set; }
        
        
        public IEnumerable<GesUngpAssessmentScoresViewModel> HumanRightsGovernanceCommitment { get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> HumanRightsGovernanceProvidesExamples { get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> HumanRightsGovernanceClearDivision { get; set; }
        
        public IEnumerable<GesUngpAssessmentScoresViewModel> HumanRightsBusinessPartnersAdditionalScore{ get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> HumanRightsIdentificationCommitment { get; set; }
        
        public IEnumerable<GesUngpAssessmentScoresViewModel> HumanRightsStakeholderEngagementAdditionalScore{ get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> HumanRightsTrainningAdditionalScore{ get; set; }
        
        public IEnumerable<GesUngpAssessmentScoresViewModel> RemediationAdverseHumanRightsImpactsRemedyProcess { get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> RemediationAdverseHumanRightsImpactsGrievancMechanismOperationalLevel { get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> RemediationAdverseHumanRightsImpactsGrievancMechanismExistenceOperationalLevel { get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> RemediationAdverseHumanRightsImpactsGrievancMechanismDisclosesClearProcess{ get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> RemediationAdverseHumanRightsImpactsGrievancMechanismAddressesGrievance{ get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> RemediationAdverseHumanRightsImpactsGrievancMechanismFilingGrievance{ get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> RemediationAdverseHumanRightsImpactsGrievancMechanismReoccurringGrievances { get; set; }
        public IEnumerable<GesUngpAssessmentScoresViewModel> RemediationAdverseHumanRightsImpactsGrievancMechanismFormatProcesses { get; set; }
    }
}