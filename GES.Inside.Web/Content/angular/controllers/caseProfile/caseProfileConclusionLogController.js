"use strict";
GesInsideApp.controller("caseProfileConclusionLogController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, ModalService, NgTableParams) {
    $scope.conclusionLogs = [];
    $scope.template = '/Content/angular/templates/caseprofile/ConclusionLogDialog.html';
    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();
    $scope.editingConclusion = null;
    $scope.conclusionTableParams = [];
    $scope.conclusionLogsOriginal = [];

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });


    $scope.viewConclusionForm = function () {
        $scope.GetConclusionLogsByCaseReportId($scope.caseProfile.I_GesCaseReports_Id);
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'caseProfileConclusionLogController');
    };

    $scope.GetConclusionLogsByCaseReportId = function (gesCaseReportId) {

        if (gesCaseReportId === 0)
            return;

        CaseProfileService.GetConclusionLogsByCaseReportId(gesCaseReportId).then(
            function (response) {
                $scope.conclusionLogsOriginal = response.data;

                if ($scope.conclusionLogsOriginal != null && $scope.conclusionLogsOriginal.length > 0) {
                    for (var i = 0; i < $scope.conclusionLogsOriginal.length; i++) {

                        if (($scope.conclusionLogsOriginal[i].AuditDatetime != null)) {
                            $scope.conclusionLogsOriginal[i].AuditDatetime =
                                new Date($scope.convertDate($scope.conclusionLogsOriginal[i].AuditDatetime, 'yyyy/MM/dd'));
                        }
                    }
                }

                $scope.conclusionLogs = angular.copy($scope.conclusionLogsOriginal);
            },
            function () {
                quickNotification("Error occurred during loading conclusions data", "error");
            }
        );

    };

    $scope.cancelConclusionLog = function () {        
        ModalService.closeModal();        
    }

    $scope.cancelEditConclusionLog = function () {        
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

    $scope.GetConclusionLogs = function (caseReportId) {
        CaseProfileService.GetConclusionLogsByCaseReportId(caseReportId).then(
            function (response) {
                $scope.conclusionLogsOriginal = response.data;

                if ($scope.conclusionLogsOriginal != null && $scope.conclusionLogsOriginal.length > 0) {
                    for (var i = 0; i < $scope.conclusionLogsOriginal.length; i++) {

                        if (($scope.conclusionLogsOriginal[i].AuditDatetime != null)) {
                            $scope.conclusionLogsOriginal[i].AuditDatetime =
                                new Date($scope.convertDate($scope.conclusionLogsOriginal[i].AuditDatetime, 'yyyy/MM/dd'));
                        }
                    }
                }

                $scope.conclusionLogs = angular.copy($scope.conclusionLogsOriginal);

            },
            function () {
                quickNotification("Error occurred during loading conclusions data", "error");
            }
        );
    };

    $scope.deleteConclusionLog = function (index) {
        var confirmed = confirm("Are you sure to delete this conclusion history?");
        if (confirmed == true) {
            $scope.editingConclusion = index;

            CaseProfileService.DeleteConclusionLog(index, deleteConclusionCallback);
        };
    };

    function deleteConclusionCallback() {
        if ($scope.editingConclusion) {
            for (var i in $scope.ngDialogData.conclusionLogs) {
                if ($scope.ngDialogData.conclusionLogs[i].Id === $scope.editingConclusion.Id) {
                    $scope.ngDialogData.conclusionLogs.splice(i, 1);
                }
            }
        }
    }

    $scope.saveConclusionLogs = function () {
        CaseProfileService.SaveConclusionLogs($scope.caseProfile.I_GesCaseReports_Id, $scope.ngDialogData.conclusionLogs, saveConclusionCallback);
    }

    function saveConclusionCallback() {        
        ModalService.closeModal();
    }

}]);