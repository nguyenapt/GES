"use strict";

GesInsideApp.factory("EngagementTypeService", ["$http", "$q", "$window", "Upload", function ($http, $q, $window, Upload) {
    return {
        GetEngagementTypeDetailsById: function (engagementTypeId) {
            if (engagementTypeId !== 0) {
                return $http.get("/EngagementType/GetEngagementTypeById", { params: { id: engagementTypeId } });
            }
            return null;
        },
        GetAllEngagementTypeCategories: function () {
            return $http.get("/EngagementType/GetEngagementTypeCategories");
        },
        GetAllEngagementTypeContacts: function () {
            return $http.get("/EngagementType/GetEngagementTypeContacts");
        },
        GetDocumentTypes: function () {
            return $http.get("/EngagementType/GetDocumentTypes");
        },
        UpdateEngagementTypeData: function (engagementType) {
            
            return $http({
                url: "/EngagementType/UpdateEngagementType",
                method: "POST",
                data: engagementType
            });
        },
        DeleteEngagementTypeData: function (engagementType) {

            return $http({
                url: "/EngagementType/DeleteEngagementType",
                method: "POST",
                data: engagementType
            });
        },
        ActiveOrDeactiveEngagementType: function (engagementType) {

            return $http({
                url: "/EngagementType/ActiveOrDeactiveEngagementType",
                method: "POST",
                data: engagementType
            });
        },
        UploadFile: function(files) {
            if (files.length === 0) return;
            var file = files[0];

            var payload = new FormData();
            payload.append("stuff", "some string");
            payload.append("file", file);

            return $http({
                url: "/EngagementType/UploadFiles",
                method: "POST",
                data: payload,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            });
        }
    }
}]);