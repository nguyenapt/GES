"use strict";

GesInsideApp.factory("GesCaseProfileUITemplateService", ["$http", "$q", "$window", "Upload", function ($http, $q, $window, Upload) {
    return {
        /**
         * @return {null}
         */
        GetCaseProfileUITemplateDetailsById: function (templateId) {

            if (templateId !== 0) {
                return $http.get("/CaseProfileUIConfig/GetGesCaseProfileUiTemaplateDetails", { params: { id: templateId } });
            }
            return null;
        },
        CheckExistedTemplate: function (engagementTypesId, recomendationId) {
            return $http.get("/CaseProfileUIConfig/CheckExistedTemplate", { params: { engagementTypesId: engagementTypesId , recomendationId: recomendationId} });
        },
        GetAllActiveEngagementTypes: function () {
            return $http.get("/CaseProfileUIConfig/GetAllActiveEngagementType");
        },
        GetAllRecommendations: function () {
            return $http.get("/CaseProfileUIConfig/GetAllRecommendations");
        },
        GetAllGesCaseProfileEntities: function () {
            return $http.get("/CaseProfileUIConfig/GetAllGesCaseProfileEntities");
        },
        GetAllGesCaseProfileEntitiesClient: function () {
            return $http.get("/CaseProfileUIConfig/GetAllGesCaseProfileEntitiesClient");
        },
        UpdateCaseProfileUiTemplate: function (gesCaseProfileUiTemplate) {
            
            return $http({
                url: "/CaseProfileUIConfig/UpdateCaseProfileUiTemplate",
                method: "POST",
                data: gesCaseProfileUiTemplate
            });
        }

    }
}]);