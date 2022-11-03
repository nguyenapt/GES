"use strict";

GesInsideApp.factory("GesServicesService", ["$http", "$q", "$window", "Upload", function ($http, $q, $window, Upload) {
    return {
        /**
         * @return {null}
         */
        GesServiceDetailsById: function (serviceId) {

            if (serviceId !== 0) {
                return $http.get("/GesServices/GesServiceDetails", { params: { id: serviceId } });
            }
            return null;
        },
        GetAllActiveEngagementTypes: function () {
            return $http.get("/GesServices/GetAllActiveEngagementType");
        },

        GetDataForGesServices: function () {
            return $http.get("/GesServices/GetDataForGesServices");
        },
        UpdateGesServiceData: function (gesService) {
            
            return $http({
                url: "/GesServices/UpdateGesServicesService",
                method: "POST",
                data: gesService
            });
        }

    }
}]);