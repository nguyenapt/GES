"use strict";
GesInsideApp.controller("CompanyFTSEController", ["$scope", "$filter","$q", "$timeout", "$window", "CompanyService", "ModalService", "NgTableParams", function ($scope, $filter,$q, $timeout, $window, CompanyService, ModalService, NgTableParams) {
    $scope.ftses = null;
    $scope.ftsesSorted = null;
    $scope.isOpen = false;
    $scope.$on('finishInitCompany', function (e) {
        init();
    });

    function init() {
        GetAllFtses();
    }

    var GetAllFtses = function () {
        CompanyService.GetAllFtses().then(
            function (d) {
                $scope.ftses = d.data;
                $scope.ftsesSorted = listToTree(d.data);                
            },
            function () {
                quickNotification("Error occurred during loading ftses data", "error");
            }
        );        
    };

    $scope.getFtseName = function (ftse) {
        if (ftse != null && ftse != undefined && ftse.Name != null) {            
            return ftse.Name;
        }
        return "";
    }
    
}]);