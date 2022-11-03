"use strict";
GesInsideApp.controller("CaseProfileConventionController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, ModalService, NgTableParams) {
    $scope.editingConvention = null;
    $scope.caseprofileConventions = [];
    $scope.caseprofileConventionsOriginal = [];

    $scope.template = '/Content/angular/templates/caseprofile/ConventionDialog.html';
    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });

    $scope.addConvention = function () {
        var temp = $scope.caseprofileConventions;
        $scope.caseprofileConventions = [];

        var convention = {
            I_GesCaseReportsI_Conventions_Id: 0,
            I_GesCaseReports_Id: CaseProfileService.caseProfile.I_GesCaseReports_Id,
            I_Conventions_Id: "",
        };

        $scope.editingConvention = convention;

        $scope.caseprofileConventions.push(convention);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.caseprofileConventions.push(temp[i]);
            }
        }

        $scope.caseprofileConventionsTableParams.reload();
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileConventionController');
    };

    $scope.deleteConvention = function (convention, frommodal) {
        ModalService.openConfirm("Are you sure to delete this convention?", function (result) {
            if (result) {
                $scope.editingConvention = convention;
                if (frommodal) {
                    CaseProfileService.DeleteConvention(convention, deleteConventionFromModalCallback);
                }
                else {
                    CaseProfileService.DeleteConvention(convention, deleteConventionCallback);
                }
                ModalService.closeModal();
            }
        });
    };

    function deleteConventionCallback() {
        if ($scope.editingConvention) {
            for (var i in $scope.caseprofileConventions) {
                if ($scope.caseprofileConventions[i].I_GesCaseReportsI_Conventions_Id === $scope.editingConvention.I_GesCaseReportsI_Conventions_Id) {
                    $scope.caseprofileConventions.splice(i, 1);
                }
            }
            if ($scope.caseprofileConventionsTableParams.data.length === 1 && $scope.caseprofileConventionsTableParams.page() !== 1) {
                $scope.caseprofileConventionsTableParams.page($scope.caseprofileConventionsTableParams.page() - 1);
            }
        }

        $scope.caseprofileConventionsOriginal = $scope.caseprofileConventions;
        $scope.caseprofileConventionsTableParams.reload();
    }

    function deleteConventionFromModalCallback() {
        if ($scope.$parent.editingConvention) {
            for (var i in $scope.$parent.caseprofileConventions) {
                if ($scope.$parent.caseprofileConventions[i].I_GesCaseReportsI_Conventions_Id === $scope.$parent.editingConvention.I_GesCaseReportsI_Conventions_Id) {
                    $scope.$parent.caseprofileConventions.splice(i, 1);
                }
            }
            if ($scope.$parent.caseprofileConventionsTableParams.data.length === 1 && $scope.$parent.caseprofileConventionsTableParams.page() !== 1) {
                $scope.$parent.caseprofileConventionsTableParams.page($scope.$parent.caseprofileConventionsTableParams.page() - 1);
            }
        }

        $scope.$parent.caseprofileConventionsOriginal = $scope.$parent.caseprofileConventions;
        $scope.$parent.caseprofileConventionsTableParams.reload();
    }

    $scope.editConvention = function (convention) {
        if (convention) {
            $scope.editingConvention = convention;
        }

        $scope.caseprofileConventionsTableParams.reload();
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileConventionController');
    };

    $scope.cancelConvention = function (convention) {
        if (convention.I_GesCaseReportsI_Conventions_Id === 0) {
            $scope.$parent.caseprofileConventions.splice(0, 1);
            $scope.$parent.caseprofileConventionsTableParams.reload();
        }
        else {
            $scope.$parent.caseprofileConventions = angular.copy($scope.$parent.caseprofileConventionsOriginal);
        }

        $scope.caseprofileConventionsTableParams.reload();
        ModalService.closeModal();
    }

    $scope.saveConvention = function (convention, isAddNew) {
        if (isAddNew) {
            CaseProfileService.SaveConvention(convention, saveAndAddNewConventionCallback);
        }
        else {
            CaseProfileService.SaveConvention(convention, saveConventionCallback);
        }
        ModalService.closeModal();
    }

    function saveConventionCallback(Id) {
        if ($scope.ngDialogData.editingConvention.I_GesCaseReportsI_Conventions_Id === 0) {
            $scope.ngDialogData.editingConvention.I_GesCaseReportsI_Conventions_Id = Id;
        }
        $scope.ngDialogData.caseprofileConventionsOriginal = angular.copy($scope.ngDialogData.caseprofileConventions);

        $scope.ngDialogData.caseprofileConventionsTableParams.reload();
    }

    function saveAndAddNewConventionCallback(Id, isAddNew) {
        if ($scope.ngDialogData.editingConvention.I_GesCaseReportsI_Conventions_Id === 0) {
            $scope.ngDialogData.editingConvention.I_GesCaseReportsI_Conventions_Id = Id;
        }

        $scope.ngDialogData.caseprofileConventionsOriginal = angular.copy($scope.ngDialogData.caseprofileConventions);

        $scope.ngDialogData.caseprofileConventionsTableParams.reload();

        $scope.ngDialogData.addConvention();

    }

    function init() {

        $scope.caseprofileConventions = CaseProfileService.caseProfile.ConventionsViewModels;

        if ($scope.caseprofileConventions != null) {
            $scope.caseprofileConventionsTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                    total: ($scope.caseprofileConventions != null ? $scope.caseprofileConventions.length : 0), // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.caseprofileConventions != null ? $scope.caseprofileConventions.length : 0);
                        $scope.data = $scope.caseprofileConventions.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        return $scope.data;
                    }
                });
        }

        $scope.caseprofileConventionsOriginal = angular.copy($scope.caseprofileConventions);
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
}]);