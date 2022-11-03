"use strict";
GesInsideApp.controller("CompanyMSCIController", ["$scope", "$filter","$q", "$timeout", "$window", "CompanyService", "ModalService", "NgTableParams", function ($scope, $filter,$q, $timeout, $window, CompanyService, ModalService, NgTableParams) {
    $scope.mscis = null;
    $scope.mscisSorted = null;
    $scope.isOpen = false;
    $scope.$on('finishInitCompany', function (e) {
        init();
    });

    function init() {
        GetAllMscis();
    }

    var GetAllMscis = function () {
        CompanyService.GetAllMscis().then(
            function (d) {
                $scope.mscis = d.data;
                $scope.mscisSorted = listToTree(d.data);                
            },
            function () {
                quickNotification("Error occurred during loading mscis data", "error");
            }
        );        
    };
    $scope.getMsciName = function (msci) {
        if (msci != null && msci!=undefined && msci.Name != null) {
            return msci.Name;
        }
        return "";
    }
}]);