"use strict";
GesInsideApp.factory("GesOrganizationService", ["$http", "$q", "$window", "Upload", function($http, $q, $window, Upload) {
    return {        
        SaveGesOrganization: function (organization, successCallback) {
            Upload.upload({
                url: "/Client/SaveGesOrganization",
                method: "POST",
                data: {
                    organization: organization
                }
            }).then(
                function (d) {
                    if (d.data.Status === "Success") {
                        quickNotification("Saved successfully");
                        successCallback();
                    } else {
                        quickNotification("Error occurred during saving GES organization", "error");
                    }
                }
            );
        },        
        DeleteGesOrganization: function (organizationId, successCallback) {
            $http({
                url: "/Client/DeleteGesOrganization",
                method: "POST",
                data: {
                    organizationId: organizationId
                }
            }).then(
                function (d) {
                    if (d.data.Status === "Success") {
                        quickNotification("Deleted GES organization successfully");
                        successCallback();
                    } else {
                        quickNotification("Can not delete GES organization", "error");
                    }
                }
            );
        }
    }
}]);