"use strict";
GesInsideApp.controller("CaseProfileUNGPController", ["$scope", "$filter", "$timeout", "$q", "$window", "CaseProfileService", "NgTableParams", function ($scope, $filter, $timeout, $q, $window, CaseProfileService, NgTableParams) {
    $scope.isNewCaseProfileUNGP = false;
    $scope.UngpAssessmentScores = [];
    $scope.GesUngpAssessmentFormAuditViewModels = [];
    $scope.GesUngpAssessmentFormViewModel = null;
    $scope.GesUngpAssessmentFormViewModelMaster = null;
    $scope.editingSources = [];
    $scope.isScoresLoaded = false;
    $scope.showUngp = false;

    $scope.scaleExtentOfHarmScore = null;
    $scope.scaleNumberOfPeopleAffectedScore = null;
    $scope.systematicOverServeralYearsScore = null;
    $scope.systematicOverServeralLocationScore = null;
    $scope.ongoingViolationOccurringScore = null;
    $scope.confirmedViolationOfInternationalNormsScore = null;
    $scope.humanRightsPubliclyDisclosedAdditionalScore = null;
    $scope.humanRightsBusinessPartnersAdditionalScore = null;
    $scope.humanRightsStakeholderEngagementAdditionalScore = null;
    $scope.humanRightsTrainningAdditionalScore = null;
    $scope.salientHumanRightsPotentialViolationTotalScore = 0;
    $scope.totalScoreForHumanRightsPolicy = 0;
    $scope.totalScoreForHumanRightsDueDiligence = 0;
    $scope.totalScoreForRemediationOfAdverseHumanRightsImpacts = 0;
    $scope.totalScoreForCompanyPreparedness = 0;

    $scope.humanRightsPubliclyDisclosedCommunicatedScore = null;
    $scope.humanRightsPubliclyDisclosedExpectationsPersonnelScore = null;
    $scope.humanRightsPubliclyDisclosedExpectationsPolicyApproved = null;
    $scope.humanRightsGovernanceCommitment = null;
    $scope.humanRightsGovernanceProvidesExamples = null;
    $scope.humanRightsGovernanceClearDivision = null;
    $scope.humanRightsIdentificationCommitment = null;
    $scope.remediationAdverseHumanRightsImpactsRemedyProcess = null;
    $scope.remediationAdverseHumanRightsImpactsGrievancMechanismOperationalLevel = null;
    $scope.remediationAdverseHumanRightsImpactsGrievancMechanismExistenceOperationalLevel = null;
    $scope.remediationAdverseHumanRightsImpactsGrievancMechanismDisclosesClearProcess = null;
    $scope.remediationAdverseHumanRightsImpactsGrievancMechanismAddressesGrievance = null;
    $scope.remediationAdverseHumanRightsImpactsGrievancMechanismFilingGrievance = null;
    $scope.remediationAdverseHumanRightsImpactsGrievancMechanismReoccurringGrievances = null;
    $scope.remediationAdverseHumanRightsImpactsGrievancMechanismFormatProcesses = null;
    
    $scope.isUngpPublishable = false;
    $scope.validateMessage = [];
    $scope.isFormInvalid = false;

    $scope.caseProfileId = 0;
    
    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });  

    function init() {
        $scope.caseProfileId = caseId;
        $scope.isNewCaseProfileUNGP = CaseProfileService.IsAddNewCaseProfile();
        GetGesUngpAssessmentData($scope.caseProfileId);
    }
    
    $scope.GetUpdateUngp = function (caseProfileId) {

        $timeout(function () {
            if (!$scope.isScoresLoaded) {
                GetUngpAssessmentScores()
                    .then(GetGesUngpAssessmentData(caseProfileId))
                    .then(function () {
                            quickNotification("Data loading completed", "success");
                        },
                        function (failReason) {
                            quickNotification("Error occurred during loading data " + failReason, "error");
                        });
            } else {
                GetGesUngpAssessmentData(caseProfileId)
                    .then(function () {
                            quickNotification("Data loading completed", "success");
                        },
                        function (failReason) {
                            quickNotification("Error occurred during loading data " + failReason, "error");
                        });
            }
            
        });       

    };

    var GetUngpAssessmentScores = function () {
        var deferred = $q.defer();        

        CaseProfileService.GetUngpAssessmentScores().then(
            function (response) {
                $scope.UngpAssessmentScores = response.data;

                if ($scope.UngpAssessmentScores != null) {

                    $scope.scaleExtentOfHarmScore = $scope.UngpAssessmentScores.ScaleExtentOfHarmScore;
                    $scope.scaleNumberOfPeopleAffectedScore = $scope.UngpAssessmentScores.ScaleNumberOfPeopleAffectedScore;
                    $scope.systematicOverServeralYearsScore = $scope.UngpAssessmentScores.SystematicOverServeralYearsScore;
                    $scope.systematicOverServeralLocationScore = $scope.UngpAssessmentScores.SystematicOverServeralLocationScore;
                    $scope.ongoingViolationOccurringScore = $scope.UngpAssessmentScores.OngoingViolationOccurringScore;
                    $scope.confirmedViolationOfInternationalNormsScore = $scope.UngpAssessmentScores.ConfirmedViolationOfInternationalNormsScore;
                    $scope.humanRightsPubliclyDisclosedAdditionalScore = $scope.UngpAssessmentScores.HumanRightsPubliclyDisclosedAdditionalScore;
                    $scope.humanRightsBusinessPartnersAdditionalScore = $scope.UngpAssessmentScores.HumanRightsBusinessPartnersAdditionalScore;
                    $scope.humanRightsStakeholderEngagementAdditionalScore = $scope.UngpAssessmentScores.HumanRightsStakeholderEngagementAdditionalScore;
                    $scope.humanRightsTrainningAdditionalScore = $scope.UngpAssessmentScores.HumanRightsTrainningAdditionalScore;

                    $scope.humanRightsPubliclyDisclosedCommunicatedScore = $scope.UngpAssessmentScores.HumanRightsPubliclyDisclosedCommunicatedScore;
                    $scope.humanRightsPubliclyDisclosedExpectationsPersonnelScore = $scope.UngpAssessmentScores.HumanRightsPubliclyDisclosedExpectationsPersonnelScore;
                    $scope.humanRightsPubliclyDisclosedExpectationsPolicyApproved = $scope.UngpAssessmentScores.HumanRightsPubliclyDisclosedExpectationsPolicyApproved;
                    $scope.humanRightsGovernanceCommitment = $scope.UngpAssessmentScores.HumanRightsGovernanceCommitment;
                    $scope.humanRightsGovernanceProvidesExamples = $scope.UngpAssessmentScores.HumanRightsGovernanceProvidesExamples;
                    $scope.humanRightsGovernanceClearDivision = $scope.UngpAssessmentScores.HumanRightsGovernanceClearDivision;
                    $scope.humanRightsIdentificationCommitment = $scope.UngpAssessmentScores.HumanRightsIdentificationCommitment;
                    $scope.remediationAdverseHumanRightsImpactsRemedyProcess = $scope.UngpAssessmentScores.RemediationAdverseHumanRightsImpactsRemedyProcess;
                    $scope.remediationAdverseHumanRightsImpactsGrievancMechanismOperationalLevel = $scope.UngpAssessmentScores.RemediationAdverseHumanRightsImpactsGrievancMechanismOperationalLevel;
                    $scope.remediationAdverseHumanRightsImpactsGrievancMechanismExistenceOperationalLevel = $scope.UngpAssessmentScores.RemediationAdverseHumanRightsImpactsGrievancMechanismExistenceOperationalLevel;
                    $scope.remediationAdverseHumanRightsImpactsGrievancMechanismDisclosesClearProcess = $scope.UngpAssessmentScores.RemediationAdverseHumanRightsImpactsGrievancMechanismDisclosesClearProcess;
                    $scope.remediationAdverseHumanRightsImpactsGrievancMechanismAddressesGrievance = $scope.UngpAssessmentScores.RemediationAdverseHumanRightsImpactsGrievancMechanismAddressesGrievance;
                    $scope.remediationAdverseHumanRightsImpactsGrievancMechanismFilingGrievance = $scope.UngpAssessmentScores.RemediationAdverseHumanRightsImpactsGrievancMechanismFilingGrievance;
                    $scope.remediationAdverseHumanRightsImpactsGrievancMechanismReoccurringGrievances = $scope.UngpAssessmentScores.RemediationAdverseHumanRightsImpactsGrievancMechanismReoccurringGrievances;
                    $scope.remediationAdverseHumanRightsImpactsGrievancMechanismFormatProcesses = $scope.UngpAssessmentScores.RemediationAdverseHumanRightsImpactsGrievancMechanismFormatProcesses;
                   
                    $scope.isScoresLoaded = true;
                }

                deferred.resolve('Get Norms Services');

            },
            function (reason) {
                quickNotification("Error occurred during loading Ungp Assessment Scores, caused: " + reason, "error");
            }
        );
       
        return deferred.promise;
    };    
    
    var GetGesUngpAssessmentData = function (caseProfileId) {
        var deferred = $q.defer();
        $scope.GesUngpAssessmentFormViewModel = [];

        CaseProfileService.GetGesUngpAssessment(caseProfileId).then(
            function (response) {
               $scope.GesUngpAssessmentFormViewModelMaster = response.data;
               $scope.reset();

               if($scope.GesUngpAssessmentFormViewModel != null && $scope.GesUngpAssessmentFormViewModel.GesUNGPAssessmentFormId !== "00000000-0000-0000-0000-000000000000"){
                   $scope.showUngp = true;
               }
               ungpPopulateData();
                

                deferred.resolve('Get UNGP Services');

            },
            function (reason) {
                quickNotification("Error occurred during loading Ungp Assessment Scores, caused: " + reason, "error");
            }
        );
       
        return deferred.promise;
    };

    function ungpPopulateData() {
        //$scope.GesUngpAssessmentFormViewModel = $scope.caseProfile.GesUngpAssessmentFormViewModel;
        if ($scope.GesUngpAssessmentFormViewModel != null) {
            if ($scope.GesUngpAssessmentFormViewModel.SalientHumanRightsPotentialViolationTotalScore !== null) {
                $scope.salientHumanRightsPotentialViolationTotalScore = $scope.GesUngpAssessmentFormViewModel.SalientHumanRightsPotentialViolationTotalScore;
            }

            if ($scope.GesUngpAssessmentFormViewModel.HumanRightsPolicyTotalScore !== null) {
                $scope.totalScoreForHumanRightsPolicy = $scope.GesUngpAssessmentFormViewModel.HumanRightsPolicyTotalScore;
            }

            if ($scope.GesUngpAssessmentFormViewModel.TotalScoreForHumanRightsDueDiligence !== null) {
                $scope.totalScoreForHumanRightsDueDiligence = $scope.GesUngpAssessmentFormViewModel.TotalScoreForHumanRightsDueDiligence;
            }

            if ($scope.GesUngpAssessmentFormViewModel.TotalScoreForRemediationOfAdverseHumanRightsImpacts !== null) {
                $scope.totalScoreForRemediationOfAdverseHumanRightsImpacts = $scope.GesUngpAssessmentFormViewModel.TotalScoreForRemediationOfAdverseHumanRightsImpacts;
            }

            if ($scope.GesUngpAssessmentFormViewModel.TotalScoreForCompanyPreparedness !== null) {
                $scope.totalScoreForCompanyPreparedness = $scope.GesUngpAssessmentFormViewModel.TotalScoreForCompanyPreparedness;
            }

            if ($scope.GesUngpAssessmentFormViewModel.Created){
                $scope.GesUngpAssessmentFormViewModel.Created =
                    $scope.convertDate($scope.GesUngpAssessmentFormViewModel.Created, 'yyyy/MM/dd HH:mm:ss a');

            }

            if ($scope.GesUngpAssessmentFormViewModel.Modified){
                $scope.GesUngpAssessmentFormViewModel.Modified =
                    $scope.convertDate($scope.GesUngpAssessmentFormViewModel.Modified, 'yyyy/MM/dd HH:mm:ss a');

            }

            if ($scope.GesUngpAssessmentFormViewModel.AssessmentFormResourcesViewModels !== null) {
                $scope.editingSources = $scope.GesUngpAssessmentFormViewModel.AssessmentFormResourcesViewModels;
                if ($scope.editingSources!=null && $scope.editingSources.length > 0) {
                    for (var i = 0; i < $scope.editingSources.length; i++) {

                        if (($scope.editingSources[i].SourceDate != null)) {
                            $scope.editingSources[i].SourceDate =
                                new Date($scope.convertDate($scope.editingSources[i].SourceDate, 'yyyy/MM/dd'));
                        }
                    }
                }
            } else {
                $scope.isNewCaseProfileUNGP = true;
            }

            if ($scope.GesUngpAssessmentFormViewModel.GesUngpAuditViewModel != null) {
                $scope.GesUngpAssessmentFormAuditViewModels = $scope.GesUngpAssessmentFormViewModel.GesUngpAuditViewModel.GesUngpAssessmentFormAuditViewModels;

                $scope.ungpMainHistoryTableParams = new NgTableParams({
                    page: 1,            // show first page
                    count: 5          // count per page    
                }, {
                    total: ($scope.GesUngpAssessmentFormAuditViewModels != null? $scope.GesUngpAssessmentFormAuditViewModels.length:0), // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.GesUngpAssessmentFormAuditViewModels != null?$scope.GesUngpAssessmentFormAuditViewModels.length:0);
                        $scope.data = $scope.GesUngpAssessmentFormAuditViewModels.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        return $scope.data;
                    }
                });
            }

            CaseProfileService.caseProfile.GesUngpAssessmentFormViewModel = $scope.GesUngpAssessmentFormViewModel;
        }
    }

    $scope.saveUngp = function () {
        var ungp = $scope.GesUngpAssessmentFormViewModel;
       
        CaseProfileService.SaveUngp($scope.caseProfileId, ungp, saveUngpCallback);
    };

    $scope.reset = function() {
        $scope.GesUngpAssessmentFormViewModel = angular.copy($scope.GesUngpAssessmentFormViewModelMaster);
    };
    
    $scope.ShowHideOnclient = function(isIgnoreValidate) {

        if ($scope.GesUngpAssessmentFormViewModel.IsPublished && !isIgnoreValidate){
            $scope.isUngpPublishable = true;

            if (isFormInvalid()){
                $scope.isFormInvalid = true;
                $scope.isNewCaseProfileUNGP = true;
                $scope.GesUngpAssessmentFormViewModel.IsPublished = false;
                return;
            }
        }
        if (isIgnoreValidate){
            $scope.GesUngpAssessmentFormViewModel.IsPublished = true;
        }              

        CaseProfileService.ShowHideUngp($scope.caseProfileId, $scope.GesUngpAssessmentFormViewModel.IsPublished, saveUngpCallback);
    };

    function isFormInvalid() {
        var result = false;
        var errorMessage = "";
        $scope.validateMessage = [];

        if($scope.GesUngpAssessmentFormViewModel.TheExtentOfHarmesScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.TheExtentOfHarmesScoreValue === null){
            result = true;
            var error = {
                code: "theextentofharmesscorevalue-input", text: "The extent of harm"
            };
            $scope.validateMessage.push(error);
        }

        if($scope.GesUngpAssessmentFormViewModel.TheNumberOfPeopleAffectedScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.TheNumberOfPeopleAffectedScoreValue === null){
            result = true;
            var error = {
                code: "thenumberofpeopleaffectedscorevalue-input", text: "The number of people affected"
            };
            $scope.validateMessage.push(error);
        }
        
        if($scope.GesUngpAssessmentFormViewModel.OverSeveralYearsScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.OverSeveralYearsScoreValue === null){
            result = true;
            var error = {
                code: "overseveralyearsscore-input", text: "Over several years"
            };
            $scope.validateMessage.push(error);
        }    
        
        if($scope.GesUngpAssessmentFormViewModel.SeveralLocationsScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.SeveralLocationsScoreValue === null){
            result = true;
            var error = {
                code: "severallocationsscore-input", text: "Over several years"
            };
            $scope.validateMessage.push(error);
        }   
        
        if($scope.GesUngpAssessmentFormViewModel.SeveralLocationsScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.SeveralLocationsScoreValue === null){
            result = true;
            var error = {
                code: "severallocationsscore-input", text: "Several locations"
            };
            $scope.validateMessage.push(error);
        }  
        
        if($scope.GesUngpAssessmentFormViewModel.IsViolationScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.IsViolationScoreValue === null){
            result = true;
            var error = {
                code: "violationscore-input", text: "violation still occurring"
            };
            $scope.validateMessage.push(error);
        }  
        
        if($scope.GesUngpAssessmentFormViewModel.GesConfirmedViolationScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.GesConfirmedViolationScoreValue === null){
            result = true;
            var error = {
                code: "gesconfirmedviolationscore-input", text: "GES’ confirmed violation of international norms"
            };
            $scope.validateMessage.push(error);
        }  

        if($scope.GesUngpAssessmentFormViewModel.GesCommentSalientHumanRight === undefined || $scope.GesUngpAssessmentFormViewModel.GesCommentSalientHumanRight === null){
            result = true;
            var error = {
                code: "gescommentgescommentsalienthumanright-input", text: "Level of human rights salience - GES general comments"
            };
            $scope.validateMessage.push(error);
        }


        if($scope.GesUngpAssessmentFormViewModel.HumanRightsPolicyPubliclyDisclosedAddScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.HumanRightsPolicyPubliclyDisclosedAddScoreValue === null){
            result = true;
            var error = {
                code: "humanrightspolicypubliclydisclosedaddscore-input", text: "A publicly disclosed human rights policy"
            };
            $scope.validateMessage.push(error);
        }

        if($scope.GesUngpAssessmentFormViewModel.HumanRightsPolicyCommunicatedScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.HumanRightsPolicyCommunicatedScoreValue === null){
            result = true;
            var error = {
                code: "humanrightspolicycommunicatedscore-input", text: "The company states how the policy is communicated internally and externally to all personnel.."
            };
            $scope.validateMessage.push(error);
        }
        
        if($scope.GesUngpAssessmentFormViewModel.HumanRightsPolicyStipulatesScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.HumanRightsPolicyStipulatesScoreValue === null){
            result = true;
            var error = {
                code: "humanrightspolicystipulatesscore-input", text: "The policy stipulates the enterprise’s human rights expectations of personnel.."
            };
            $scope.validateMessage.push(error);
        }
        
        if($scope.GesUngpAssessmentFormViewModel.HumanRightsPolicyApprovedScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.HumanRightsPolicyApprovedScoreValue === null){
            result = true;
            var error = {
                code: "humanrightspolicyapprovedscore-input", text: "The policy is approved at the most senior level of the business enterprise"
            };
            $scope.validateMessage.push(error);
        }        
        
        if($scope.GesUngpAssessmentFormViewModel.GovernanceCommitmentScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.GovernanceCommitmentScoreValue === null){
            result = true;
            var error = {
                code: "governancecommitmentscore-input", text: "A written commitment from the company’s senior management and/or board to lead..."
            };
            $scope.validateMessage.push(error);
        }        
        
        if($scope.GesUngpAssessmentFormViewModel.GovernanceExamplesScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.GovernanceExamplesScoreValue === null){
            result = true;
            var error = {
                code: "governanceexamplesscore-input", text: "The company provides examples of how top management and/or the board have initiated human rights initiatives"
            };
            $scope.validateMessage.push(error);
        }
        
        if($scope.GesUngpAssessmentFormViewModel.GovernanceClearDivisionScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.GovernanceClearDivisionScoreValue === null){
            result = true;
            var error = {
                code: "governancecleardivisionscore-input", text: "The company provides a clear division of responsibility within the company to manage the company’s salient human rights issues..."
            };
            $scope.validateMessage.push(error);
        }
        
        if($scope.GesUngpAssessmentFormViewModel.BusinessPartnersAddScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.BusinessPartnersAddScoreValue === null){
            result = true;
            var error = {
                code: "businesspartnersaddscorevalue-input", text: "The company takes human rights considerations into account when deciding to engage (or terminate) business relationships..."
            };
            $scope.validateMessage.push(error);
        }
        
        if($scope.GesUngpAssessmentFormViewModel.IdentificationAndCommitmentScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.IdentificationAndCommitmentScoreValue === null){
            result = true;
            var error = {
                code: "identificationandcommitmentscore-input", text: "The company demonstrates a continuous process to determine salient human rights issues and discloses results"
            };
            $scope.validateMessage.push(error);
        }
        
        if($scope.GesUngpAssessmentFormViewModel.StakeholderEngagementAddScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.StakeholderEngagementAddScoreValue === null){
            result = true;
            var error = {
                code: "stakeholderengagementaddscore-input", text: "The company demonstrates how stakeholder engagement influences the company’s understanding of salient human rights issue and/or its approach to addressing them"
            };
            $scope.validateMessage.push(error);
        }

        if($scope.GesUngpAssessmentFormViewModel.HumanRightsTrainingScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.HumanRightsTrainingScoreValue === null){
            result = true;
            var error = {
                code: "humanrightstrainingscore-input", text: "Human rights training is conducted for employees, contract workers and suppliers"
            };
            $scope.validateMessage.push(error);
        }

        if($scope.GesUngpAssessmentFormViewModel.RemedyProcessInPlaceScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.RemedyProcessInPlaceScoreValue === null){
            result = true;
            var error = {
                code: "remedyprocessinplacescore-input", text: "The company has a clear process in place to address and remedy human rights impacts"
            };
            $scope.validateMessage.push(error);
        }

        if($scope.GesUngpAssessmentFormViewModel.GrievanceMechanismHasOperationalLevelScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.GrievanceMechanismHasOperationalLevelScoreValue === null){
            result = true;
            var error = {
                code: "grievancemechanismhasoperationallevelscore-input", text: "The company has operational-level mechanism(s) through which people affected by company operations..."
            };
            $scope.validateMessage.push(error);
        }

        if($scope.GesUngpAssessmentFormViewModel.GrievanceMechanismExistenceOfOperationalLevelScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.GrievanceMechanismExistenceOfOperationalLevelScoreValue === null){
            result = true;
            var error = {
                code: "grievancemechanismexistenceofoperationallevelscore-input", text: "The existence of operational-level grievance mechanism is clearly communicated in relevant local languages to those affected or potentially affected..."
            };
            $scope.validateMessage.push(error);
        }

        if($scope.GesUngpAssessmentFormViewModel.GrievanceMechanismClearProcessScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.GrievanceMechanismClearProcessScoreValue === null){
            result = true;
            var error = {
                code: "grievancemechanismclearprocessscore-input", text: "The company discloses a clear process on how grievances are managed from operational-level grievance mechanisms"
            };
            $scope.validateMessage.push(error);
        }

        if($scope.GesUngpAssessmentFormViewModel.GrievanceMechanismRightsNormsScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.GrievanceMechanismRightsNormsScoreValue === null){
            result = true;
            var error = {
                code: "grievancemechanismrightsnormsscore-input", text: "The way in which the company addresses a grievance is aligned with international human rights norms, when relevant"
            };
            $scope.validateMessage.push(error);
        }
        
        if($scope.GesUngpAssessmentFormViewModel.GrievanceMechanismFilingGrievanceScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.GrievanceMechanismFilingGrievanceScoreValue === null){
            result = true;
            var error = {
                code: "grievancemechanismfilinggrievancescore-input", text: "The people filing grievance are kept informed of how the grievance is being addressed. External expert is available to assist in severe grievances..."
            };
            $scope.validateMessage.push(error);
        }
        
        if($scope.GesUngpAssessmentFormViewModel.GrievanceMechanismReoccurringGrievancesScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.GrievanceMechanismReoccurringGrievancesScoreValue === null){
            result = true;
            var error = {
                code: "grievancemechanismreoccurringgrievancesscore-input", text: "Reoccurring grievances on similar matters are identified and informs the company’s priorities and preventive measures to avoid future harm"
            };
            $scope.validateMessage.push(error);
        }
        if($scope.GesUngpAssessmentFormViewModel.GrievanceMechanismFormatAndProcesseScoreValue === undefined || $scope.GesUngpAssessmentFormViewModel.GrievanceMechanismFormatAndProcesseScoreValue === null){
            result = true;
            var error = {
                code: "grievancemechanismformatandprocessescore-input", text: "The format and processes related to the grievance mechanism is developed based on engagement with people affected or potentially affected by the company’s operations"
            };
            $scope.validateMessage.push(error);
        }

        if($scope.GesUngpAssessmentFormViewModel.GesCommentCompanyPreparedness === undefined || $scope.GesUngpAssessmentFormViewModel.GesCommentCompanyPreparedness === null){
            result = true;
            var error = {
                code: "gescommentcompanypreparedness-input", text: "Company preparedness - GES general comment"
            };
            $scope.validateMessage.push(error);
        }

        return result;
    }

    function updateErrorList(code, value) {

        if ($scope.validateMessage != null && $scope.isFormInvalid) {
            for (var i = 0; i < $scope.validateMessage.length; i++) {
                if ($scope.validateMessage[i].code === code && value != null) {
                    $scope.validateMessage.splice(i, 1);
                }
            }
        }

        if ((value != null || value !== undefined) && $scope.validateMessage.length === 0) {
            $scope.isFormInvalid = false;
        }

    }
    
    $scope.addSource = function () {
        var source = {
            GesUNGPAssessmentFormResourcesId: "", GesUNGPAssessmentFormId: "", SourcesName: "", SourcesLink: "", SourceDate: "", Modified: "", Created: "", ModifiedBy: ""
        };
        $scope.editingSources.push(source);
    };
    $scope.deleteSource = function (index) {
        $scope.editingSources.splice(index, 1);
    };

    function saveUngpCallback() {
        GetGesUngpAssessmentData($scope.caseProfileId);
        $scope.validateMessage = [];
        $scope.isFormInvalid = false;
        $scope.isUngpPublishable = false;
    }   
    

    //Calculate UNGP points
    $scope.updateTotalOfHumanRightsScore = function () {

        CalculateTotalOfHumanRightsScores();
    };
    function CalculateTotalOfHumanRightsScores() {
        $scope.salientHumanRightsPotentialViolationTotalScore = 0;
        var i;

        if ($scope.scaleExtentOfHarmScore != null) 
        for (i = 0; i < $scope.scaleExtentOfHarmScore.length; i++) {
            if ($scope.GesUngpAssessmentFormViewModel.TheExtentOfHarmesScoreId === $scope.scaleExtentOfHarmScore[i].GesUngpAssessmentScoresId) {
                $scope.salientHumanRightsPotentialViolationTotalScore += $scope.scaleExtentOfHarmScore[i].Score;
                break;
            }
        }

        if ($scope.scaleNumberOfPeopleAffectedScore != null) 
        for (i = 0; i < $scope.scaleNumberOfPeopleAffectedScore.length; i++) {
            if ($scope.GesUngpAssessmentFormViewModel.TheNumberOfPeopleAffectedScoreId === $scope.scaleNumberOfPeopleAffectedScore[i].GesUngpAssessmentScoresId) {
                $scope.salientHumanRightsPotentialViolationTotalScore += $scope.scaleNumberOfPeopleAffectedScore[i].Score;
                break;
            }
        }

        if ($scope.systematicOverServeralYearsScore != null) 
        for (i = 0; i < $scope.systematicOverServeralYearsScore.length; i++) {
            if ($scope.GesUngpAssessmentFormViewModel.OverSeveralYearsScoreId === $scope.systematicOverServeralYearsScore[i].GesUngpAssessmentScoresId) {
                $scope.salientHumanRightsPotentialViolationTotalScore += $scope.systematicOverServeralYearsScore[i].Score;
                break;
            }
        }

        if ($scope.systematicOverServeralLocationScore != null) 
        for (i = 0; i < $scope.systematicOverServeralLocationScore.length; i++) {
            if ($scope.GesUngpAssessmentFormViewModel.SeveralLocationsScoreId === $scope.systematicOverServeralLocationScore[i].GesUngpAssessmentScoresId) {
                $scope.salientHumanRightsPotentialViolationTotalScore += $scope.systematicOverServeralLocationScore[i].Score;
                break;
            }
        }
        
        if ($scope.ongoingViolationOccurringScore != null) 
        for (i = 0; i < $scope.ongoingViolationOccurringScore.length; i++) {
            if ($scope.GesUngpAssessmentFormViewModel.IsViolationScoreId === $scope.ongoingViolationOccurringScore[i].GesUngpAssessmentScoresId) {
                $scope.salientHumanRightsPotentialViolationTotalScore += $scope.ongoingViolationOccurringScore[i].Score;
                break;
            }
        }
        
        if ($scope.confirmedViolationOfInternationalNormsScore != null) 
        for (i = 0; i < $scope.confirmedViolationOfInternationalNormsScore.length; i++) {
            if ($scope.GesUngpAssessmentFormViewModel.GesConfirmedViolationScoreId === $scope.confirmedViolationOfInternationalNormsScore[i].GesUngpAssessmentScoresId) {
                $scope.salientHumanRightsPotentialViolationTotalScore += $scope.confirmedViolationOfInternationalNormsScore[i].Score;
                break;
            }
        }

        $scope.GesUngpAssessmentFormViewModel.SalientHumanRightsPotentialViolationTotalScore = $scope.salientHumanRightsPotentialViolationTotalScore;
    }

    $scope.updateTotalScoreForHumanRightsPolicy = function () {

        calculateTotalScoreForHumanRightsPolicy();
    };

    function calculateTotalScoreForHumanRightsPolicy() {
        $scope.totalScoreForHumanRightsPolicy = 0;
        var i;

        if ($scope.GesUngpAssessmentFormViewModel == null) {
            return;
        }

        if (($scope.humanRightsPubliclyDisclosedAdditionalScore != null)) {
            for (i = 0; i < $scope.humanRightsPubliclyDisclosedAdditionalScore.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.HumanRightsPolicyPubliclyDisclosedAddScoreId === $scope.humanRightsPubliclyDisclosedAdditionalScore[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForHumanRightsPolicy += $scope.humanRightsPubliclyDisclosedAdditionalScore[i].Score;
                    break;
                }
            }
        }

        if (($scope.humanRightsPubliclyDisclosedCommunicatedScore != null)) {
            for (i = 0; i < $scope.humanRightsPubliclyDisclosedCommunicatedScore.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.HumanRightsPolicyCommunicatedScoreId === $scope.humanRightsPubliclyDisclosedCommunicatedScore[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForHumanRightsPolicy += $scope.humanRightsPubliclyDisclosedCommunicatedScore[i].Score;
                    break;
                }
            }
        }

        if (($scope.humanRightsPubliclyDisclosedExpectationsPersonnelScore != null)) {
            for (i = 0; i < $scope.humanRightsPubliclyDisclosedExpectationsPersonnelScore.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.HumanRightsPolicyStipulatesScoreId === $scope.humanRightsPubliclyDisclosedExpectationsPersonnelScore[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForHumanRightsPolicy += $scope.humanRightsPubliclyDisclosedExpectationsPersonnelScore[i].Score;
                    break;
                }
            }
        }

        if (($scope.humanRightsPubliclyDisclosedExpectationsPolicyApproved != null)) {
            for (i = 0; i < $scope.humanRightsPubliclyDisclosedExpectationsPolicyApproved.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.HumanRightsPolicyApprovedScoreId === $scope.humanRightsPubliclyDisclosedExpectationsPolicyApproved[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForHumanRightsPolicy += $scope.humanRightsPubliclyDisclosedExpectationsPolicyApproved[i].Score;
                    break;
                }
            }
        }

        $scope.GesUngpAssessmentFormViewModel.HumanRightsPolicyTotalScore = $scope.totalScoreForHumanRightsPolicy;
        calculateTotalScoreForCompanyPreparedness();
    }

    $scope.updateTotalScoreForHumanRightsDueDiligence = function () {

        calculateTotalScoreForHumanRightsDueDiligence();
    };

    function calculateTotalScoreForHumanRightsDueDiligence() {
        $scope.totalScoreForHumanRightsDueDiligence = 0;
        var i;

        if ($scope.GesUngpAssessmentFormViewModel == null) {
            return;
        }

        //Governance

        if (($scope.humanRightsGovernanceCommitment != null)) {
            for (i = 0; i < $scope.humanRightsGovernanceCommitment.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.GovernanceCommitmentScoreId === $scope.humanRightsGovernanceCommitment[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForHumanRightsDueDiligence += $scope.humanRightsGovernanceCommitment[i].Score;
                    break;
                }
            }
        }

        if (($scope.humanRightsGovernanceProvidesExamples != null)) {
            for (i = 0; i < $scope.humanRightsGovernanceProvidesExamples.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.GovernanceExamplesScoreId === $scope.humanRightsGovernanceProvidesExamples[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForHumanRightsDueDiligence += $scope.humanRightsGovernanceProvidesExamples[i].Score;
                    break;
                }
            }
        }

        if (($scope.humanRightsGovernanceClearDivision != null)) {
            for (i = 0; i < $scope.humanRightsGovernanceClearDivision.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.GovernanceClearDivisionScoreId === $scope.humanRightsGovernanceClearDivision[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForHumanRightsDueDiligence += $scope.humanRightsGovernanceClearDivision[i].Score;
                    break;
                }
            }
        }
        //

        //Business partners
        if (($scope.humanRightsBusinessPartnersAdditionalScore != null)) {
            for (i = 0; i < $scope.humanRightsBusinessPartnersAdditionalScore.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.BusinessPartnersAddScoreId === $scope.humanRightsBusinessPartnersAdditionalScore[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForHumanRightsDueDiligence += $scope.humanRightsBusinessPartnersAdditionalScore[i].Score;
                    break;
                }
            }
        }

        //Identification and commitment
        if (($scope.humanRightsIdentificationCommitment != null)) {
            for (i = 0; i < $scope.humanRightsIdentificationCommitment.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.IdentificationAndCommitmentScoreId === $scope.humanRightsIdentificationCommitment[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForHumanRightsDueDiligence += $scope.humanRightsIdentificationCommitment[i].Score;
                    break;
                }
            }
        }


        //Stakeholder engagement       
        if ($scope.humanRightsStakeholderEngagementAdditionalScore != null) {
            for (i = 0; i < $scope.humanRightsStakeholderEngagementAdditionalScore.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.StakeholderEngagementAddScoreId === $scope.humanRightsStakeholderEngagementAdditionalScore[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForHumanRightsDueDiligence += $scope.humanRightsStakeholderEngagementAdditionalScore[i].Score;
                    break;
                }
            }
        }

        //Human rights training        
        if ($scope.humanRightsTrainningAdditionalScore != null) {
            for (i = 0; i < $scope.humanRightsTrainningAdditionalScore.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.HumanRightsTrainingScoreId === $scope.humanRightsTrainningAdditionalScore[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForHumanRightsDueDiligence += $scope.humanRightsTrainningAdditionalScore[i].Score;
                    break;
                }
            }
        }

        $scope.GesUngpAssessmentFormViewModel.TotalScoreForHumanRightsDueDiligence = $scope.totalScoreForHumanRightsDueDiligence;
        calculateTotalScoreForCompanyPreparedness();
    }

    $scope.updateTotalScoreForRemediationOfAdverseHumanRightsImpacts = function () {

        calculateTotalScoreForRemediationOfAdverseHumanRightsImpacts();
    };

    function calculateTotalScoreForRemediationOfAdverseHumanRightsImpacts() {
        $scope.totalScoreForRemediationOfAdverseHumanRightsImpacts = 0;

        if ($scope.GesUngpAssessmentFormViewModel == null) {
            return;
        }

        var i;
        //Remedy process in place
        if ($scope.remediationAdverseHumanRightsImpactsRemedyProcess != null) {
            for (i = 0; i < $scope.remediationAdverseHumanRightsImpactsRemedyProcess.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.RemedyProcessInPlaceScoreId === $scope.remediationAdverseHumanRightsImpactsRemedyProcess[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForRemediationOfAdverseHumanRightsImpacts += $scope.remediationAdverseHumanRightsImpactsRemedyProcess[i].Score;
                    break;
                }
            }
        }


        //Grievance mechanism
        if ($scope.remediationAdverseHumanRightsImpactsGrievancMechanismOperationalLevel != null) {
            for (i = 0; i < $scope.remediationAdverseHumanRightsImpactsRemedyProcess.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.GrievanceMechanismHasOperationalLevelScoreId === $scope.remediationAdverseHumanRightsImpactsGrievancMechanismOperationalLevel[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForRemediationOfAdverseHumanRightsImpacts += $scope.remediationAdverseHumanRightsImpactsGrievancMechanismOperationalLevel[i].Score;
                    break;
                }
            }
        }

        if ($scope.remediationAdverseHumanRightsImpactsGrievancMechanismExistenceOperationalLevel != null) {
            for (i = 0; i < $scope.remediationAdverseHumanRightsImpactsGrievancMechanismExistenceOperationalLevel.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.GrievanceMechanismExistenceOfOperationalLevelScoreId === $scope.remediationAdverseHumanRightsImpactsGrievancMechanismExistenceOperationalLevel[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForRemediationOfAdverseHumanRightsImpacts += $scope.remediationAdverseHumanRightsImpactsGrievancMechanismExistenceOperationalLevel[i].Score;
                    break;
                }
            }
        }

        if ($scope.remediationAdverseHumanRightsImpactsGrievancMechanismDisclosesClearProcess != null) {
            for (i = 0; i < $scope.remediationAdverseHumanRightsImpactsGrievancMechanismDisclosesClearProcess.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.GrievanceMechanismClearProcessScoreId === $scope.remediationAdverseHumanRightsImpactsGrievancMechanismDisclosesClearProcess[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForRemediationOfAdverseHumanRightsImpacts += $scope.remediationAdverseHumanRightsImpactsGrievancMechanismDisclosesClearProcess[i].Score;
                    break;
                }
            }
        }

        if ($scope.remediationAdverseHumanRightsImpactsGrievancMechanismAddressesGrievance != null) {
            for (i = 0; i < $scope.remediationAdverseHumanRightsImpactsGrievancMechanismAddressesGrievance.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.GrievanceMechanismRightsNormsScoreId === $scope.remediationAdverseHumanRightsImpactsGrievancMechanismAddressesGrievance[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForRemediationOfAdverseHumanRightsImpacts += $scope.remediationAdverseHumanRightsImpactsGrievancMechanismAddressesGrievance[i].Score;
                    break;
                }
            }
        }

        if ($scope.remediationAdverseHumanRightsImpactsGrievancMechanismFilingGrievance != null) {
            for (i = 0; i < $scope.remediationAdverseHumanRightsImpactsGrievancMechanismFilingGrievance.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.GrievanceMechanismFilingGrievanceScoreId === $scope.remediationAdverseHumanRightsImpactsGrievancMechanismFilingGrievance[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForRemediationOfAdverseHumanRightsImpacts += $scope.remediationAdverseHumanRightsImpactsGrievancMechanismFilingGrievance[i].Score;
                    break;
                }
            }
        }

        if ($scope.remediationAdverseHumanRightsImpactsGrievancMechanismReoccurringGrievances != null) {
            for (i = 0; i < $scope.remediationAdverseHumanRightsImpactsGrievancMechanismReoccurringGrievances.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.GrievanceMechanismReoccurringGrievancesScoreId === $scope.remediationAdverseHumanRightsImpactsGrievancMechanismReoccurringGrievances[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForRemediationOfAdverseHumanRightsImpacts += $scope.remediationAdverseHumanRightsImpactsGrievancMechanismReoccurringGrievances[i].Score;
                    break;
                }
            }
        }

        if ($scope.remediationAdverseHumanRightsImpactsGrievancMechanismFormatProcesses != null) {
            for (i = 0; i < $scope.remediationAdverseHumanRightsImpactsGrievancMechanismFormatProcesses.length; i++) {
                if ($scope.GesUngpAssessmentFormViewModel.GrievanceMechanismFormatAndProcesseScoreId === $scope.remediationAdverseHumanRightsImpactsGrievancMechanismFormatProcesses[i].GesUngpAssessmentScoresId) {
                    $scope.totalScoreForRemediationOfAdverseHumanRightsImpacts += $scope.remediationAdverseHumanRightsImpactsGrievancMechanismFormatProcesses[i].Score;
                    break;
                }
            }
        }

        $scope.GesUngpAssessmentFormViewModel.TotalScoreForRemediationOfAdverseHumanRightsImpacts = $scope.totalScoreForRemediationOfAdverseHumanRightsImpacts;
        calculateTotalScoreForCompanyPreparedness();
    }

    function calculateTotalScoreForCompanyPreparedness() {
        $scope.totalScoreForCompanyPreparedness = $scope.totalScoreForRemediationOfAdverseHumanRightsImpacts + $scope.totalScoreForHumanRightsDueDiligence + $scope.totalScoreForHumanRightsPolicy;
        $scope.GesUngpAssessmentFormViewModel.TotalScoreForCompanyPreparedness = $scope.totalScoreForCompanyPreparedness;
    }
    //End of Calculate UNGP points

    $scope.convertDate = function (value, format) {
        if (value != null && !value.isNullOrEmpty) {
            return $filter("date")(new Date(parseInt(value.substr(6))), format);
        }

        return null;
    };

    String.isNullOrEmpty = function (value) {
        return !(typeof value === "undefined" || (typeof value === "string" && value.length > 0));
    };
    
}]);