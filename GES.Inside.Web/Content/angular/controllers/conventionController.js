"use strict";
GesInsideApp.controller("ConventionController", ["$scope", "$timeout", "$window", "ConventionService", "NgTableParams", "textAngularManager",function ($scope, $timeout, $window, conventionService, NgTableParams, textAngularManager) {
    $scope.isFormValid = false;
    $scope.message = null;
    $scope.conventionDetails = null;
    $scope.categories = null;
    
    
    $scope.editingAgreement= null;
    $scope.isNewConvention = false;
    $scope.isSaving = false;
    $scope.isAddNew = false;
    $scope.isAddNewService = true;
    $scope.isExistedService = false;
    $scope.tinymceOptions = {
        resize: false,
        width: 400,  // I *think* its a number and not '400' string
        height: 300,
        plugins: 'print textcolor',
        toolbar: "undo redo styleselect bold italic print forecolor backcolor"

    };
    init();


    function init() {
        initSelect2();
        var urlPath = $window.location.href;
        var urlPathSplit = String(urlPath).split("/");             
        
        var conventionId = 0;

        if (urlPathSplit !== null && urlPathSplit.length > 0) {
            var i = urlPathSplit[urlPathSplit.length - 1].indexOf('#');
            var s = urlPathSplit[urlPathSplit.length - 1];
            if (i !== -1) {
                s  = urlPathSplit[urlPathSplit.length - 1].substring(0, i); 
            }            
            
            if (s!== 'Add') {
                conventionId = s;
            } else {
                $scope.isAddNew = true;
            }
        }

        $scope.isNewConvention = urlPath.includes("/Add");        

        conventionService.GetCatalogues().then(
            function (d) {
                $scope.categories = d.data;


                if (conventionId !== 0) {
                    conventionService.GetConventionDetailsById(conventionId).then(
                        function (d) {
                            $scope.conventionDetails = d.data;

                            $timeout(function () {
                                $("#category-select").val($scope.conventionDetails.I_ConventionCategories_Id).trigger("change");
                                $scope.conventionForm.$setPristine();

                                $scope.edited = true;


                            }, 100); //Add timeout to fix initialize value in minified code                            

                        },
                        function () {
                            alert("Failed");
                        }
                    );
                    
                }               
            },
            function () {
                alert("Failed");
            }
        );
        initCancelSaveConfirmationBox();
        initDeleteConfirmationBox();
    }

    $scope.UpdateConvention = function () {
        $scope.isSaving = true;
        conventionService.UpdateConventionData($scope.conventionDetails).then(function () {
            goToConventionList();
        });

    };  

    function initSelect2() {
    }



    function goToConventionList() {
        $window.location.href = "/Convention/List";
    }

    String.isNullOrEmpty = function(value) {
        return !(typeof value === "undefined" || (typeof value === "string" && value.length > 0));
    };

    function initCancelSaveConfirmationBox() {
        $("#cancel-save").confirmModal({
            confirmCallback: goToConventionList
        });
    }

    function initDeleteConfirmationBox() {
        $("#delete-convention").confirmModal({
            confirmCallback: deleteConvention
        });
    }

    function deleteConvention() {
        var conventionId = $scope.conventionDetails.I_Conventions_Id;
        conventionService.DeleteConvention(conventionId, goToConventionList);
    }    
}]);
