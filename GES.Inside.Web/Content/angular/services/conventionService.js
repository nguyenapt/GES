"use strict";

GesInsideApp.factory("ConventionService", ["$http", "$q", "$window", "Upload", function ($http, $q, $window, Upload) {
    return {
        /**
         * @return {null}
         */
        GetConventionDetailsById: function (conventionId) {

            if (conventionId !== 0) {
                return $http.get("/Convention/GetConventionDetails", { params: { id: conventionId } });
            }
            return null;
        },
        GetCatalogues: function(){
            return $http.get("/Convention/GetCatalogues");
        },        
        UpdateConventionData: function (convention) {            
            return $http({
                url: "/Convention/UpdateConvention",
                method: "POST",
                data: convention
            });
        },
        DeleteConvention: function (conventionId, successCallback) {
            $http({
                url: "/Convention/DeleteConvention",
                method: "POST",
                data: {
                    conventionId: conventionId
                }
            }).then(
                function (d) {
                    if (d.data.Status === "Success") {
                        quickNotification("Deleted Convention successfully");
                        successCallback();
                    } else {
                        quickNotification("Can not delete Convention", "error");
                    }
                }
            );
        }

    }
}]);