"use strict";

GesInsideApp.factory("CompanyService", ["$http", "$q", "$window", "Upload", function ($http, $q, $window, Upload) {
    return {
        GetCompanyDetails: function () {
            return this.GetCompanyDetailsById();
        },
        GetCompanyDetailsById: function () {
            var urlPath = $window.location.href;
            var urlPathSplit = String(urlPath).split("/");
            var companyId = 0;

            if (urlPathSplit !== null && urlPathSplit.length > 0) {
                companyId = urlPathSplit[urlPathSplit.length - 1];
            }

            if (companyId !== 0) {
                return $http.get("/Company/GetCompanyById", { params: { id: companyId } });
            }
            return null;
        },
        GetCompanyId: function () {
            var urlPath = $window.location.href;
            var urlPathSplit = String(urlPath).split("/");
            var companyId = 0;

            if (urlPathSplit !== null && urlPathSplit.length > 0) {
                companyId = urlPathSplit[urlPathSplit.length - 1];
            }

            if (companyId !== 0) {
                return companyId;
            }
            return 0;
        },
        UpdateCompanyData: function (company) {
            
            return $http({
                url: "/Company/UpdateCompany",
                method: "POST",
                data: company
            });
        },
        GetAllCountries: function () {
            return $http.get("/CaseProfile/GetCountries");
        },
        GetAllSubPeerGroups: function () {
            return $http.get("/Company/GetAllSubPeerGroups");
        },
        GetAllManagementSystems: function () {
            return $http.get("/Company/GetAllManagementSystems");
        },
        GetAllMscis: function () {
            return $http.get("/Company/GetAllMscis");
        },
        GetAllFtses: function () {
            return $http.get("/Company/GetAllFtses");
        },        
        GetAllCaseProfiles: function (companyId) {            
            return $http.get("/Company/GetAllCaseProfiles", { params: { companyId: companyId } });
        },
        GetCompanyEvents: function (companyId) {            
            return $http.get("/Company/GetCompanyEvents", { params: { companyId: companyId } });
        },         
        SaveCompanyManagementSystem: function (companyManagementSystem, successCallback) {
            $http({
                url: "/Company/SaveCompanyManagementSystem",
                method: "POST",
                data: {
                    companyManagementSystem: companyManagementSystem
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save company management system successfully");
                    successCallback(d.data.Id);
                } else {
                    quickNotification("Can not save company management system", "error");
                }
            });
        },
        DeleteCompanyManagementSystem: function (companyManagementSystem, successCallback) {
            $http({
                url: "/Company/DeleteCompanyManagementSystem",
                method: "POST",
                data: {
                    companyManagementSystem: companyManagementSystem
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete company management system successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete company management system", "error");
                }
            });
        },

        DeleteCompanyEvent: function (companyEvent, successCallback) {
            $http({
                url: "/Company/DeleteCompanyEvent",
                method: "POST",
                data: {
                    companyEvent: companyEvent
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Delete company event successfully");
                    successCallback();
                } else {
                    quickNotification("Can not delete company event", "error");
                }
            });
        },
        SaveCompanyEvent: function (companyEvent, successCallback) {
            $http({
                url: "/Company/SaveCompanyEvent",
                method: "POST",
                data: {
                    companyEvent: companyEvent
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Save company event system successfully");
                    successCallback();
                } else {
                    quickNotification("Can not save company event system", "error");
                }
            });
        },
        SaveCompanyAdditionalDocument: function (companyAdditionalDocument, file, successCallback) {           

            var payload = new FormData();
            payload.append("managedDocumentsId", companyAdditionalDocument.G_ManagedDocuments_Id);
            payload.append("companiesId", companyAdditionalDocument.I_Companies_Id);
            payload.append("caseProfileId", companyAdditionalDocument.I_GesCaseReports_Id);
            payload.append("name", companyAdditionalDocument.Name);
            payload.append("comment", companyAdditionalDocument.Comment);
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

        DeleteCompanyAdditionalDocument: function (companyAdditionalDocument, successCallback) {
            
            var documentIds = [];
            documentIds.push(companyAdditionalDocument.G_ManagedDocuments_Id);
            
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
        
        companyDetails: null
    }
}]);