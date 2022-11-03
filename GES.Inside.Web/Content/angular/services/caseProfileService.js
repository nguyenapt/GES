"use strict";

GesInsideApp.factory("CaseProfileService", ["$http", "$q", "$window", "Upload", function ($http, $q, $window, Upload) {
    return {
        GetCaseProfileId: function () {
            var urlPath = $window.location.href;
            var urlPathSplit = String(urlPath).split("/");

            var id = 0;

            if (urlPathSplit !== null && urlPathSplit.length > 0) {
                var i = urlPathSplit[urlPathSplit.length - 1].indexOf('#');
                var s = urlPathSplit[urlPathSplit.length - 1];
                if (i !== -1) {
                    s = urlPathSplit[urlPathSplit.length - 1].substring(0, i);
                }

                if (s !== 'Add') {
                    id = s;
                }
            }
            return id;
        },
        GetCompanyId: function () {
            var urlPath = $window.location.href;
            var result = String(urlPath).split("=");
            if (result !== null && result.length > 0) {
                var id = result[result.length - 1];
                return id;
            }
            return 0;
        },
        getParameterByName: function(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, '\\$&');
            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, ' '));
        },
        
        GetCaseProfileById: function () {
            var caseProfileId = this.GetCaseProfileId();
            if (caseProfileId !== 0) {
                return $http.get("/CaseProfile/GetCaseProfileById", { params: { id: caseProfileId } });
            }
            return null;
        },
        GetNewCaseProfile: function () {
            var companyId = this.getParameterByName("companyId");
            
            if (companyId !== null) {
                return $http.get("/CaseProfile/GetNewCaseProfile", { params: { companyId: companyId } });
            }
            return null;
        },
        IsAddNewCaseProfile: function () {
            var urlPath = $window.location.href;
            return urlPath.includes("/Add");
        },
        GetCaseProfile: function () {
            if (this.IsAddNewCaseProfile()) {
                return this.GetNewCaseProfile();
            }
            return this.GetCaseProfileById();
        },
        GetCaseProfileInvisibleEntities: function (engagementTypesId, gesCaseReportStatusesId) {
            return $http.get("/CaseProfile/GetCaseProfileInvisibleEntities", {
                params: {
                    engagementTypesId: engagementTypesId,
                    gesCaseReportStatusesId: gesCaseReportStatusesId
                }
            });
        },
        CheckExistHeading: function (issueHeadingId, caseprofileId,companyId) {
            return $http.get("/CaseProfile/CheckExistHeading", {
                params: {                    
                    issueHeadingId: issueHeadingId,
                    caseprofileId: caseprofileId,
                    companyId: companyId
                }
            });
        },
        GetAllCountries: function () {
            return $http.get("/CaseProfile/GetCountries");
        },

        GetMilestoneTypes: function () {
            return $http.get("/CaseProfile/GetMilestoneTypes");
        },
        GetMilestonesByCaseReportId: function (caseId) {
            return $http.get("/CaseProfile/GetMilestoneByCaseReportId", { params: { caseId: caseId } });
        },
        GetManagedDocumentByDocumentId: function (companyDialogId, documentId) {
            return $http.get("/CaseProfile/GetManagedDocumentByDocumentId", { params: { companyDialogId: companyDialogId, documentId: documentId } });
        },
        GetManagedDocumentByCompanySourceDialogId: function (companySourceDialogId, dialogType) {
            return $http.get("/CaseProfile/GetManagedDocumentByCompanySourceDialogId", { params: { companySourceDialogId: companySourceDialogId, dialogType: dialogType } });
        },
        GetAllRecommendations: function () {
            return $http.get("/CaseProfile/GetRecommendations");
        },
        GetAllIssueHeadings: function () {
            return $http.get("/CaseProfile/GetIssueHeadings");
        },
        GetAllConclusions: function () {
            return $http.get("/CaseProfile/GetConclusions");
        },
        
        GetAllEngagementTypes: function () {
            return $http.get("/CaseProfile/GetEngagementTypes");
        },

        GetNormAreas: function () {
            return $http.get("/CaseProfile/GetNormAreas");
        },

        GetContactDirections: function () {
            return $http.get("/CaseProfile/GetContactDirections");
        },

        GetContactTypes: function () {
            return $http.get("/CaseProfile/GetContactTypes");
        },

        GetUsers: function () {
            return $http.get("/CaseProfile/GetUsers");
        },

        GetUserById: function (userId) {
            return $http.get("/CaseProfile/GetUserById", { params: { id: userId } });
        },

        GetResponseStatuses: function () {
            return $http.get("/CaseProfile/GetResponseStatuses");
        },

        GetProgressStatuses: function () {
            return $http.get("/CaseProfile/GetProgressStatuses");
        },

        GetDevelopmentGradeText: function (progressId, responseId) {
            return $http.get("/CaseProfile/GetDevelopmentGradeText",
                { params: { progressId: progressId, responseId: responseId } });
        },

        GetAdditionalDocuments: function (companyId) {
            if (companyId !== 0) {
                return $http.get("/CaseProfile/GetAdditionalDocuments", { params: { companyId: companyId } });
            }
            return null;
        },
        
        GetCompanyDetailsById: function (companyId) {
            if (companyId !== 0 && companyId > 0) {
                return $http.get("/Company/GetCompanyById", { params: { id: companyId } });
            }
            return null;
        },

        GetUploadedDocumentById: function (documentId) {
            if (documentId !== 0) {
                return $http.get("/CaseProfile/GetUploadedDocumentById", { params: { documentId: documentId } });
            }
            return null;
        },

        GetManagedDocumentServices: function () {
            return $http.get("/CaseProfile/GetManagedDocumentServices");
        },
        GetConventionsServices: function () {
            return $http.get("/CaseProfile/GetAllConventions");
        },
        GetNormsServices: function () {
            return $http.get("/CaseProfile/GetNorms");
        },
        GetDocumentManagementTaxonomies: function () {
            return $http.get("/CaseProfile/GetDocumentManagementTaxonomies");
        },

        GetSdgs: function () {
            return $http.get("/Sdg/GetSdgs");
        },
        GetCaseReportKPIsByCaseReportId: function (gesCaseReportId) {
            return $http.get("/CaseProfile/GetCaseReportKpisByCaseReportId", { params: { gesCaseReportId: gesCaseReportId } });
        },
        GetRecommendationLogsByCaseReportId: function (gesCaseReportId) {
            return $http.get("/CaseProfile/GetRecommendationHistory", { params: { gesCaseReportId: gesCaseReportId } });
        },
        DeleteRecommendationLog: function (recommendationLog, successCallback) {
            $http({
                url: "/CaseProfile/DeleteGesCaseAuditLogs",
                method: "POST",
                data: {
                    recommendationLog: recommendationLog
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete recommendation history successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete recommendation history", "error");
                }
            });
        },
        SaveRecommendationLogs: function (caseId,recommendationLogs, successCallback) {
            $http({
                url: "/CaseProfile/SaveGesCaseAuditLogs",
                method: "POST",
                data: {
                    caseReportId: caseId,
                    recommendationLogs: recommendationLogs,
                    statusType: 'recommendation'
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save recommendation history successfully");
                    successCallback(d.data);
                } else {
                    quickNotification("Can not save recommendation history", "error");
                }
            });
        },
        GetConclusionLogsByCaseReportId: function (gesCaseReportId) {
            return $http.get("/CaseProfile/GetConclusionHistory", { params: { gesCaseReportId: gesCaseReportId } });
        },
        DeleteConclusionLog: function (recommendationLog, successCallback) {
            $http({
                url: "/CaseProfile/DeleteGesCaseAuditLogs",
                method: "POST",
                data: {
                    recommendationLog: recommendationLog
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete conclusion history successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete conclusion history", "error");
                }
            });
        },
        SaveConclusionLogs: function (caseId, recommendationLogs, successCallback) {
            $http({
                url: "/CaseProfile/SaveGesCaseAuditLogs",
                method: "POST",
                data: {
                    caseReportId: caseId,
                    recommendationLogs: recommendationLogs,
                    statusType: 'conclusion'
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save conclusion history successfully");
                    successCallback(d.data);
                } else {
                    quickNotification("Can not save conclusion history", "error");
                }
            });
        },
        GetGesUngpAssessment: function (gesCaseReportId) {
            return $http.get("/CaseProfile/GetGesUngpAssessment", { params: { gesCaseReportId: gesCaseReportId} });
        },
        GetKpisByEngagementTypeId: function (engagementTypeId) {
            return $http.get("/CaseProfile/GetKpisByEngagementTypeId", { params: { engagementTypeId: engagementTypeId } });
        },
        GetKpiPerformances: function () {
            return $http.get("/CaseProfile/GetKpiPerformances");
        },

        GetUngpAssessmentScores: function () {
            return $http.get("/CaseProfile/GetUngpAssessmentScores");
        },
        SaveUngp: function (caseProfileId, ungp, successCallback) {
            $http({
                url: "/CaseProfile/SaveUngp",
                method: "POST",
                data: {
                    caseProfileId: caseProfileId,
                    ungp: ungp
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save UNGP successfully");
                    successCallback(d.data);
                } else {
                    quickNotification("Can not save ungp", "error");
                }
            });
        },
        ShowHideUngp: function (caseProfileId, isPublished, successCallback) {
            $http({
                url: "/CaseProfile/ShowHideUngp",
                method: "POST",
                data: {
                    caseProfileId: caseProfileId,
                    isPublished: isPublished                    
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    if (isPublished){
                        quickNotification("The UNGP information is published  successfully");
                    } else{
                        quickNotification("The UNGP information is un-published  successfully");
                    }
                    
                    successCallback(d.data);
                } else {
                    quickNotification("Can not save ungp", "error");
                }
            });
        },
        UpdateCaseProfileData: function (caseProfile) {
            caseProfile.Id = this.GetCaseProfileId();
            return $http({
                url: "/CaseProfile/UpdateCaseProfile",
                method: "POST",
                data: caseProfile
            });
        },
        SaveManagedDocument: function (document, file, successCallback) {
            Upload.upload({
                url: "/CaseProfile/SaveManagedDocument",
                method: "POST",
                data: {
                    document: document,
                    file: file
                }
            }).then(function () {
                quickNotification("Saved successfully");
                successCallback();
            });
        },
        SaveCompanyDialogAttachmentFile: function (document, file, dialogType, successCallback) {
            Upload.upload({
                url: "/CaseProfile/SaveCompanyDialogAttachmentFile",
                method: "POST",
                data: {
                    document: document,
                    file: file,
                    dialogType: dialogType
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Saved successfully");
                    successCallback(d.data.Id);
                } else {
                    quickNotification("Can not save Document", "error");
                }
            });
        },

        DeleteManagedDocument: function (documentId, successCallback) {
            $http({
                url: "/CaseProfile/DeleteManagedDocument",
                method: "POST",
                data: {
                    documentId: documentId
                }
            }).then(function () {
                quickNotification("Deleted document successfully");
                successCallback();
            });
        },

        SaveDiscussionPoint: function (discussionPoint, successCallback) {
            $http({
                url: "/CaseProfile/SaveDiscussionPoint",
                method: "POST",
                data: {
                    discussionPoint: discussionPoint
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save Discussion Point successfully");
                    successCallback(d.data.Id);
                } else {
                    quickNotification("Can not save Discussion Point", "error");
                }
            });
        },
        DeleteDiscussionPoint: function (discussionPoint, successCallback) {
            $http({
                url: "/CaseProfile/DeleteDiscussionPoint",
                method: "POST",
                data: {
                    discussionPoint: discussionPoint
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete Discussion Point successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete Discussion Point", "error");
                }
            });
        },
        SaveStakeholderView: function (stakeholderView, successCallback) {
            $http({
                url: "/CaseProfile/SaveStakeholderView",
                method: "POST",
                data: {
                    stakeholderView: stakeholderView
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save Other Stakeholder successfully");
                    successCallback(d.data.Id);
                } else {
                    quickNotification("Can not save Other Stakeholder", "error");
                }
            });
        },
        DeleteStakeholderView: function (stakeholderView, successCallback) {
            $http({
                url: "/CaseProfile/DeleteStakeholderView",
                method: "POST",
                data: {
                    stakeholderView: stakeholderView
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete Other Stakeholder successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete Other Stakeholder", "error");
                }
            });
        },
        SaveCompanyDialogue: function (dialogue, successCallback) {
            $http({
                url: "/CaseProfile/SaveCompanyDialogue",
                method: "POST",
                data: {
                    dialogue: dialogue                    
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save company dialogue successfully");
                    successCallback(d.data.Id);
                } else {
                    quickNotification("Can not save company dialogue", "error");
                }
            });
        },
        DeleteCompanyDialogue: function (dialogue, successCallback) {
            $http({
                url: "/CaseProfile/DeleteCompanyDialogue",
                method: "POST",
                data: {
                    dialogue: dialogue
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete company dialogue successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete company dialogue", "error");
                }
            });
        },
        SaveSourceDialogue: function (dialogue, successCallback) {
            $http({
                url: "/CaseProfile/SaveSourceDialogue",
                method: "POST",
                data: {
                    dialogue: dialogue
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save source dialogue successfully");
                    successCallback(d.data.Id);
                } else {
                    quickNotification("Can not save source dialogue", "error");
                }
            });
        },
        DeleteSourceDialogue: function (dialogue, successCallback) {
            $http({
                url: "/CaseProfile/DeleteSourceDialogue",
                method: "POST",
                data: {
                    dialogue: dialogue
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete source dialogue successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete source dialogue", "error");
                }
            });
        },
        SaveCommentary: function (commentary, successCallback) {
            $http({
                url: "/CaseProfile/SaveCommentary",
                method: "POST",
                data: {
                    commentary: commentary
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save commentary successfully");
                    successCallback(d.data.Id);
                } else {
                    quickNotification("Can not save commentary", "error");
                }
            });
        },
        DeleteCommentary: function (commentary, successCallback) {
            $http({
                url: "/CaseProfile/DeleteCommentary",
                method: "POST",
                data: {
                    commentary: commentary
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete commentary successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete commentary", "error");
                }
            });
        },
        saveGSSLink: function (gsslink, successCallback) {
            $http({
                url: "/CaseProfile/SaveGSSLink",
                method: "POST",
                data: {
                    gsslink: gsslink
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save gss link successfully");
                    successCallback(d.data.Id);
                } else {
                    quickNotification("Can not save gss link", "error");
                }
            });
        },
        deleteGSSLink: function (gsslink, successCallback) {
            $http({
                url: "/CaseProfile/DeleteGSSLink",
                method: "POST",
                data: {
                    gsslink: gsslink
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete gss link successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete gss link", "error");
                }
            });
        },
        SaveNews: function (news, successCallback) {
            $http({
                url: "/CaseProfile/SaveNews",
                method: "POST",
                data: {
                    news: news
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save news successfully");
                    successCallback(d.data.Id);
                } else {
                    quickNotification("Can not save news", "error");
                }
            });
        },
        DeleteNews: function (news, successCallback) {
            $http({
                url: "/CaseProfile/DeleteNews",
                method: "POST",
                data: {
                    news: news
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete news successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete news", "error");
                }
            });
        },
        SaveKpi: function (kpi, successCallback) {
            $http({
                url: "/CaseProfile/SaveKpi",
                method: "POST",
                data: {
                    kpi: kpi
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save kpi successfully");
                    successCallback(d.data.Id);
                } else {
                    quickNotification("Can not save kpi", "error");
                }
            });
        },
        DeleteKpi: function (kpi, successCallback) {
            $http({
                url: "/CaseProfile/DeleteKpi",
                method: "POST",
                data: {
                    kpi: kpi
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete kpi successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete kpi", "error");
                }
            });
        },
        SaveMilestone: function (milestone, successCallback) {
            $http({
                url: "/CaseProfile/SaveMilestone",
                method: "POST",
                data: {
                    milestone: milestone
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save milestone successfully");
                    successCallback();
                } else {
                    quickNotification("Can not save milestone", "error");
                }
            });
        },
        DeleteMilestone: function (milestoneId, successCallback) {
            $http({
                url: "/CaseProfile/DeleteMilestone",
                method: "POST",
                data: {
                    milestoneId: milestoneId
                }
            }).then(function () {
                quickNotification("Deleted milestone successfully");
                successCallback();
            });
        },
        SaveSdgs: function (caseprofileid,sdgs, successCallback) {
            $http({
                url: "/CaseProfile/SaveSdgs",
                method: "POST",
                data: {
                    I_GesCaseReports_Id: caseprofileid,
                    sdgs: sdgs
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save sdgs successfully");
                    if (successCallback != null) {
                        successCallback();
                    }
                } else {
                    quickNotification("Can not save sdgs", "error");
                }
            });
        },
        SaveGuideline: function (guideline, successCallback) {
            $http({
                url: "/CaseProfile/SaveGuideline",
                method: "POST",
                data: {
                    guideline: guideline
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save guideline successfully");
                    successCallback(d.data.Id);
                } else {
                    quickNotification("Can not save guideline", "error");
                }
            });
        },
        DeleteGuideline: function (guideline, successCallback) {
            $http({
                url: "/CaseProfile/DeleteGuideline",
                method: "POST",
                data: {
                    guideline: guideline
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete guideline successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete guideline", "error");
                }
            });
        },
        SaveConvention: function (convention, successCallback) {
            $http({
                url: "/CaseProfile/SaveConvention",
                method: "POST",
                data: {
                    convention: convention
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save convention successfully");
                    successCallback(d.data.Id);
                } else {
                    quickNotification("Can not save convention", "error");
                }
            });
        },
        DeleteConvention: function (convention, successCallback) {
            $http({
                url: "/CaseProfile/DeleteConvention",
                method: "POST",
                data: {
                    convention: convention
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete convention successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete convention", "error");
                }
            });
        },
        SaveAssociatedCorporation: function (associated, successCallback) {
            $http({
                url: "/CaseProfile/SaveAssociatedCorporation",
                method: "POST",
                data: {
                    associated: associated
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save associated corporation successfully");
                    successCallback(d.data.Id);
                } else {
                    quickNotification("Can not save associated corporation", "error");
                }
            });
        },
        DeleteAssociatedCorporation: function (associated, successCallback) {
            $http({
                url: "/CaseProfile/DeleteAssociatedCorporation",
                method: "POST",
                data: {
                    associated: associated
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete associated corporation successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete associated corporation", "error");
                }
            });
        },
        SaveSource: function (source, successCallback) {
            $http({
                url: "/CaseProfile/SaveSource",
                method: "POST",
                data: {
                    source: source
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save source successfully");
                    successCallback(d.data.Id);
                } else {
                    quickNotification("Can not save source", "error");
                }
            });
        },
        DeleteSource: function (source, successCallback) {
            $http({
                url: "/CaseProfile/DeleteSource",
                method: "POST",
                data: {
                    source: source
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete source successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete source", "error");
                }
            });
        },
        SaveReference: function (reference, successCallback) {
            $http({
                url: "/CaseProfile/SaveReference",
                method: "POST",
                data: {
                    reference: reference
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save reference successfully");
                    successCallback(d.data.Id);
                } else {
                    quickNotification("Can not save reference", "error");
                }
            });
        },
        DeleteReference: function (reference, successCallback) {
            $http({
                url: "/CaseProfile/DeleteReference",
                method: "POST",
                data: {
                    reference: reference
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete reference successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete reference", "error");
                }
            });
        },
        SaveSupplementaryReading: function (supplementary, successCallback) {
            $http({
                url: "/CaseProfile/SaveSupplementaryReading",
                method: "POST",
                data: {
                    supplementary: supplementary
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save supplementary reading successfully");
                    successCallback(d.data.Id);
                } else {
                    quickNotification("Can not save supplementary reading", "error");
                }
            });
        },
        DeleteSupplementaryReading: function (supplementary, successCallback) {
            $http({
                url: "/CaseProfile/DeleteSupplementaryReading",
                method: "POST",
                data: {
                    supplementary: supplementary
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete supplementary reading successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete supplementary reading", "error");
                }
            });
        },
        SavecaseProfileAdditionalDocument : function (caseProfileAdditionalDocument, file, successCallback) {

            var payload = new FormData();
            payload.append("managedDocumentsId", caseProfileAdditionalDocument.G_ManagedDocuments_Id);
            payload.append("companiesId", caseProfileAdditionalDocument.I_Companies_Id);
            payload.append("caseProfileId", caseProfileAdditionalDocument.I_GesCaseReports_Id);
            payload.append("name", caseProfileAdditionalDocument.Name);
            payload.append("comment", caseProfileAdditionalDocument.Comment);
            payload.append("file", file);

            $http({
                url: "/DocumentMgmt/AddOrUpdateCompanyDocument",
                method: "POST",
                data: payload,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).then(function (d) {
                if (d.data.success){
                    quickNotification("Save additional document successfully");
                    
                    successCallback(d.data);
                } else {
                    quickNotification("Can not save additional document", "error");
                }
            });

        },

        DeleteCaseProfileAdditionalDocument: function (caseProfileAdditionalDocument, successCallback) {

            var documentIds = [];
            documentIds.push(caseProfileAdditionalDocument.G_ManagedDocuments_Id);

            $http({
                url: "/DocumentMgmt/DeleteCompanyDocuments",
                method: "POST",
                data: {
                    documentIds: documentIds
                }
            }).then(function (d) {
                if (d.data.success) {
                    quickNotification("Delete additional documensuccessfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete additional document", "error");
                }
            });
        },
        DownloadDocument : function (documentId) {

            $http({
                url: "/CompanyDocDownload/" + documentId,
                method: "POST",
                data: {
                    documentName: documentId
                }
            });
        },
        caseProfile: null
    }
}]);