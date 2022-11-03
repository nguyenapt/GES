"use strict";
GesInsideApp.controller("caseProfileMileStoneController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, ModalService, NgTableParams) {
    $scope.mileStonesOriginal = [];
    $scope.mileStones = [];
    $scope.gesMilestoneTypes = [];
    $scope.template = '/Content/angular/templates/caseprofile/MileStoneDialog.html';
    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();
    $scope.editingMilestone = null;

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });

    $scope.addMilestone = function () {
        var milestone = {
            MilestoneId: 0,
            CaseReportId: CaseProfileService.caseProfile.I_GesCaseReports_Id,
            MilestoneModified: "",
            MilestoneDescription: "",
            GesMilestoneTypesId: ""
        };

        $scope.editingMilestone = milestone;

        $scope.mileStones.push(milestone);

        CaseProfileService.caseProfile.MileStoneModel = $scope.mileStones;

        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'caseProfileMileStoneController');
    };

    $scope.editMilestone = function (milestone) {
        if (milestone) {
            $scope.editingMilestone = milestone;
        }
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'caseProfileMileStoneController');
    };

    $scope.GetMilestonesByCaseReportId = function (gesCaseReportId) {

        if (gesCaseReportId === 0)
            return;

        CaseProfileService.GetMilestonesByCaseReportId(gesCaseReportId).then(
            function (response) {
                $scope.mileStonesOriginal = response.data;

                if ($scope.mileStonesOriginal != null && $scope.mileStonesOriginal.length > 0) {
                    for (var i = 0; i < $scope.mileStonesOriginal.length; i++) {

                        if (($scope.mileStonesOriginal[i].MilestoneModified != null)) {
                            $scope.mileStonesOriginal[i].MilestoneModified = new Date(convertDate($scope.mileStonesOriginal[i].MilestoneModified, 'yyyy-MM-dd'));
                        }
                    }
                }

                $scope.mileStones = angular.copy($scope.mileStonesOriginal);
            },
            function () {
                quickNotification("Error occurred during loading milestone data", "error");
            }
        );
    };

    $scope.cancelMilestoneDialog = function (milestone) {

        if (milestone.I_Milestones_Id === 0) {
            $scope.$parent.mileStones.splice(0, 1);
        }
        else {
            $scope.$parent.mileStones = angular.copy($scope.$parent.mileStonesOriginal);
        }
        ModalService.closeModal();
    }

    String.isNullOrEmpty = function (value) {
        return !(typeof value === "undefined" || (typeof value === "string" && value.length > 0));
    };

    function convertDate(value, format) {
        if (value != null && !value.isNullOrEmpty) {
            return $filter("date")(new Date(parseInt(value.substr(6))), format);
        }

        return null;
    };

    function init() {
        $scope.GetMilestonesByCaseReportId($scope.caseProfile.I_GesCaseReports_Id);
        CaseProfileService.GetMilestoneTypes().then(
            function (d) {
                $scope.gesMilestoneTypes = d.data;
            },
            function () {
                alert("Failed");
            }
        );
    }

    $scope.deleteMileStone = function (index) {
        ModalService.openConfirm("Are you sure to delete this milestone?", function (result) {
            if (result) {
                $scope.editingMilestone = index;

                CaseProfileService.DeleteMilestone(index.MilestoneId, deleteMilestoneCallback);
                ModalService.closeModal();
            }
        });

    };

    function deleteMilestoneCallback() {
        if ($scope.editingMilestone) {
            for (var i in $scope.mileStones) {
                if ($scope.mileStones[i].MilestoneId === $scope.editingMilestone.MilestoneId) {
                    $scope.mileStones.splice(i, 1);
                }
            }
        }
        CaseProfileService.caseProfile.MileStoneModel = $scope.mileStones;
    }

    $scope.saveMileStone = function (milestone) {
        CaseProfileService.SaveMilestone(milestone, saveMilestoneCallback);
    }

    function saveMilestoneCallback() {
        $scope.$parent.GetMilestonesByCaseReportId($scope.caseProfile.I_GesCaseReports_Id);
        ModalService.closeModal();
    }

}]);