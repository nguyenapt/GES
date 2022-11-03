"use strict";
GesInsideApp.controller("CaseProfileSdgController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, NgTableParams) {
    $scope.selectedSdgs = [];
    $scope.sdgs = [];

    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });    

    $scope.toggleUpdateSdg = function (sdg) {
        if ($scope.selectedSdgs.indexOf(sdg) === -1) {
            $scope.selectedSdgs.push(sdg);
        } else {
            $scope.selectedSdgs.splice($scope.selectedSdgs.indexOf(sdg), 1);
        }

        $timeout(function () {
            updateSdgItemSize();
        }, 500);

        $scope.saveSdgs($scope.selectedSdgs);
    };

    $scope.removeSdg = function (sdg) {
        bootbox.confirm("Are you sure to delete this sdg of this case profile?", function (result) {
            if (result) {
                $scope.selectedSdgs.splice($scope.selectedSdgs.indexOf(sdg), 1);
                CaseProfileService.SaveSdgs(CaseProfileService.caseProfile.I_GesCaseReports_Id, $scope.selectedSdgs, null);                
            }
        });        
    };
    
    $scope.saveSdgs = function (sdgs) {
        CaseProfileService.SaveSdgs(CaseProfileService.caseProfile.I_GesCaseReports_Id, sdgs, null);        
    } 

    function init() {        
        initSortableSdg();
        updateSdgData();
    }

    function initSortableSdg() {
        $("#sortable").sortable();
        $("#sortable").disableSelection();

        $(window).resize(function () {
            $timeout(
                updateSdgItemSize(),
                500
            );
        });
    }

    function updateSdgItemSize() {
        var containerWidth = $("#sortable").parent().width() - 1;
        var itemSize = Math.floor(containerWidth / 4) - 3;
        var $sortableItems = $("#sortable li");
        $sortableItems.removeClass("relative-width");
        $sortableItems.css({ "width": itemSize, "height": itemSize });
        $("#sortable li img").each(function () {
            $(this).css({
                "width": "auto",
                "height": "auto",
                "maxWidth": itemSize,
                "maxHeight": itemSize
            });
        });
    }

    function updateSdgData() {
        CaseProfileService.GetSdgs().then(
            function (response) {
                $scope.sdgs = response.data;

                if ($scope.caseProfile.SdgIds != null && $scope.caseProfile.SdgIds.length > 0) {
                    for (var i = 0; i < $scope.caseProfile.SdgIds.length; i++) {
                        $scope.selectedSdgs.push($scope.sdgs.filter(x => x.Sdg_Id === $scope.caseProfile.SdgIds[i])[0]);
                    }

                    $timeout(function () {
                        updateSdgItemSize();
                    }, 500);
                }
            },
            function () {
                quickNotification("Error occurred during loading SDGs", "error");
            }
        );
    }
}]);