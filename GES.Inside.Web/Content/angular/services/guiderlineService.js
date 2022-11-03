"use strict";

GesInsideApp.factory("GuiderlineService", ["$http", "$q", "$window", "Upload", function ($http, $q, $window, Upload) {
    return {
        /**
         * @return {null}
         */
        GetGuiderlineDetailsById: function (guiderlineId) {

            if (guiderlineId !== 0) {
                return $http.get("/Guiderlines/GetConventionDetails", { params: { id: guiderlineId } });
            }
            return null;
        },

        UpdateGuiderlineData: function (guiderline) {            
            return $http({
                url: "/Guiderlines/UpdateGuiderline",
                method: "POST",
                data: guiderline
            });
        },
        DeleteGuiderline: function (guiderlineId, successCallback) {
            $http({
                url: "/Guiderlines/DeleteGuiderline",
                method: "POST",
                data: {
                    guiderlineId: guiderlineId
                }
            }).then(
                function (d) {
                    if (d.data.Status === "Success") {
                        quickNotification("Deleted Guiderline successfully");
                        successCallback();
                    } else {
                        quickNotification("Can not delete Guiderline", "error");
                    }
                }
            );
        }

    }
}]);