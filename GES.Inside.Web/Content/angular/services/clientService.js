"use strict";

GesInsideApp.factory("ClientService", ["$http", "$q", "$window", "Upload", function ($http, $q, $window, Upload) {
    return {
        /**
         * @return {null}
         */
        GetClientDetailsById: function (clientId) {

            if (clientId !== 0) {
                return $http.get("/Client/GetClientDetails", { params: { id: clientId } });
            }
            return null;
        },
        GetCountries: function(){
            return $http.get("/Client/GetCountries");
        },
        GetClientTypes: function(){
            return $http.get("/Client/GetIndustries");
        },
        GetServices: function(){
            return $http.get("/Client/GetServices");
        },        
        UpdateClientData: function (client) {
            
            return $http({
                url: "/Client/UpdateClient",
                method: "POST",
                data: client
            });
        }

    }
}]);