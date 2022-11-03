"use strict";
GesInsideApp.controller("caseProfileRecommendationLogController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, ModalService, NgTableParams) {
    $scope.recommendationLogs = [];
    $scope.template = '/Content/angular/templates/caseprofile/RecommendationLogDialog.html';
    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();
    $scope.editingRecommendation = null;
    $scope.recommendationTableParams = [];
    $scope.recommendationLogsOriginal = [];

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });


    $scope.viewRecommendationForm = function () {
        $scope.GetRecommendationLogsByCaseReportId($scope.caseProfile.I_GesCaseReports_Id);
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'caseProfileRecommendationLogController');
    };

    $scope.GetRecommendationLogsByCaseReportId = function (gesCaseReportId) {

        if (gesCaseReportId === 0)
            return;

        CaseProfileService.GetRecommendationLogsByCaseReportId(gesCaseReportId).then(
            function (response) {
                $scope.recommendationLogsOriginal = response.data;

                if ($scope.recommendationLogsOriginal != null && $scope.recommendationLogsOriginal.length > 0) {
                    for (var i = 0; i < $scope.recommendationLogsOriginal.length; i++) {

                        if (($scope.recommendationLogsOriginal[i].AuditDatetime != null)) {
                            $scope.recommendationLogsOriginal[i].AuditDatetime =
                                new Date($scope.convertDate($scope.recommendationLogsOriginal[i].AuditDatetime, 'yyyy/MM/dd'));
                        }
                    }
                }

                $scope.recommendationLogs = angular.copy($scope.recommendationLogsOriginal);
            },
            function () {
                quickNotification("Error occurred during loading recommendations data", "error");
            }
        );

    };

    $scope.cancelRecommendationLog = function () {
        ModalService.closeModal();
    }

    $scope.cancelEditRecommendationLog = function () {
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

    }

    $scope.GetRecommendationLogs = function (caseReportId) {
        CaseProfileService.GetRecommendationLogsByCaseReportId(caseReportId).then(
            function (response) {
                $scope.recommendationLogsOriginal = response.data;

                if ($scope.recommendationLogsOriginal != null && $scope.recommendationLogsOriginal.length > 0) {
                    for (var i = 0; i < $scope.recommendationLogsOriginal.length; i++) {

                        if (($scope.recommendationLogsOriginal[i].AuditDatetime != null)) {
                            $scope.recommendationLogsOriginal[i].AuditDatetime =
                                new Date($scope.convertDate($scope.recommendationLogsOriginal[i].AuditDatetime, 'yyyy/MM/dd'));
                        }
                    }
                }

                $scope.recommendationLogs = angular.copy($scope.recommendationLogsOriginal);

            },
            function () {
                quickNotification("Error occurred during loading recommendations data", "error");
            }
        );
    };

    $scope.deleteRecommendationLog = function (index) {
        var confirmed = confirm("Are you sure to delete this recommendation history?");
        if (confirmed == true) {
            $scope.editingRecommendation = index;

            CaseProfileService.DeleteRecommendationLog(index, deleteRecommendationCallback);
        };
    };

    function deleteRecommendationCallback() {
        if ($scope.editingRecommendation) {
            for (var i in $scope.ngDialogData.recommendationLogs) {
                if ($scope.ngDialogData.recommendationLogs[i].Id === $scope.editingRecommendation.Id) {
                    $scope.ngDialogData.recommendationLogs.splice(i, 1);
                }
            }
        }
    }

    $scope.saveRecommendationLogs = function () {
        CaseProfileService.SaveRecommendationLogs($scope.caseProfile.I_GesCaseReports_Id, $scope.ngDialogData.recommendationLogs, saveRecommendationCallback);
    }

    function saveRecommendationCallback() {
        ModalService.closeModal();
    }

}]);