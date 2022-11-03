"use strict";
GesInsideApp.controller("GesServicesController", ["$scope","$filter", "$timeout", "$window", "GesServicesService", function ($scope, $filter, $timeout, $window, gesServicesService) {
    $scope.isFormValid = false;
    $scope.message = null;
    $scope.gesServiceDetails = null;
    $scope.engagementTypes = null;
    $scope.gesServices = null;
    $scope.isSaving = false;
    $scope.isAddNew = true;

    init();

    function init() {
        initSelect2();

        var urlPath = $window.location.href;
        var urlPathSplit = String(urlPath).split("/");
        var serviceId = 0;

        if (urlPathSplit !== null && urlPathSplit.length > 0) {
            if (urlPathSplit[urlPathSplit.length - 1] !== 'Add') {
                serviceId = urlPathSplit[urlPathSplit.length - 1];
            }
        }
        
        if (serviceId !== 0) {
            gesServicesService.GesServiceDetailsById(serviceId).then(
                function(d) {
                    $scope.gesServiceDetails = d.data;

                    if (($scope.gesServiceDetails !== null)) {
                        $timeout(function () {
                            $("#engagementtype-select").val($scope.gesServiceDetails.EngagementTypesId).trigger("change");
                        }, 2500); //Add timeout to fix initialize value in minified code   
                    }                   

                },
                function() {
                    alert("Failed");
                }
            );
            $scope.isAddNew = false;
        }

        gesServicesService.GetDataForGesServices().then(
            function(d) {
                $scope.gesServices = d.data;
                initEngagementType();
            },
            function() {
                initEngagementType();
                alert("Failed");
            }
        );     
        
        $scope.$watch(function(){
            return $scope.gesServiceDetails != null && $scope.gesServiceDetails.Sort;
        }, function(newvalue, oldvalue){
            if ($scope.gesServiceDetails !== null && $scope.gesServiceDetails.Sort !== null && $scope.gesServiceDetails.Sort.match &&  $scope.gesServiceDetails.Sort.match(/^\d+$/) === null) {
                $scope.gesServiceDetails.Sort = oldvalue;
            }

        },true);
        initCancelSaveConfirmationBox();
    }

    function initSelect2() {
        $("#engagementtype-select").select2();

    }
    
    function initEngagementType(){
        gesServicesService.GetAllActiveEngagementTypes().then(
            function(d) {
                $scope.engagementTypes = d.data;

                var temp = $scope.engagementTypes;
                $scope.engagementTypes = [];

                if (!$scope.isAddNew) {
                    for (var i = 0; i < temp.length; i++) {
                        if (temp[i].I_EngagementTypes_Id === $scope.gesServiceDetails.EngagementTypesId){
                            $scope.engagementTypes.push(temp[i]);
                            break;
                        }                            
                    }
                }                
                
                if (temp.length > 0) {
                    for (var i = 0; i < temp.length; i++) {
                        var isExisted = false;
                        for (var j = 0; j < $scope.gesServices.length; j++) {
                            if ($scope.gesServices[j].EngagementTypesId != null && temp[i].I_EngagementTypes_Id.toString() === $scope.gesServices[j].EngagementTypesId.toString()) {
                                isExisted = true;
                                break;
                            }
                        }
                        if (!isExisted) {
                            if (temp[i].Deactive != null && temp[i].Deactive) {
                                temp[i].Name = temp[i].Name + " --- Closed";
                            }
                            $scope.engagementTypes.push(temp[i]);
                        }
                    }
                }
            },
            function() {
                alert("Failed");
            }
        );
    }

    $scope.UpdateGesService = function () {
        $scope.isSaving = true;
        gesServicesService.UpdateGesServiceData($scope.gesServiceDetails).then(function () {
            goToGesServicesList();
        });
    };

    function goToGesServicesList() {
        $window.location.href = "/GesServices/List";
    }

    String.isNullOrEmpty = function(value) {
        return !(typeof value === "undefined" || (typeof value === "string" && value.length > 0));
    };

    function initCancelSaveConfirmationBox() {
        $("#cancel-save").confirmModal({
            confirmCallback: goToGesServicesList
        });
    }
}]);