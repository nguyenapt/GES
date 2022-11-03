"use strict";
GesInsideApp.controller("GuiderlineController", ["$scope", "$timeout", "$window", "GuiderlineService", "NgTableParams",function ($scope, $timeout, $window, guiderlineService, NgTableParams) {
    $scope.isFormValid = false;
    $scope.message = null;
    $scope.guiderlineDetails = null;
    $scope.categories = null;
    
    
    $scope.editingAgreement= null;
    $scope.isNewGuiderline= false;
    $scope.isSaving = false;
    $scope.isAddNew = false;

    init();


    function init() {
        initSelect2();
        var urlPath = $window.location.href;
        var urlPathSplit = String(urlPath).split("/");             
        
        var guiderlineId = 0;

        if (urlPathSplit !== null && urlPathSplit.length > 0) {
            var i = urlPathSplit[urlPathSplit.length - 1].indexOf('#');
            var s = urlPathSplit[urlPathSplit.length - 1];
            if (i !== -1) {
                s  = urlPathSplit[urlPathSplit.length - 1].substring(0, i); 
            }            
            
            if (s!== 'Add') {
                guiderlineId = s;
            } else{
                $scope.isAddNew = true;
            }
        }

        $scope.isNewGuiderline = urlPath.includes("/Add");

        if (guiderlineId !== 0) {
            guiderlineService.GetGuiderlineDetailsById(guiderlineId).then(
                function (d) {
                    $scope.guiderlineDetails = d.data;                         

                },
                function () {
                    alert("Failed");
                }
            );

        }
        
        
        initCancelSaveConfirmationBox();
        initDeleteConfirmationBox();
    }

    $scope.UpdateGuiderline = function () {
        $scope.isSaving = true;
        guiderlineService.UpdateGuiderlineData($scope.guiderlineDetails).then(function () {
            goToGuiderlinesList();
        });

    };  

    function initSelect2() {
    }



    function goToGuiderlinesList() {
        $window.location.href = "/Guiderlines/List";
    }

    String.isNullOrEmpty = function(value) {
        return !(typeof value === "undefined" || (typeof value === "string" && value.length > 0));
    };

    function initCancelSaveConfirmationBox() {
        $("#cancel-save").confirmModal({
            confirmCallback: goToGuiderlinesList
        });
    }

    function initDeleteConfirmationBox() {
        $("#delete-guiderline").confirmModal({
            confirmCallback: deleteConvention
        });
    }

    function deleteConvention() {
        var guiderlineId = $scope.guiderlineDetails.I_Norms_Id;
        guiderlineService.DeleteGuiderline(guiderlineId, goToGuiderlinesList);
    }    
}]);
