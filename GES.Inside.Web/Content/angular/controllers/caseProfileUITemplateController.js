"use strict";
GesInsideApp.controller("CaseProfileUITemplateController", ["$scope","$filter", "$timeout", "$window", "GesCaseProfileUITemplateService", function ($scope, $filter, $timeout, $window, gesCaseProfileUITemplateService) {
    $scope.isFormValid = false;
    $scope.message = null;
    $scope.gesTemplateDetails = null;
    $scope.engagementTypes = null;
    $scope.recommendations= null;
    $scope.entities = null;
    $scope.entitiesClient = null;
    $scope.invisibleEntities = null;
    $scope.invisibleEntitiesClient = null;
    $scope.validateMessage = [];
    $scope.isSaving = false;
    $scope.isAddNew = true;
    $scope.isFormInvalid = false;
    $scope.selection = {
        ids: {"SUMM": true, "GUIDELINES": true}
    };

    $scope.selectionClient = {
        ids: { "SUMM": true, "GUIDELINES": true }
    };

    init();

    function init() {
        initSelect2();

        var urlPath = $window.location.href;
        var urlPathSplit = String(urlPath).split("/");
        var templateId = 0;

        if (urlPathSplit !== null && urlPathSplit.length > 0) {
            if (urlPathSplit[urlPathSplit.length - 1] !== 'Add') {
                templateId = urlPathSplit[urlPathSplit.length - 1];
            } else{
                initEntities();
            }
        }

        initEngagementType();
        initRecommendation();
        
        
        if (templateId !== 0) {
            gesCaseProfileUITemplateService.GetCaseProfileUITemplateDetailsById(templateId).then(
                function(d) {
                    $scope.gesTemplateDetails = d.data;

                    if (($scope.gesTemplateDetails !== null)) {
                        $timeout(function () {
                            $("#engagementtype-select").val($scope.gesTemplateDetails.EngagementTypeId).trigger("change");
                            $("#recommendation-select").val($scope.gesTemplateDetails.RecomendationId).trigger("change");
                        }, 2500); //Add timeout to fix initialize value in minified code   
                        $scope.invisibleEntities = $filter('filter')($scope.gesTemplateDetails.ProfileInvisibleEntitiesViewModels, { 'InVisibleType': 1 }) 
                        $scope.invisibleEntitiesClient = $filter('filter')($scope.gesTemplateDetails.ProfileInvisibleEntitiesViewModels, { 'InVisibleType': 2 }) 
                        initEntities();                      

                    }                   

                },
                function() {
                    alert("Failed");
                }
            );
            $scope.isAddNew = false;
        }           

        initCancelSaveConfirmationBox();
    }

    function initSelect2() {
        $("#engagementtype-select").select2();
        $("#recommendation-select").select2();

    }

    function getEntityStatus(invisibleEntities, entityId) {

        if (invisibleEntities != null && invisibleEntities.length > 0) {
            for (var i = 0; i < invisibleEntities.length; i++) {                
                if (invisibleEntities[i].EntityCodeType === entityId ) {
                    return false;
                }
            }
        }
        return true;
    }
    
    function initEngagementType(){
        gesCaseProfileUITemplateService.GetAllActiveEngagementTypes().then(
            function(d) {
                $scope.engagementTypes = d.data;               
            },
            function() {
                alert("Failed");
            }
        );
    }

    function initRecommendation(){
        gesCaseProfileUITemplateService.GetAllRecommendations().then(
            function(d) {
                $scope.recommendations = d.data;
            },
            function() {
                alert("Failed");
            }
        );
    }

    function initEntities(){
        gesCaseProfileUITemplateService.GetAllGesCaseProfileEntities().then(
            function(d) {

                $scope.groupEntities = d.data;
                $scope.entities =[];

                if ($scope.groupEntities != null && $scope.groupEntities.length > 0) {
                    for (var i = 0; i < $scope.groupEntities.length; i++) {
                        if ($scope.groupEntities[i].CaseProfileEntities != null && $scope.groupEntities[i].CaseProfileEntities.length > 0) {
                            for (var j = 0; j < $scope.groupEntities[i].CaseProfileEntities.length; j++) {
                                $scope.entities.push($scope.groupEntities[i].CaseProfileEntities[j])
                            }
                        }                      
 
                    }
                }
                var obj = "{";
                if ($scope.entities != null && $scope.entities.length > 0) {
                    for (var k = 0; k < $scope.entities.length; k++) {

                        obj = obj + "\"" +  $scope.entities[k].id + "\": " + getEntityStatus($scope.invisibleEntities,$scope.entities[k].id);
                        if (k <  $scope.entities.length -1 ){
                            obj = obj  + ",";
                        }
                    }
                    obj = obj + "}";
                    $scope.selection = {ids:  angular.fromJson(obj) };
                }

            },
            function() {
                alert("Failed");
            }
        );

        //Client
        gesCaseProfileUITemplateService.GetAllGesCaseProfileEntitiesClient().then(
            function (d) {

                $scope.groupEntitiesClient = d.data;
                $scope.entitiesClient = [];

                if ($scope.groupEntitiesClient != null && $scope.groupEntitiesClient.length > 0) {
                    for (var i = 0; i < $scope.groupEntitiesClient.length; i++) {
                        if ($scope.groupEntitiesClient[i].CaseProfileEntities != null && $scope.groupEntitiesClient[i].CaseProfileEntities.length > 0) {
                            for (var j = 0; j < $scope.groupEntitiesClient[i].CaseProfileEntities.length; j++) {
                                $scope.entitiesClient.push($scope.groupEntitiesClient[i].CaseProfileEntities[j])
                            }
                        }

                    }
                }
                var obj = "{";
                if ($scope.entitiesClient != null && $scope.entitiesClient.length > 0) {
                    for (var k = 0; k < $scope.entitiesClient.length; k++) {

                        obj = obj + "\"" + $scope.entitiesClient[k].id + "\": " + getEntityStatus($scope.invisibleEntitiesClient, $scope.entitiesClient[k].id);
                        if (k < $scope.entitiesClient.length - 1) {
                            obj = obj + ",";
                        }
                    }
                    obj = obj + "}";
                    $scope.selectionClient = { ids: angular.fromJson(obj) };
                }

            },
            function () {
                alert("Failed");
            }
        );
    }

    
    $scope.UpdateCaseProfileUiTemplate = function () {           

        gesCaseProfileUITemplateService.CheckExistedTemplate($scope.gesTemplateDetails.EngagementTypeId, $scope.gesTemplateDetails.RecomendationId).then(
            function(d) {
               var status = d.data;
                if ($scope.isAddNew && status.existed ) {
                    var error = {
                        code: "Temp-Existed", text: status.message
                    };
                    $scope.validateMessage.push(error);
                    $scope.isFormInvalid = true;
                } else{
                    $scope.isSaving = true;
                    $scope.isFormInvalid = false;
                    var profileInvisibleEntitiesViewModels =[];
                    for (var i = 0; i < $scope.entities.length; i++) {

                        if (!$scope.selection.ids[$scope.entities[i].id]) {
                            var profileInvisibleEntitiesViewModel = {
                                EntityCodeType: $scope.entities[i].id,
                                EntityName: $scope.entities[i].name,
                                GesCaseProfileEntity_Id: $scope.entities[i].GesCaseProfileEntity_Id,
                                GesCaseProfileInvisibleEntity_Id: "",
                                GesCaseProfileTemplates_Id: "",
                                InVisibleType:1
                            };                            
                            profileInvisibleEntitiesViewModels.push(profileInvisibleEntitiesViewModel);
                        }
                    }

                    for (var j = 0; j < $scope.entitiesClient.length; j++) {

                        if (!$scope.selectionClient.ids[$scope.entitiesClient[j].id]) {
                            var profileInvisibleEntitiesViewModelClient = {
                                EntityCodeType: $scope.entitiesClient[j].id,
                                EntityName: $scope.entitiesClient[j].name,
                                GesCaseProfileEntity_Id: $scope.entitiesClient[j].GesCaseProfileEntity_Id,
                                GesCaseProfileInvisibleEntity_Id: "",
                                GesCaseProfileTemplates_Id: "",
                                InVisibleType: 2
                            };
                            profileInvisibleEntitiesViewModels.push(profileInvisibleEntitiesViewModelClient);
                        }
                    }

                    $scope.gesTemplateDetails.ProfileInvisibleEntitiesViewModels = profileInvisibleEntitiesViewModels;


                    gesCaseProfileUITemplateService.UpdateCaseProfileUiTemplate($scope.gesTemplateDetails).then(function () {
                        goToTemplateList();
                    });
                }
               
            },
            function() {
                
            }
        );        

    };

    function goToTemplateList() {
        $window.location.href = "/CaseProfileUIConfig/List";
    }

    String.isNullOrEmpty = function(value) {
        return !(typeof value === "undefined" || (typeof value === "string" && value.length > 0));
    };

    function initCancelSaveConfirmationBox() {
        $("#cancel-save").confirmModal({
            confirmCallback: goToTemplateList
        });
    }
}]);