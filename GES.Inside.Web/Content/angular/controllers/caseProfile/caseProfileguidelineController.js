"use strict";
GesInsideApp.controller("CaseProfileGuidelineController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, ModalService, NgTableParams) {
    $scope.editingGuideline = null;
    $scope.caseprofileGuidelines = [];
    $scope.caseprofileGuidelinesOriginal = [];

    $scope.template = '/Content/angular/templates/caseprofile/GuidelineDialog.html';
    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });

    $scope.addGuideline = function () {
        var temp = $scope.caseprofileGuidelines;
        $scope.caseprofileGuidelines = [];

        var guideline = {
            I_GesCaseReportsI_Norms_Id: 0,
            I_GesCaseReports_Id: CaseProfileService.caseProfile.I_GesCaseReports_Id,
            I_Norms_Id: "",
        };

        $scope.editingGuideline = guideline;

        $scope.caseprofileGuidelines.push(guideline);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.caseprofileGuidelines.push(temp[i]);
            }
        }

        $scope.caseprofileGuidelinesTableParams.reload();
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileGuidelineController');
    };

    $scope.deleteGuideline = function (guideline, frommodal) {
        ModalService.openConfirm("Are you sure to delete this guideline?", function (result) {
            if (result) {
                $scope.editingGuideline = guideline;
                if (frommodal) {
                    CaseProfileService.DeleteGuideline(guideline, deleteGuidelineFromModalCallback);
                }
                else {
                    CaseProfileService.DeleteGuideline(guideline, deleteGuidelineCallback);
                }
                ModalService.closeModal();
            }
        });
    };

    function deleteGuidelineCallback() {
        if ($scope.editingGuideline) {
            for (var i in $scope.caseprofileGuidelines) {
                if ($scope.caseprofileGuidelines[i].I_GesCaseReportsI_Norms_Id === $scope.editingGuideline.I_GesCaseReportsI_Norms_Id) {
                    $scope.caseprofileGuidelines.splice(i, 1);
                }
            }
            if ($scope.caseprofileGuidelinesTableParams.data.length === 1 && $scope.caseprofileGuidelinesTableParams.page() !== 1) {
                $scope.caseprofileGuidelinesTableParams.page($scope.caseprofileGuidelinesTableParams.page() - 1);
            }
        }

        $scope.caseprofileGuidelinesOriginal = $scope.caseprofileGuidelines;
        $scope.caseprofileGuidelinesTableParams.reload();
    }

    function deleteGuidelineFromModalCallback() {
        if ($scope.$parent.editingGuideline) {
            for (var i in $scope.$parent.caseprofileGuidelines) {
                if ($scope.$parent.caseprofileGuidelines[i].I_GesCaseReportsI_Norms_Id === $scope.$parent.editingGuideline.I_GesCaseReportsI_Norms_Id) {
                    $scope.$parent.caseprofileGuidelines.splice(i, 1);
                }
            }
            if ($scope.$parent.caseprofileGuidelinesTableParams.data.length === 1 && $scope.$parent.caseprofileGuidelinesTableParams.page() !== 1) {
                $scope.$parent.caseprofileGuidelinesTableParams.page($scope.$parent.caseprofileGuidelinesTableParams.page() - 1);
            }
        }

        $scope.$parent.caseprofileGuidelinesOriginal = $scope.$parent.caseprofileGuidelines;
        $scope.$parent.caseprofileGuidelinesTableParams.reload();
    }

    $scope.editGuideline = function (guideline) {
        if (guideline) {
            $scope.editingGuideline = guideline;
        }

        $scope.caseprofileGuidelinesTableParams.reload();
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileGuidelineController');
    };

    $scope.cancelGuideline = function (guideline) {
        if (guideline.I_GesCaseReportsI_Norms_Id === 0) {
            $scope.$parent.caseprofileGuidelines.splice(0, 1);
            $scope.$parent.caseprofileGuidelinesTableParams.reload();
        }
        else {
            $scope.$parent.caseprofileGuidelines = angular.copy($scope.$parent.caseprofileGuidelinesOriginal);
        }

        $scope.caseprofileGuidelinesTableParams.reload();
        ModalService.closeModal();
    }

    $scope.saveGuideline = function (guideline, isAddNew) {

        if (isAddNew) {
            CaseProfileService.SaveGuideline(guideline, saveAndAddNewGuidelineCallback);
        }
        else {
            CaseProfileService.SaveGuideline(guideline, saveGuidelineCallback);
        }
        ModalService.closeModal();
    }

    function saveGuidelineCallback(Id) {
        if ($scope.ngDialogData.editingGuideline.I_GesCaseReportsI_Norms_Id === 0) {
            $scope.ngDialogData.editingGuideline.I_GesCaseReportsI_Norms_Id = Id;
        }
        $scope.ngDialogData.caseprofileGuidelinesOriginal = angular.copy($scope.ngDialogData.caseprofileGuidelines);

        $scope.ngDialogData.caseprofileGuidelinesTableParams.reload();
    }

    function saveAndAddNewGuidelineCallback(Id, isAddNew) {
        if ($scope.ngDialogData.editingGuideline.I_GesCaseReportsI_Norms_Id === 0) {
            $scope.ngDialogData.editingGuideline.I_GesCaseReportsI_Norms_Id = Id;
        }

        $scope.ngDialogData.caseprofileGuidelinesOriginal = angular.copy($scope.ngDialogData.caseprofileGuidelines);

        $scope.ngDialogData.caseprofileGuidelinesTableParams.reload();

        $scope.ngDialogData.addGuideline();

    }

    function init() {

        $scope.caseprofileGuidelines = CaseProfileService.caseProfile.NormViewModels;

        if ($scope.caseprofileGuidelines != null) {
            $scope.caseprofileGuidelinesTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                    total: $scope.caseprofileGuidelines.length, // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.caseprofileGuidelines.length);
                        $scope.data = $scope.caseprofileGuidelines.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        return $scope.data;
                    }
                });
        }

        $scope.caseprofileGuidelinesOriginal = angular.copy($scope.caseprofileGuidelines);
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