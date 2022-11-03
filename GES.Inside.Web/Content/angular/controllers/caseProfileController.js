"use strict";
GesInsideApp.controller("CaseProfileController", ["$scope", "$filter", "$timeout", "$q", "$window", "CaseProfileService", "NgTableParams", function ($scope, $filter, $timeout, $q, $window, CaseProfileService, NgTableParams) {
    $scope.isFormValid = false;
    $scope.message = null;
    $scope.caseProfile = null;
    $scope.caseProfileInvisibleEntities = [];
    $scope.countries = null;
    $scope.milestones = null;
    $scope.recommendations = null;
    $scope.issueheadings = null;
    $scope.issueHeadingId = null;
    $scope.PreviousIssueHeadingId = null;
    $scope.conclusions = null;
    $scope.normAreas = null;
    $scope.users = null;
    $scope.currentUser = null;
    $scope.responseStatuses = null;
    $scope.progressStatuses = null;
    $scope.documents = null;
    $scope.documentServices = null;
    $scope.conventions = null;
    $scope.editingConventions = [];
    $scope.norms = null;
    //$scope.editingNorms = [];
    //$scope.editingDocument = null;
    $scope.isNewCaseProfile = false;
    
    $scope.isSaving = false; 
    $scope.editingEngagementIdTemp = null;
    $scope.allowShow = false;    
    $scope.savingAndCloseAction = false;

    $scope.editingNews = null;
    
    $scope.validateMessage = [];    
    $scope.isFormInvalid = false;  
    
    $scope.overallStatus = 'All unstarted';

    $scope.edited = false;

    $scope.isTemplateLoaded =  false;

    $scope.isHideCompanyinfo =  false;
    $scope.isHideEntrydate =  false;
    $scope.isHideNorm =  false;
    $scope.isHideLocation =  false;
    $scope.isHideRecommendation =  false;
    $scope.isHideCaseInformationConclusion =  false;
    $scope.isHideEngagementtype =  false;
    $scope.isHideResponse =  false;
    $scope.isHideProgress =  false;
    $scope.isHideDevelopment =  false;
    $scope.isHideGEScontactinformation =  false;
    $scope.isHideSDGs =  false;
    $scope.isHideGuidelineslist =  false;
    $scope.isHideConventions =  false;
    $scope.isHideKpis =  false;
    $scope.isHideSummary =  false;
    $scope.isHideGuidelines =  false;
    $scope.isHideFulldescription =  false;
    $scope.isHideDescriptionnew =  false;
    $scope.isHideConclusion =  false;
    $scope.isHideCompanydialogue =  false;
    $scope.isHideCompanydialoguenew =  false;
    $scope.isHideCompanydialoguelog =  false;
    $scope.isHideSourcedialogue =  false;
    $scope.isHideSourcedialoguenew =  false;
    $scope.isHideSourcedialoguelog =  false;
    $scope.isHideGEScommentary = false;
    $scope.isHideGSSLink = false;
    $scope.isHideNews =  false;
    $scope.isHideChangeobjective =  false;
    $scope.isHideMilestone =  false;
    $scope.isHideNextStep =  false;
    $scope.isHideDiscussionpoints =  false;
    $scope.isHideOtherstakeholders =  false;
    $scope.isHideAssociatedCorporations =  false;
    $scope.isHideSources =  false;
    $scope.isHideReferences =  false;
    $scope.isHideSupplementaryReading =  false;
    $scope.isHide360IncidentAnalysisSummary =  false;
    $scope.isHide360IncidentAnalysisDialogueAndAnalysis =  false;
    $scope.isHide360IncidentAnalysisConclusion =  false;
    $scope.isHide360IncidentAnalysisGuidelinesandconventions =  false;
    $scope.isHide360IncidentAnalysisSources =  false;
    $scope.isHide360ClosingIncidentAnalysisSummary =  false;
    $scope.isHide360ClosingIncidentAnalysisDialogueAndAnalysis =  false;
    $scope.isHide360ClosingIncidentAnalysisConclusion =  false;
    $scope.isHideUNGPassessmentform =  false;
    $scope.isHideAdditionalDocument =  false;
    $scope.isHideInternalComments =  false;


    $scope.isCompanyinfoDisabled =  false;
    $scope.isEntrydateDisabled =  false;
    $scope.isNormDisabled =  false;
    $scope.isLocationDisabled =  false;
    $scope.isRecommendationDisabled =  false;
    $scope.isCaseInformationConclusionDisabled =  false;
    $scope.isEngagementtypeDisabled =  false;
    $scope.isResponseDisabled =  false;
    $scope.isProgressDisabled =  false;
    $scope.isDevelopmentDisabled =  false;
    $scope.isGEScontactinformationDisabled =  false;
    $scope.isSDGsDisabled =  false;
    $scope.isGuidelineslistDisabled =  false;
    $scope.isConventionsDisabled =  false;
    $scope.isKpisDisabled =  false;
    $scope.isSummaryDisabled =  false;
    $scope.isGuidelinesDisabled =  false;
    $scope.isFulldescriptionDisabled =  false;
    $scope.isDescriptionnewDisabled =  false;
    $scope.isConclusionDisabled =  false;
    $scope.isCompanydialogueDisabled =  false;
    $scope.isCompanydialoguenewDisabled =  false;
    $scope.isCompanydialoguelogDisabled =  false;
    $scope.isSourcedialogueDisabled =  false;
    $scope.isSourcedialoguenewDisabled =  false;
    $scope.isSourcedialoguelogDisabled =  false;
    $scope.isGEScommentaryDisabled = false;
    $scope.isGSSLinkDisabled = false;
    $scope.isNewsDisabled =  false;
    $scope.isChangeobjectiveDisabled =  false;
    $scope.isMilestoneDisabled =  false;
    $scope.isNextStepDisabled =  false;
    $scope.isDiscussionpointsDisabled =  false;
    $scope.isOtherstakeholdersDisabled =  false;
    $scope.isAssociatedCorporationsDisabled =  false;
    $scope.isSourcesDisabled =  false;
    $scope.isReferencesDisabled =  false;
    $scope.isSupplementaryReadingDisabled =  false;
    $scope.is360IncidentAnalysisSummaryDisabled =  false;
    $scope.is360IncidentAnalysisDialogueAndAnalysisDisabled =  false;
    $scope.is360IncidentAnalysisConclusionDisabled =  false;
    $scope.is360IncidentAnalysisGuidelinesandconventionsDisabled =  false;
    $scope.is360IncidentAnalysisSourcesDisabled =  false;
    $scope.is360ClosingIncidentAnalysisSummaryDisabled =  false;
    $scope.is360ClosingIncidentAnalysisDialogueAndAnalysisDisabled =  false;
    $scope.is360ClosingIncidentAnalysisConclusionDisabled =  false;
    $scope.isUNGPassessmentformDisabled =  false;
    $scope.isAdditionalDocumentDisabled =  false;
    $scope.isInternalCommentsDisabled =  false;


    $scope.milestones = [
        { value: 1, name: "Milestone 1" },
        { value: 2, name: "Milestone 2" },
        { value: 3, name: "Milestone 3" },
        { value: 4, name: "Milestone 4" },
        { value: 5, name: "Milestone 5" }
    ];

    $scope.calculatedDevelopmentGrade = null;

    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();     
    
    $scope.engagementTypes = null;
    
    init();    

    $scope.GetUserById = function () {
        var userId = $scope.caseProfile.AnalystG_Users_Id || 0;
        if (userId === 0)
            return;

        CaseProfileService.GetUserById(userId).then(
            function (response) {
                $scope.currentUser = response.data;
                fetchUserAvatar("userImageUrl", $scope.currentUser.RetrievedImageUrl);
                fetchUserAvatar("userImageUrlLower", $scope.currentUser.RetrievedImageUrlLower);
            },
            function (reason) {
                quickNotification("Error occurred during loading User, caused: " + reason, "error");
            }
        );
    };

    $scope.addNewFile = function (file) {
        if (file) {
            $scope.editingDocument = { G_ManagedDocuments_Id: 0, DownloadUrl: "", FileName: file.name };
            $("#document-dialog").modal("show");
            resetSelectInDocumentDialog();
        }
    };

    //$scope.changeFile = function (file) {
    //    if (file) {
    //        $scope.editingDocument.DownloadUrl = "";
    //        $scope.editingDocument.FileName = file.name;
    //    }
    //};

    //$scope.getDocumentById = function (documentId) {
    //    CaseProfileService.GetUploadedDocumentById(documentId).then(
    //        function (response) {
    //            $scope.editingDocument = response.data;
    //            resetSelectInDocumentDialog();
    //        },
    //        function (reason) {
    //            quickNotification("Error occurred during loading document, caused: " + reason, "error");
    //        }
    //    );
    //};

    $scope.getKPIs = function () {

        if (String($scope.editingEngagementIdTemp) !== $scope.caseProfile.I_Engagement_Type_Id) {
            if ($scope.edited && $scope.editingKpis.length !== 0 && confirm('Are you sure do you want to change value of Engagement type? The old KPIs of the report will be deleted when saving')) {
                $scope.editingKpisTemp = $scope.editingKpis;
                $scope.editingKpis = [];
            }
        } else if ($scope.edited) {
            $scope.editingKpis = $scope.editingKpisTemp;
        }

        if ($scope.caseProfile.I_Engagement_Type_Id !== undefined) {
            $scope.GetKpisByEngagementTypeId($scope.caseProfile.I_Engagement_Type_Id);
            $scope.isNotAllowAddKpi = false;
        } else {
            $scope.isNotAllowAddKpi = true;
            $scope.Kpis = [];
            $scope.editingKpisTemp = $scope.editingKpis;
            $scope.editingKpis = [];
        }

        $scope.allowShow = $scope.caseProfile.I_Engagement_Type_Id === "9"; //EmerginMarket = 9

    };

    $scope.GetKpisByEngagementTypeId = function (engagementTypeId) {

        if (engagementTypeId === 0 || engagementTypeId === null)
            return;

        CaseProfileService.GetKpisByEngagementTypeId(engagementTypeId).then(
            function (response) {
                $scope.Kpis = response.data;
                var i;
                if ($scope.Kpis.length > 0) {
                    for (i = 0; i < $scope.Kpis.length; i++) {
                        $scope.Kpis[i].Created =
                            $scope.convertDate($scope.Kpis[i].Created, 'yyyy/MM/dd HH:mm:ss a');
                    }
                }
            },
            function (reason) {
                quickNotification("Error occurred during loading Kpi base on engagement, caused: " + reason, "error");
            }
        );
    };

    $scope.GetKpiPerformances = function () {

        CaseProfileService.GetKpiPerformances().then(
            function (response) {
                $scope.KpiPerformances = response.data;
                var i;
                if ($scope.KpiPerformances.length > 0) {
                    for (i = 0; i < $scope.KpiPerformances.length; i++) {
                        $scope.KpiPerformances[i].Created =
                            $scope.convertDate($scope.KpiPerformances[i].Created, 'yyyy/MM/dd HH:mm:ss a');
                        $scope.KpiPerformances[i].image = '<i class="fa fa-gear"></i>';
                    }
                }
            },
            function (reason) {
                quickNotification("Error occurred during loading Kpi performance, caused: " + reason, "error");
            }
        );
    };   

    function getKPIs()
    {

        if (String($scope.editingEngagementIdTemp) !== $scope.caseProfile.I_Engagement_Type_Id) {
            if ($scope.edited && $scope.editingKpis!=null && $scope.editingKpis.length !== 0 && confirm('Are you sure do you want to change value of Engagement type? The old KPIs of the report will be deleted when saving')) {
                $scope.editingKpisTemp = $scope.editingKpis;
                $scope.editingKpis = [];
            }
        } else if ($scope.edited) {
            $scope.editingKpis = $scope.editingKpisTemp;
        }

        if ($scope.caseProfile.I_Engagement_Type_Id !== undefined) {
            $scope.GetKpisByEngagementTypeId($scope.caseProfile.I_Engagement_Type_Id);
            $scope.isNotAllowAddKpi = false;
        } else {
            $scope.isNotAllowAddKpi = true;
            $scope.Kpis = [];
            $scope.editingKpisTemp = $scope.editingKpis;
            $scope.editingKpis = [];
        }

        $scope.allowShow = $scope.caseProfile.I_Engagement_Type_Id === "9"; //EmerginMarket = 9

    };

    //End Kpis  

    function resetSelectInDocumentDialog() {
        $timeout(function () {
            $("#taxonomy-select").val($scope.editingDocument.G_DocumentManagementTaxonomies_Id).trigger("change");
            $("#document-service-select").val($scope.editingDocument.G_ManagedDocumentServices_Id).trigger("change");
        });
    }

    function getCompanyId() {
        return $scope.isNewCaseProfile ? CaseProfileService.getParameterByName("companyId") : $scope.caseProfile.I_Companies_Id;
    }

    function goToCaseProfileList() {
        $window.location.href = "/CaseProfile/List?companyId=" + getCompanyId();
    }

    function reloadPage(caseProfileId) {
        if ($scope.savingAndCloseAction) {
            goToCaseProfileList();
        } else{
            $window.location.href = "/CaseProfile/Edit/" + caseProfileId;
        }        
    }  

    $scope.saveManagedDocument = function () {
        $scope.editingDocument.I_Companies_Id = getCompanyId();
        CaseProfileService.SaveManagedDocument($scope.editingDocument, $scope.file, loadDoucments);
    };

    $scope.getDevelopmentGradeText = function () {
        var progressStatusId = $scope.caseProfile.I_ProgressStatuses_Id || 0;
        var responseStatusId = $scope.caseProfile.I_ResponseStatuses_Id || 0;
        CaseProfileService.GetDevelopmentGradeText(progressStatusId, responseStatusId).then(
            function (response) {
                $scope.calculatedDevelopmentGrade = response.data;
            },
            function (reason) {
                quickNotification("Error occurred during loading Development Grade text, caused: " + reason, "error");
            }
        );
    };

    $scope.deleteDocument = function (target) {
        var documentId = Number(target.attr("data-id"));
        CaseProfileService.DeleteManagedDocument(documentId, loadDoucments);
    };  

    $scope.updateCaseProfileData = function (action) {
        
        if (isFormInvalid()){
            $scope.isFormInvalid = true;
            return;
        } 
        
        $scope.isSaving = true;

        var a = $scope.caseProfile.milestones;

        $scope.caseProfile.ConventionsViewModels = $scope.editingConventions;        
                
        $scope.savingAndCloseAction = action === "save_close";           
            
        CaseProfileService.UpdateCaseProfileData($scope.caseProfile).then(function (d) {

                quickNotification("Saved successfully");
                reloadPage(d.data.caseProfileId);
            },
            function (failReason) {
                quickNotification("Error occurred during save data: " + failReason, "error");
            });               
    };

    $scope.saveManagedDocument = function () {
        $scope.editingDocument.I_Companies_Id = getCompanyId();
        CaseProfileService.SaveManagedDocument($scope.editingDocument, $scope.file, loadDoucments);
    };
        

    function isFormInvalid() {
        var result = false;
        var errorMessage = "";
        $scope.validateMessage = [];

        if($scope.caseProfile.ReportIncident === undefined || $scope.caseProfile.ReportIncident === null){
            result = true;
            var error = {
                code: "reportIncident-input", text: "Name of case report should not be empty value"
            };
            $scope.validateMessage.push(error);

            $timeout(function () {
                var input = $("#reportIncident-input").find(".inputText")[0];
                input.focus();
                input.blur();
            });
            
        }
        
        if($scope.caseProfile.I_Engagement_Type_Id === undefined || $scope.caseProfile.I_Engagement_Type_Id === null){
            result = true;
            var error = {
                code: "engagement-type-select", text: "Engagement type should not be empty value"
            };
            $scope.validateMessage.push(error);
        }

        //if ($scope.caseProfile.CountryId === undefined || $scope.caseProfile.CountryId === null){
        //    result = true;
        //    var error = {
        //        code: "country-select", text: "Location should not be empty value"
        //    };
        //    $scope.validateMessage.push(error);
        //}

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

    $scope.validateUpdate = function (code,text){
        updateErrorList(code,text);
    };

    $scope.engagementTypesUpdate = function (code,text){
        updateErrorList(code,text);
        getKPIs();
        $scope.updateTemplate();
    };

    $scope.$watch(function(){
        return $scope.caseProfile != null && $scope.caseProfile.ReportIncident != null;
    }, function(newvalue, oldvalue){
        if(oldvalue != newvalue){
            updateErrorList('reportIncident-input',newvalue);
        }
        
    },true);
       
    
    function keyId() {
        var number = Math.random();
        number.toString(36);
        return number.toString(36).substr(2, 9);
    }
    
    function initCancelSaveConfirmationBox() {
        $("#cancel-save").confirmModal({
            confirmCallback: goToCaseProfileList
        });

        $("#cancel-save-bottom").confirmModal({
            confirmCallback: goToCaseProfileList
        });
    }

    function init() {

        initSelect2();
        
        $('.button-bottom').fadeOut();
        $(document).scroll(function () {
            var y = $(this).scrollTop();
            if (y > 600) {
                $('.button-bottom').fadeIn();
            } else {
                $('.button-bottom').fadeOut();
            }
        });        

        CaseProfileService.GetMilestoneTypes().then(
            function (d) {
                $scope.milestones = d.data;

                if ($scope.caseProfile != null && $scope.caseProfile.MileStoneModel != null) {
                    $timeout(function () {
                        $("#caseprofilemilestone-select").val($scope.caseProfile.MileStoneModel.GesMilestoneTypesId).trigger("change");
                    }, 6000); //Add timeout to fix initialize value in minified code 
                }

            },
            function (reason) {
                quickNotification("Error occurred during loading Milestones, caused: " + reason, "error");
            }
        );

        CaseProfileService.GetContactDirections().then(
            function (d) {
                $scope.contactDirections = d.data;
            },
            function (reason) {
                quickNotification("Error occurred during loading Contact Directions, caused: " + reason, "error");
            }
        );

        CaseProfileService.GetContactTypes().then(
            function (d) {
                $scope.contactTypes = d.data;
            },
            function (reason) {
                quickNotification("Error occurred during loading Contact Type, caused: " + reason, "error");
            }
        );

        CaseProfileService.GetDocumentManagementTaxonomies().then(
            function (d) {
                $scope.taxonomies = d.data;
            },
            function () {
                quickNotification("Error occurred during loading taxonomies", "error");
            }
        );

        CaseProfileService.GetManagedDocumentServices().then(
            function (d) {
                $scope.documentServices = d.data;
            },
            function () {
                quickNotification("Error occurred during loading documents", "error");
            }
        );
       

        var isAddNewCaseId = CaseProfileService.IsAddNewCaseProfile();
 
        $timeout(function () {
            $('[data-toggle="popover"]')
                .popover({
                    html: true,
                    animation: true
                });

            StartLoad()
                .then(GetAllCountries)
                .then(GetAllIssueHeadings)
                .then(GetAllRecommendations)
                // .then(GetAllConclusions)
                .then(GetAllEngagementTypes)
                .then(GetNormAreas)
                .then(GetProgressStatuses)
                .then(GetResponseStatuses)
                .then(GetNormsServices)
                .then(GetConventionsServices)
                .then(GetUsers)
                .then(GetCaseProfile)
                .then(GetCaseProfileInvisibleEntities)
                .then(CaseProfilePopulateData)
                .then(function () {
                        setUpContentBlockAnimation();
                        $(".animation-loading").hide();
                        quickNotification("Data loading completed", "success");
                    },
                    function (failReason) {
                        setUpContentBlockAnimation();
                        $(".animation-loading").hide();
                        quickNotification("Error occurred during loading data " + failReason, "error");
                    });
        });

        $scope.updateTemplate = function () {

            GetCaseProfileInvisibleEntities();

        };

        $scope.updateIssueHeading = function (event) {
            if ($scope.caseProfile.IssueHeadingId != null) {
                CaseProfileService.CheckExistHeading($scope.caseProfile.IssueHeadingId, $scope.caseProfile.I_GesCaseReports_Id, getCompanyId()).then(
                    function (d) {
                        var existCaseProfile = d.data;
                        if (existCaseProfile.length == 0) {
                            $scope.caseProfile.ReportIncident = $scope.GetHeadingName($scope.caseProfile.IssueHeadingId);
                        }
                        else {
                            quickNotification("Cannot using this issue heading ( " + $scope.GetHeadingName($scope.caseProfile.IssueHeadingId) +" ) because is used in another case", "error");
                            $scope.caseProfile.IssueHeadingId = $scope.PreviousIssueHeadingId;
                            if ($scope.isNewCaseProfile) {
                                $scope.caseProfile.ReportIncident = "";
                            }
                        }
                    },
                    function () {
                        quickNotification("Error occurred during checking issue heading", "error");
                    }
                );
            }
        };
    }         

    var StartLoad = function () {
        var deferred = $q.defer();
        deferred.resolve('Starting');
        return deferred.promise;
    };
    
    var GetAllCountries = function () {
        var deferred = $q.defer();

        CaseProfileService.GetAllCountries().then(
            function (d) {
                $scope.countries = d.data;
                deferred.resolve('Get all Countries');
            },
            function () {
                quickNotification("Error occurred during loading countries data", "error");
            }
        );
        
        return deferred.promise;
    };

    var GetNormsServices = function () {
        var deferred = $q.defer();
        
        CaseProfileService.GetNormsServices().then(
            function (d) {
                $scope.norms = d.data;
                deferred.resolve('Get Norms Services');
            },
            function () {
                quickNotification("Error occurred during loading norms", "error");
            }
        );
        return deferred.promise;
    };

    var GetConventionsServices = function () {
        var deferred = $q.defer();

        CaseProfileService.GetConventionsServices().then(
            function (d) {
                $scope.conventions = d.data;
                deferred.resolve('Get Conventions');
            },
            function () {
                quickNotification("Error occurred during loading conventions", "error");
            }
        );
        return deferred.promise;
    };

    var GetUsers = function () {
        var deferred = $q.defer();
        
        CaseProfileService.GetUsers().then(
            function (d) {
                $scope.users = d.data;
                deferred.resolve('GetUsers');
            },
            function () {
                quickNotification("Error occurred during loading Case profile data", "error");
            }
        );

        return deferred.promise;
    };

    var GetProgressStatuses = function () {
        var deferred = $q.defer();

        CaseProfileService.GetProgressStatuses().then(
            function (d) {
                $scope.progressStatuses = d.data;
                deferred.resolve('GetProgressStatuses');

            },
            function () {
                quickNotification("Error occurred during loading progress", "error");
            }
        );
        return deferred.promise;
    };

    var GetResponseStatuses = function () {
        var deferred = $q.defer();

        CaseProfileService.GetResponseStatuses().then(
            function (d) {
                $scope.responseStatuses = d.data;
                deferred.resolve('GetResponseStatuses');
            },
            function () {
                quickNotification("Error occurred during loading response data", "error");
            }
        );

        return deferred.promise;
    };

    var GetNormAreas = function () {
        var deferred = $q.defer();

        CaseProfileService.GetNormAreas().then(
            function (d) {
                $scope.normAreas = d.data;
                deferred.resolve('GetNormAreas');
            },
            function () {
                quickNotification("Error occurred during loading Norm data", "error");
            }
        );
        return deferred.promise;
    };

    var GetAllEngagementTypes = function () {
        var deferred = $q.defer();

        CaseProfileService.GetAllEngagementTypes().then(
            function (d) {
                $scope.engagementTypes = d.data;               
                deferred.resolve('GetAllEngagementTypes');               
                
            },
            function () {
                quickNotification("Error occurred during loading engagement type data", "error");
            }
        );
        return deferred.promise;
    };

    var GetAllRecommendations = function () {
        var deferred = $q.defer();

        CaseProfileService.GetAllRecommendations().then(
            function (d) {
                $scope.recommendations = d.data;                
                deferred.resolve('GetAllRecommendations');
            },
            function () {
                quickNotification("Error occurred during loading recommendations data", "error");
            }
        );
        return deferred.promise;
    };
    var GetAllIssueHeadings = function () {
        var deferred = $q.defer();

        CaseProfileService.GetAllIssueHeadings().then(
            function (d) {
                $scope.issueheadings = d.data;
                deferred.resolve('GetAllIssueHeadings');
            },
            function () {
                quickNotification("Error occurred during loading issue headings data", "error");
            }
        );
        return deferred.promise;
    };

    var GetAllConclusions = function () {
        var deferred = $q.defer();

        CaseProfileService.GetAllConclusions().then(
            function (d) {
                $scope.conclusions = d.data;
                deferred.resolve('GetAllConclusions');
            },
            function () {
                quickNotification("Error occurred during loading conclusions data", "error");
            }
        );
        return deferred.promise;
    };

    var GetCaseProfile = function () {
        var deferred = $q.defer();

        CaseProfileService.GetCaseProfile().then(
            function (d) {
                $scope.caseProfile = d.data;            
                CaseProfileService.caseProfile = $scope.caseProfile;
                $scope.$broadcast('finishInitCaseProfile');
                deferred.resolve('Get CaseProfile');
            },
            function () {
                quickNotification("Error occurred during loading users data", "error");
            }
        );

        return deferred.promise;
    };

    var GetCaseProfileInvisibleEntities = function () {

        var engagementTypeId = $scope.caseProfile.I_Engagement_Type_Id;
        var recomendationId = $scope.caseProfile.NewI_GesCaseReportStatuses_Id;
        
        if ($scope.isNewCaseProfile && !$scope.isTemplateLoaded) {
            engagementTypeId = CaseProfileService.getParameterByName("engagementTypeId");
            recomendationId = CaseProfileService.getParameterByName("recommendationId");
            $scope.isTemplateLoaded = true;
        } 
               
        
        if (engagementTypeId != null && recomendationId != null) {
            var deferred = $q.defer();
            CaseProfileService.GetCaseProfileInvisibleEntities(engagementTypeId, recomendationId).then(
                function (d) {
                    $scope.caseProfileInvisibleEntities = d.data;
                    deferred.resolve('Get GesCaseProfileInvisibleEntities');

                    if ($scope.caseProfileInvisibleEntities != null && $scope.caseProfileInvisibleEntities.length > 0) {
                        initEntites();
                    } else{
                        resetEntities();
                    }                  
                    
                },
                function () {
                    quickNotification("Error occurred during loading users data", "error");
                }
            );

            return deferred.promise;
        }        
    };

    function CaseProfilePopulateData() {       

        //TODO truong.pham Find a way to set default value in select is 0
        if ($scope.caseProfile.CountryId === '00000000-0000-0000-0000-000000000000')
            $scope.caseProfile.CountryId = null;
        $scope.GetUserById();
        $scope.getDevelopmentGradeText();
        loadDoucments();        
        $scope.editingEngagementIdTemp = $scope.caseProfile.I_Engagement_Type_Id;
        
        $scope.allowShow = $scope.caseProfile.I_Engagement_Type_Id === 9; //EmerginMarket = 9        

        if ($scope.caseProfile.IssueHeadingId != null) {
            $scope.PreviousIssueHeadingId = angular.copy($scope.caseProfile.IssueHeadingId)
        }

        initCancelSaveConfirmationBox();
        if (!$scope.isNewCaseProfile) {
            $timeout(function () {
                $("#response-status-select").val($scope.caseProfile.I_ResponseStatuses_Id).trigger("change");
                $("#progress-status-select").val($scope.caseProfile.I_ProgressStatuses_Id).trigger("change");
                //$("#milestone-select").val($scope.caseProfile.MileStone).trigger("change");
                $("#norm-select").val($scope.caseProfile.I_NormAreas_Id).trigger("change");
                $("#country-select").val($scope.caseProfile.CountryId).trigger("change");
                $("#recommendation-select").val($scope.caseProfile.NewI_GesCaseReportStatuses_Id).trigger("change");
                $("#conclusion-select").val($scope.caseProfile.I_GesCaseReportStatuses_Id).trigger("change");
                $("#engagement-type-select").val($scope.caseProfile.I_Engagement_Type_Id).trigger("change");
                $("#user-select").val($scope.caseProfile.AnalystG_Users_Id).trigger("change");

                if ($scope.caseProfile.MileStoneModel !== null) {
                    $("#caseprofilemilestone-select").val($scope.caseProfile.MileStoneModel.GesMilestoneTypesId).trigger("change");
                }

                $scope.caseProfileForm.$setPristine();

                $scope.edited = true;

                initEntites();

            }, 200); //Add timeout to fix initialize value in minified code  
        } else{

            $scope.caseProfile.NewI_GesCaseReportStatuses_Id = CaseProfileService.getParameterByName("recommendationId");
            $scope.caseProfile.I_Engagement_Type_Id = CaseProfileService.getParameterByName("engagementTypeId");

            $timeout(function () {

                $("#recommendation-select").val($scope.caseProfile.NewI_GesCaseReportStatuses_Id).trigger("change");
                $("#engagement-type-select").val($scope.caseProfile.I_Engagement_Type_Id).trigger("change");

                $scope.caseProfileForm.$setPristine();

            }, 200); //Add timeout to fix initialize value in minified code             
            
        }        

    }

    $scope.convertDate = function (value, format) {
        if (value != null && !value.isNullOrEmpty) {
            return $filter("date")(new Date(parseInt(value.substr(6))), format);
        }

        return null;
    };

    String.isNullOrEmpty = function (value) {
        return !(typeof value === "undefined" || (typeof value === "string" && value.length > 0));
    };
    
    function initEntites() {
        if ($scope.caseProfileInvisibleEntities != null){
            for (var i = 0; i < $scope.caseProfileInvisibleEntities.length; i++) {

                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'COMPANY-INFO')) {
                    $scope.isHideCompanyinfo = true;
                    if ($scope.caseProfile && $scope.caseProfile.caseProfile != null) {
                        $scope.isCompanyinfoDisabled = true;
                        $scope.isHideCompanyinfo = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'ENTRY-DATE')) {
                    $scope.isHideEntrydate = true;
                    if ($scope.caseProfile && $scope.caseProfile.EntryDate != null) {
                        $scope.isEntrydateDisabled = true;
                        $scope.isHideEntrydate = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'NORM')) {
                    $scope.isHideNorm = true;
                    if ($scope.caseProfile && $scope.caseProfile.I_NormAreas_Id != null) {
                        $scope.isNormDisabled = true;
                        $scope.isHideNorm = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'LOCATION')) {
                    $scope.isHideLocation = true;
                    if ($scope.caseProfile && $scope.caseProfile.CountryId != null) {
                        $scope.isLocationDisabled = true;
                        $scope.isHideLocation = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'RECOMMENDATION')) {
                    $scope.isHideRecommendation = true;
                    if ($scope.caseProfile && $scope.caseProfile.NewI_GesCaseReportStatuses_Id != null) {
                        $scope.isRecommendationDisabled = true;
                        $scope.isHideRecommendation = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'CASE-INFORMATION-CONCLUSION')) {
                    $scope.isHideCaseInformationConclusion = true;
                    if ($scope.caseProfile && $scope.caseProfile.I_GesCaseReportStatuses_Id != null) {
                        $scope.isCaseInformationConclusionDisabled = true;
                        $scope.isHideCaseInformationConclusion = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'ENGAGEMENT-TYPE')) {
                    $scope.isHideEngagementtype = true;
                    if ($scope.caseProfile && $scope.caseProfile.I_Engagement_Type_Id != null) {
                        $scope.isEngagementtypeDisabled = true;
                        $scope.isHideEngagementtype = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'RESPONSE')) {
                    $scope.isHideResponse = true;
                    if ($scope.caseProfile && $scope.caseProfile.I_ResponseStatuses_Id != null && $scope.caseProfile.I_ResponseStatuses_Id !== "") {
                        $scope.isResponseDisabled = true;
                        $scope.isHideResponse = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'PROGRESS')) {
                    $scope.isHideProgress = true;
                    if ($scope.caseProfile && $scope.caseProfile.I_ProgressStatuses_Id != null && $scope.caseProfile.I_ProgressStatuses_Id !== "") {
                        $scope.isProgressDisabled = true;
                        $scope.isHideProgress = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'GES-CONTACT-INFORMATION')) {
                    $scope.isHideGEScontactinformation = true;
                    if ($scope.caseProfile && $scope.caseProfile.AnalystG_Users_Id != null) {
                        $scope.isGEScontactinformationDisabled = true;
                        $scope.isHideGEScontactinformation = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'SDGS')) {

                    $scope.isHideSDGs = false;
                    $scope.isSDGsDisabled = true;
                    if (null === $scope.caseProfile.SdgIds || ($scope.caseProfile.SdgIds !== null && $scope.caseProfile.SdgIds.length === 0)) {
                        $scope.isHideSDGs = true;
                    }
                }

                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'GUIDELINES-LIST')) {

                    $scope.isHideGuidelineslist = false;
                    $scope.isGuidelineslistDisabled = true;
                    if (null === $scope.caseProfile.NormViewModels || ($scope.caseProfile.NormViewModels !== null && $scope.caseProfile.NormViewModels.length === 0)) {
                        $scope.isHideGuidelineslist = true;
                    }                    
                }
                
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'CONVENTIONS')) {

                    $scope.isHideConventions = false;
                    $scope.isConventionsDisabled = true;
                    if (null === $scope.caseProfile.ConventionsViewModels || ($scope.caseProfile.ConventionsViewModels !== null && $scope.caseProfile.ConventionsViewModels.length === 0)) {
                        $scope.isHideConventions = true;
                    }
                }
                
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'KPIS')) {

                    $scope.isHideKpis = false;
                    $scope.isKpisDisabled = true;
                    if (null === $scope.caseProfile.CaseReportKpiViewModels || ($scope.caseProfile.CaseReportKpiViewModels !== null && $scope.caseProfile.CaseReportKpiViewModels.length === 0)) {
                        $scope.isHideKpis = true;
                    }
                }
                
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'SUMMARY')) {
                    $scope.isHideSummary = true;
                    if ($scope.caseProfile && ($scope.caseProfile.Summary != null && $scope.caseProfile.Summary !== "")) {
                        $scope.isSummaryDisabled = true;
                        $scope.isHideSummary = false;
                    }
                }
                
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'GUIDELINES')) {
                    $scope.isHideGuidelines = true;
                    if ($scope.caseProfile && ($scope.caseProfile.Guidelines != null && $scope.caseProfile.Guidelines !== "" && $scope.caseProfile.Guidelines.toUpperCase() !== "NULL")) {
                        $scope.isGuidelinesDisabled = true;
                        $scope.isHideGuidelines = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'FULL-DESCRIPTION')) {
                    $scope.isHideFulldescription = true;
                    if ($scope.caseProfile && ($scope.caseProfile.Description != null && $scope.caseProfile.Description !== "" && $scope.caseProfile.Description.toUpperCase() !== "NULL")) {
                        $scope.isFulldescriptionDisabled = true;
                        $scope.isHideFulldescription = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'DESCRIPTION-NEW')) {
                    $scope.isHideDescriptionnew = true;
                    if ($scope.caseProfile && ($scope.caseProfile.DescriptionNew != null && $scope.caseProfile.DescriptionNew !== "" && $scope.caseProfile.DescriptionNew.toUpperCase() !== "NULL")) {
                        $scope.isDescriptionnewDisabled = true;
                        $scope.isHideDescriptionnew = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'CONCLUSION')) {
                    $scope.isHideConclusion = true;
                    if ($scope.caseProfile && ($scope.caseProfile.Conclusion != null && $scope.caseProfile.Conclusion !== "" && $scope.caseProfile.Conclusion.toUpperCase() !== "NULL")) {
                        $scope.isConclusionDisabled = true;
                        $scope.isHideConclusion = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'COMPANY-DIALOGUE')) {
                    $scope.isHideCompanydialogue = true;
                    if ($scope.caseProfile && ($scope.caseProfile.CompanyDialogueSummary != null && $scope.caseProfile.CompanyDialogueSummary !== "" && $scope.caseProfile.CompanyDialogueSummary.toUpperCase() !== "NULL")) {
                        $scope.isCompanydialogueDisabled = true;
                        $scope.isHideCompanydialogue = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'COMPANY-DIALOGUE-NEW')) {
                    $scope.isHideCompanydialoguenew = true;
                    if ($scope.caseProfile && ($scope.caseProfile.CompanyDialogueNew != null && $scope.caseProfile.CompanyDialogueNew !== "" && $scope.caseProfile.CompanyDialogueNew.toUpperCase() !== "NULL")) {
                        $scope.isCompanydialoguenewDisabled = true;
                        $scope.isHideCompanydialoguenew = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'COMPANY-DIALOGUE-LOG')) {

                    $scope.isHideCompanydialoguelog = false;
                    $scope.isCompanydialoguelogDisabled = true;
                    if (null === $scope.caseProfile.CompanyDialogueLogs || ($scope.caseProfile.CompanyDialogueLogs !== null && $scope.caseProfile.CompanyDialogueLogs.length === 0)) {
                        $scope.isHideCompanydialoguelog = true;
                    }
                    
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'SOURCE-DIALOGUE')) {
                    $scope.isHideSourcedialogue = true;
                    if ($scope.caseProfile && ($scope.caseProfile.SourceDialogueSummary != null && $scope.caseProfile.SourceDialogueSummary !== "" && $scope.caseProfile.SourceDialogueSummary.toUpperCase() !== "NULL")) {
                        $scope.isSourcedialogueDisabled = true;
                        $scope.isHideSourcedialogue = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'SOURCE-DIALOGUE-NEW')) {
                    $scope.isHideSourcedialoguenew = true;
                    if ($scope.caseProfile && ($scope.caseProfile.SourceDialogueNew != null && $scope.caseProfile.SourceDialogueNew !== "" && $scope.caseProfile.SourceDialogueNew.toUpperCase() !== "NULL")) {
                        $scope.isSourcedialoguenewDisabled = true;
                        $scope.isHideSourcedialoguenew = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'SOURCE-DIALOGUE-LOG')) {
                    $scope.isHideSourcedialoguelog = false;
                    $scope.isSourcedialoguelogDisabled = true;
                    if (null === $scope.caseProfile.SourceDialogueLogs || ($scope.caseProfile.SourceDialogueLogs !== null && $scope.caseProfile.SourceDialogueLogs.length === 0)) {
                        $scope.isHideSourcedialoguelog = true;
                    }
                    
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'GES-COMMENTARY')) {

                    $scope.isHideGEScommentary = false;
                    $scope.isGEScommentaryDisabled = true;
                    if (null === $scope.caseProfile.CommentaryViewModels || ($scope.caseProfile.CommentaryViewModels !== null && $scope.caseProfile.CommentaryViewModels.length === 0)) {
                        $scope.isHideGEScommentary = true;
                    }                   
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'GSS-LINK')) {

                    $scope.isHideGSSLink = false;
                    $scope.isGSSLinkDisabled = true;
                    if (null === $scope.caseProfile.GSSLinkViewModels || ($scope.caseProfile.GSSLinkViewModels !== null && $scope.caseProfile.GSSLinkViewModels.length === 0)) {
                        $scope.isHideGSSLink = true;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'NEWS')) {
                    $scope.isHideNews = false;
                    $scope.isNewsDisabled = true;
                    if (null === $scope.caseProfile.GesLatestNewsViewModels || ($scope.caseProfile.GesLatestNewsViewModels !== null && $scope.caseProfile.GesLatestNewsViewModels.length === 0)) {
                        $scope.isHideNews = true;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'CHANGE-OBJECTIVE')) {
                    $scope.isHideChangeobjective = true;
                    if ($scope.caseProfile && ($scope.caseProfile.ProcessGoal != null && $scope.caseProfile.ProcessGoal !== "" && $scope.caseProfile.ProcessGoal.toUpperCase() !== "NULL")) {
                        $scope.isChangeobjectiveDisabled = true;
                        $scope.isHideChangeobjective = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'MILESTONE')) {
 
                    $scope.isHideMilestone = false;
                    $scope.isMilestoneDisabled = true;
                    if (null === $scope.caseProfile.MileStoneModel || ($scope.caseProfile.MileStoneModel !== null && $scope.caseProfile.MileStoneModel.length === 0)) {
                        $scope.isHideMilestone = true;
                    }
                    
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'NEXT-STEP')) {
                    $scope.isHideNextStep = true;
                    if ($scope.caseProfile && ($scope.caseProfile.ProcessStep != null && $scope.caseProfile.ProcessStep !== "" && $scope.caseProfile.ProcessStep.toUpperCase() !== "NULL")) {
                        $scope.isNextStepDisabled = true;
                        $scope.isHideNextStep = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'DISCUSSION-POINTS')) {

                    $scope.isHideDiscussionpoints = false;
                    $scope.isDiscussionpointsDisabled = true;
                    if (null === $scope.caseProfile.DiscussionPoints || ($scope.caseProfile.DiscussionPoints !== null && $scope.caseProfile.DiscussionPoints.length === 0)) {
                        $scope.isHideDiscussionpoints = true;
                    }                   
                    
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'OTHER-STAKEHOLDERS')) {

                    $scope.isHideOtherstakeholders = false;
                    $scope.isOtherstakeholdersDisabled = true;
                    if (null === $scope.caseProfile.StakeholderViews || ($scope.caseProfile.StakeholderViews !== null && $scope.caseProfile.StakeholderViews.length === 0)) {
                        $scope.isHideOtherstakeholders = true;
                    }

                }
                
                
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'ASSOCIATED-CORPORATIONS')) {

                    $scope.isHideAssociatedCorporations = false;
                    $scope.isAssociatedCorporationsDisabled = true;
                    if (null === $scope.caseProfile.AssociatedCorporations || ($scope.caseProfile.AssociatedCorporations !== null && $scope.caseProfile.AssociatedCorporations.length === 0)) {
                        $scope.isHideAssociatedCorporations = true;
                    }

                }
                
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'SOURCES')) {

                    $scope.isHideSources = false;
                    $scope.isSourcesDisabled = true;
                    if (null === $scope.caseProfile.SourcesViewModels || ($scope.caseProfile.SourcesViewModels !== null && $scope.caseProfile.SourcesViewModels.length === 0)) {
                        $scope.isHideSources = true;
                    }
                }
                
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'REFERENCES')) {

                    $scope.isHideReferences = false;
                    $scope.isReferencesDisabled = true;
                    if (null === $scope.caseProfile.ReferencesViewModels || ($scope.caseProfile.ReferencesViewModels !== null && $scope.caseProfile.ReferencesViewModels.length === 0)) {
                        $scope.isHideReferences = true;
                    }
                }
                
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'SUPPLEMENTARY-READING')) {

                    $scope.isHideSupplementaryReading = false;
                    $scope.isSupplementaryReadingDisabled = true;
                    if (null === $scope.caseProfile.SupplementaryReadingViewModels || ($scope.caseProfile.SupplementaryReadingViewModels !== null && $scope.caseProfile.SupplementaryReadingViewModels.length === 0)) {
                        $scope.isHideSupplementaryReading = true;
                    }
                }
                
                
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === '360-INCIDENT-ANALYSIS-SUMMARY')) {
                    $scope.isHide360IncidentAnalysisSummary = true;
                    if ($scope.caseProfile && ($scope.caseProfile.IncidentAnalysisSummary != null && $scope.caseProfile.IncidentAnalysisSummary !== "" && $scope.caseProfile.IncidentAnalysisSummary.toUpperCase() !== "NULL")) {
                        $scope.is360IncidentAnalysisSummaryDisabled = true;
                        $scope.isHide360IncidentAnalysisSummary = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === '360-INCIDENT-ANALYSIS-DIALOGUE-AND-ANALYSIS')) {
                    $scope.isHide360IncidentAnalysisDialogueAndAnalysis = true;
                    if ($scope.caseProfile && ($scope.caseProfile.IncidentAnalysisDialogueAndAnalysis != null && $scope.caseProfile.IncidentAnalysisDialogueAndAnalysis !== "" && $scope.caseProfile.IncidentAnalysisDialogueAndAnalysis.toUpperCase() !== "NULL")) {
                        $scope.is360IncidentAnalysisDialogueAndAnalysisDisabled = true;
                        $scope.isHide360IncidentAnalysisDialogueAndAnalysis = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === '360-INCIDENT-ANALYSIS-CONCLUSION')) {
                    $scope.isHide360IncidentAnalysisConclusion = true;
                    if ($scope.caseProfile && ($scope.caseProfile.IncidentAnalysisConclusion != null && $scope.caseProfile.IncidentAnalysisConclusion !== "" && $scope.caseProfile.IncidentAnalysisConclusion.toUpperCase() !== "NULL")) {
                        $scope.is360IncidentAnalysisConclusionDisabled = true;
                        $scope.isHide360IncidentAnalysisConclusion = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === '360-INCIDENT-ANALYSIS-GUIDELINES-AND-CONVENTIONS')) {
                    $scope.isHide360IncidentAnalysisGuidelinesandconventions = true;
                    if ($scope.caseProfile && ($scope.caseProfile.IncidentAnalysisGuidelines != null && $scope.caseProfile.IncidentAnalysisGuidelines !== "" && $scope.caseProfile.IncidentAnalysisGuidelines.toUpperCase() !== "NULL")) {
                        $scope.is360IncidentAnalysisGuidelinesandconventionsDisabled = true;
                        $scope.isHide360IncidentAnalysisGuidelinesandconventions = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === '360-INCIDENT-ANALYSIS-SOURCES')) {
                    $scope.isHide360IncidentAnalysisSources = true;
                    if ($scope.caseProfile && ($scope.caseProfile.IncidentAnalysisSources != null && $scope.caseProfile.IncidentAnalysisSources !== "" && $scope.caseProfile.IncidentAnalysisSources.toUpperCase() !== "NULL")) {
                        $scope.is360IncidentAnalysisSourcesDisabled = true;
                        $scope.isHide360IncidentAnalysisSources = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === '360-CLOSING-INCIDENT-ANALYSIS-SUMMARY')) {
                    $scope.isHide360ClosingIncidentAnalysisSummary = true;
                    if ($scope.caseProfile && ($scope.caseProfile.ClosingIncidentAnalysisSummary != null && $scope.caseProfile.ClosingIncidentAnalysisSummary !== "" && $scope.caseProfile.ClosingIncidentAnalysisSummary.toUpperCase() !== "NULL")) {
                        $scope.is360ClosingIncidentAnalysisSummaryDisabled = true;
                        $scope.isHide360ClosingIncidentAnalysisSummary = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === '360-CLOSING-INCIDENT-ANALYSIS-DIALOGUE-AND-ANALYSIS')) {
                    $scope.isHide360ClosingIncidentAnalysisDialogueAndAnalysis = true;
                    if ($scope.caseProfile && ($scope.caseProfile.ClosingIncidentAnalysisDialogueAndAnalysis != null && $scope.caseProfile.ClosingIncidentAnalysisDialogueAndAnalysis !== "" && $scope.caseProfile.ClosingIncidentAnalysisDialogueAndAnalysis.toUpperCase() !== "NULL")) {
                        $scope.is360ClosingIncidentAnalysisDialogueAndAnalysisDisabled = true;
                        $scope.isHide360ClosingIncidentAnalysisDialogueAndAnalysis = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === '360-CLOSING-INCIDENT-ANALYSIS-CONCLUSION')) {
                    $scope.isHide360ClosingIncidentAnalysisConclusion = true;
                    if ($scope.caseProfile && ($scope.caseProfile.ClosingIncidentAnalysisConclusion != null && $scope.caseProfile.ClosingIncidentAnalysisConclusion !== "" && $scope.caseProfile.ClosingIncidentAnalysisConclusion.toUpperCase() !== "NULL")) {
                        $scope.is360ClosingIncidentAnalysisConclusionDisabled = true;
                        $scope.isHide360ClosingIncidentAnalysisConclusion = false;
                    }
                }
                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'UNGP-ASSESSMENT-FORM')) {
                    $scope.isHideUNGPassessmentform = true;
                    if ($scope.caseProfile && $scope.caseProfile.GesUngpAssessmentFormViewModel != null && $scope.caseProfile.GesUngpAssessmentFormViewModel.GesUNGPAssessmentFormId != "00000000-0000-0000-0000-000000000000") {
                        $scope.isUNGPassessmentformDisabled = true;
                        $scope.isHideUNGPassessmentform = false;
                    }
                }

                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'ADDITIONAL-DOCUMENT')) {

                    $scope.isHideAdditionalDocument = false;
                    $scope.isAdditionalDocumentDisabled = true;
                    if (null === $scope.caseProfile.Documents || ($scope.caseProfile.Documents !== null && $scope.caseProfile.Documents.length === 0)) {
                        $scope.isHideAdditionalDocument = true;
                    }
                }


                if (($scope.caseProfileInvisibleEntities[i].EntityCodeType === 'INTERNAL-COMMENTS')) {
                    $scope.isHideInternalComments = true;
                    if ($scope.caseProfile && ($scope.caseProfile.Comment != null && $scope.caseProfile.Comment !== "")) {
                        $scope.isInternalCommentsDisabled = true;
                        $scope.isHideInternalComments = false;
                    }
                }

           
            }
        }
    }   

    
    function initSelect2() {
        //Have a bug with ui-select2 directive in form
        //So, use manually initialize instead of directive

        $("#response-status-select").select2({
            templateResult: responseStatusFormatState,
            templateSelection: responseStatusFormatState
        });
        $("#progress-status-select").select2({
            templateResult: progressStatusFormatState,
            templateSelection: progressStatusFormatState
        });
        $("#norm-select").select2({
            templateResult: normAreaFormatState,
            templateSelection: normAreaFormatState
        });
        $("#country-select").select2({
            templateResult: countryFormatState,
            templateSelection: countryFormatState
        });
        $("#engagement-type-select").select2();

        $("#recommendation-select").select2();
        $("#user-select").select2();

        $("#taxonomy-select").select2();
        $("#document-service-select").select2();

        $("#caseprofilemilestone-select").select2({
            templateResult: milestoneFormatState,
            templateSelection: milestoneFormatState
        });

        $("#issue-heading-select").select2();
    }

    function convertDate(value, format) {
        if (value != null && !value.isNullOrEmpty) {
            return $filter("date")(new Date(parseInt(value.substr(6))), format);
        }

        return null;
    };
    
    function loadDoucments() {
        var companyId = getCompanyId();
        CaseProfileService.GetAdditionalDocuments(companyId).then(
            function (d) {
                $scope.documents = d.data;
                //TODO: This code is belong with additional document function
                //$timeout(function() {
                //    $(".icon-remove").confirmModal({
                //        confirmCallback: $scope.deleteDocument
                //    });
                //});
            },
            function (reason) {
                quickNotification("Error occurred during loading Additional Documents, caused: " + reason, "error");
            }
        );
    }

    function countryFormatState(state) {
        if (!state.id) {
            return state.text;
        }

        var flagIcon = "";
        var countryCode = state.element.attributes["data-country-code"];
        if (countryCode !== undefined && countryCode !== null)
            flagIcon = '<span class="flag-icon flag-icon-' + countryCode.value.toLowerCase() + '"></span>';
        return $(
            '<div class="status-value">' + flagIcon + state.text + "</div>"
        );
    }

    function normAreaFormatState(state) {
        if (!state.id) {
            return state.text;
        }
        var baseUrl = "/Content/img/icons/norm_area_";
        return $(
            '<div class="status-value response-status" style="background-image: url(\'' + baseUrl + state.element.value + '.png\')">' + state.text + "</div>"
        );
    }

    function progressStatusFormatState(state) {
        var baseUrl = "/Content/img/icons/progress_";
        return formatState(state, baseUrl);
    }

    function responseStatusFormatState(state) {
        var baseUrl = "/Content/img/icons/response_";
        return formatState(state, baseUrl);
    }

    function milestoneFormatState(state) {
        var levelValue = $(state.element).attr("data-milestone-level-code");

        if (!levelValue) {
            return state.text;
        }
        var baseUrl = "/Content/img/icons/milestone_";
        return $(
            '<div class="status-value response-status" style="background-image: url(\'' + baseUrl + levelValue + '.png\')">' + state.text + "</div>"
        );
    }

    function formatState(state, baseUrl) {
        if (!state.id) {
            return state.text;
        }
        return $(
            '<div class="status-value response-status" style="background-image: url(\'' + baseUrl + state.text.toLowerCase() + '.png\')">' + state.text + "</div>"
        );
    }

    function fetchUserAvatar(objectId, newData) {
        //var object = document.getElementById(objectId);
        //object.setAttribute("data", newData);

        //var clone = object.cloneNode(true);
        //var parent = object.parentNode;

        //parent.removeChild(object);
        //parent.appendChild(clone);
    }        

    $scope.GetNormSectionTitle = function (id) {
        var obj = $scope.norms.filter(function (norm) {
            return norm.I_Norms_Id == id;
        });

        if (obj != null && obj.length > 0)
            return obj[0].SectionTitle;
        return "";
    }

    $scope.GetConventionName = function (id) {
        var obj = $scope.conventions.filter(function (convention) {
            return convention.I_Conventions_Id == id;
        });

        if (obj != null && obj.length > 0)
            return obj[0].Name;
        return "";
    };
    
    $scope.GetHeadingName = function (id) {
        var obj = $scope.issueheadings.filter(function (heading) {
            return heading.Id == id;
        });

        if (obj != null && obj.length > 0)
            return obj[0].Name;
        return "";
    }

    function resetEntities() {
        $scope.isHideCompanyinfo =  false;
        $scope.isHideEntrydate =  false;
        $scope.isHideNorm =  false;
        $scope.isHideLocation =  false;
        $scope.isHideRecommendation =  false;
        $scope.isHideCaseInformationConclusion =  false;
        $scope.isHideEngagementtype =  false;
        $scope.isHideResponse =  false;
        $scope.isHideProgress =  false;
        $scope.isHideDevelopment =  false;
        $scope.isHideGEScontactinformation =  false;
        $scope.isHideSDGs =  false;
        $scope.isHideGuidelineslist =  false;
        $scope.isHideConventions =  false;
        $scope.isHideKpis =  false;
        $scope.isHideSummary =  false;
        $scope.isHideGuidelines =  false;
        $scope.isHideFulldescription =  false;
        $scope.isHideDescriptionnew =  false;
        $scope.isHideConclusion =  false;
        $scope.isHideCompanydialogue =  false;
        $scope.isHideCompanydialoguenew =  false;
        $scope.isHideCompanydialoguelog =  false;
        $scope.isHideSourcedialogue =  false;
        $scope.isHideSourcedialoguenew =  false;
        $scope.isHideSourcedialoguelog =  false;
        $scope.isHideGEScommentary =  false;
        $scope.isHideNews =  false;
        $scope.isHideChangeobjective =  false;
        $scope.isHideMilestone =  false;
        $scope.isHideNextStep =  false;
        $scope.isHideDiscussionpoints =  false;
        $scope.isHideOtherstakeholders =  false;
        $scope.isHideAssociatedCorporations =  false;
        $scope.isHideSources =  false;
        $scope.isHideReferences =  false;
        $scope.isHideSupplementaryReading =  false;
        $scope.isHide360IncidentAnalysisSummary =  false;
        $scope.isHide360IncidentAnalysisDialogueAndAnalysis =  false;
        $scope.isHide360IncidentAnalysisConclusion =  false;
        $scope.isHide360IncidentAnalysisGuidelinesandconventions =  false;
        $scope.isHide360IncidentAnalysisSources =  false;
        $scope.isHide360ClosingIncidentAnalysisSummary =  false;
        $scope.isHide360ClosingIncidentAnalysisDialogueAndAnalysis =  false;
        $scope.isHide360ClosingIncidentAnalysisConclusion =  false;
        $scope.isHideUNGPassessmentform =  false;


        $scope.isCompanyinfoDisabled =  false;
        $scope.isEntrydateDisabled =  false;
        $scope.isNormDisabled =  false;
        $scope.isLocationDisabled =  false;
        $scope.isRecommendationDisabled =  false;
        $scope.isCaseInformationConclusionDisabled =  false;
        $scope.isEngagementtypeDisabled =  false;
        $scope.isResponseDisabled =  false;
        $scope.isProgressDisabled =  false;
        $scope.isDevelopmentDisabled =  false;
        $scope.isGEScontactinformationDisabled =  false;
        $scope.isSDGsDisabled =  false;
        $scope.isGuidelineslistDisabled =  false;
        $scope.isConventionsDisabled =  false;
        $scope.isKpisDisabled =  false;
        $scope.isSummaryDisabled =  false;
        $scope.isGuidelinesDisabled =  false;
        $scope.isFulldescriptionDisabled =  false;
        $scope.isDescriptionnewDisabled =  false;
        $scope.isConclusionDisabled =  false;
        $scope.isCompanydialogueDisabled =  false;
        $scope.isCompanydialoguenewDisabled =  false;
        $scope.isCompanydialoguelogDisabled =  false;
        $scope.isSourcedialogueDisabled =  false;
        $scope.isSourcedialoguenewDisabled =  false;
        $scope.isSourcedialoguelogDisabled =  false;
        $scope.isGEScommentaryDisabled =  false;
        $scope.isNewsDisabled =  false;
        $scope.isChangeobjectiveDisabled =  false;
        $scope.isMilestoneDisabled =  false;
        $scope.isNextStepDisabled =  false;
        $scope.isDiscussionpointsDisabled =  false;
        $scope.isOtherstakeholdersDisabled =  false;
        $scope.isAssociatedCorporationsDisabled =  false;
        $scope.isSourcesDisabled =  false;
        $scope.isReferencesDisabled =  false;
        $scope.isSupplementaryReadingDisabled =  false;
        $scope.is360IncidentAnalysisSummaryDisabled =  false;
        $scope.is360IncidentAnalysisDialogueAndAnalysisDisabled =  false;
        $scope.is360IncidentAnalysisConclusionDisabled =  false;
        $scope.is360IncidentAnalysisGuidelinesandconventionsDisabled =  false;
        $scope.is360IncidentAnalysisSourcesDisabled =  false;
        $scope.is360ClosingIncidentAnalysisSummaryDisabled =  false;
        $scope.is360ClosingIncidentAnalysisDialogueAndAnalysisDisabled =  false;
        $scope.is360ClosingIncidentAnalysisConclusionDisabled =  false;
        $scope.isUNGPassessmentformDisabled =  false;

    }
    
}]);