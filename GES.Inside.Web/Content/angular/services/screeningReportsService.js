"use strict";

GesInsideApp.factory("ScreeningReportsService", ["$http", "$q", "$window", "Upload", function ($http, $q, $window, Upload) {
    return {
        /**
         * @return {null}
         */
        GetClients: function(){
            return $http.get("/Export/GetClients");
        },
        GetPortfolioIndex: function(clientId){
            return $http.get("/Export/GetPortfolioIndexByClientId", { params: { clientId: clientId } });
        },
        Export: function (pram) {

            return $http({
                url: "/Export/ScreeningReportExport",
                method: "POST",
                data: pram
            });
        }

    }
}]);