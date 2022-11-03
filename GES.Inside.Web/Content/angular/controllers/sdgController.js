"use strict";
GesInsideApp.controller("SdgController", ["$scope", "$timeout", "SdgService", function ($scope, $timeout, SdgService) {
    $scope.sdgs = null;
    $scope.editingSdg = null;
    $scope.file = null;
    $scope.invalidFiles = null;

    init();

    $scope.addNewSdg = function () {
        $scope.editingSdg = null;
        $scope.file = null;
        $scope.sdgDialogForm.$setPristine();
        $scope.sdgDialogForm.$setUntouched();
        $("#sdg-dialog").modal("show");
    }

    $scope.changeFile = function (file) {
        if (file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $("#sdg-img").attr("src", e.target.result);
            }
            reader.readAsDataURL(file);
        }
    }
    
    $scope.getSdgById = function(sdgId) {
        SdgService.GetSdgById(sdgId).then(
            function (response) {
                $scope.file = null;
                $scope.editingSdg = response.data;
            },
            function () {
                quickNotification("Error occurred during loading SDG", "error");
            }
        );
    }

    $scope.saveSdg = function () {
        SdgService.SaveSdg($scope.editingSdg, $scope.file, loadSdgs);
    }

    $scope.deleteSdg = function(target) {
        var sdgId = Number(target.attr("data-id"));
        SdgService.DeleteSdg(sdgId, loadSdgs);
    }

    function init() {
        loadSdgs();
    }

    function loadSdgs() {
        SdgService.GetSdgs().then(
            function(d) {
                $scope.sdgs = d.data;
                $timeout(function () {
                    $(".icon-remove").confirmModal({
                        confirmCallback: $scope.deleteSdg
                    });
                });
            },
            function () {
                quickNotification("Error occurred during loading SDGs", "error");
            }
        );
    }
}]);