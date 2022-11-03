"use strict";
GesInsideApp.controller("CaseProfileAssociatedCorporationsController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, ModalService, NgTableParams) {
    $scope.editingAssociatedCorporation = null;
    $scope.caseprofileAssociatedCorporations = [];
    $scope.caseprofileAssociatedCorporationsOriginal = [];

    $scope.template = '/Content/angular/templates/caseprofile/AssociatedCorporationDialog.html';
    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });

    $scope.addAssociatedCorporation = function () {
        var temp = $scope.caseprofileAssociatedCorporations;
        $scope.caseprofileAssociatedCorporations = [];

        var associatedCorporation = {
            AssociatedCorporationId: 0,
            CaseReportId: CaseProfileService.caseProfile.I_GesCaseReports_Id,
            Name: "",
            Ownership: "",
            Comment: "",
            Traded: false,
            ShowInReport: false
        };

        $scope.editingAssociatedCorporation = associatedCorporation;

        $scope.caseprofileAssociatedCorporations.push(associatedCorporation);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.caseprofileAssociatedCorporations.push(temp[i]);
            }
        }

        $scope.caseprofileAssociatedCorporationsTableParams.reload();        
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileAssociatedCorporationsController');        
    };

    $scope.deleteAssociatedCorporation = function (associatedCorporation, frommodal) {
        ModalService.openConfirm("Are you sure to delete this associatedCorporation?", function (result) {
            if (result) {
                $scope.editingAssociatedCorporation = associatedCorporation;
                if (frommodal) {
                    CaseProfileService.DeleteAssociatedCorporation(associatedCorporation, deleteAssociatedCorporationFromModalCallback);
                }
                else {
                    CaseProfileService.DeleteAssociatedCorporation(associatedCorporation, deleteAssociatedCorporationCallback);
                }                
                ModalService.closeModal();                
            }
        });
    };

    function deleteAssociatedCorporationCallback() {
        if ($scope.editingAssociatedCorporation) {
            for (var i in $scope.caseprofileAssociatedCorporations) {
                if ($scope.caseprofileAssociatedCorporations[i].AssociatedCorporationId === $scope.editingAssociatedCorporation.AssociatedCorporationId) {
                    $scope.caseprofileAssociatedCorporations.splice(i, 1);
                }
            }
            if ($scope.caseprofileAssociatedCorporationsTableParams.data.length === 1 && $scope.caseprofileAssociatedCorporationsTableParams.page() !== 1) {
                $scope.caseprofileAssociatedCorporationsTableParams.page($scope.caseprofileAssociatedCorporationsTableParams.page() - 1);
            }
        }

        $scope.caseprofileAssociatedCorporationsOriginal = $scope.caseprofileAssociatedCorporations;
        $scope.caseprofileAssociatedCorporationsTableParams.reload();
    }

    function deleteAssociatedCorporationFromModalCallback() {
        if ($scope.$parent.editingAssociatedCorporation) {
            for (var i in $scope.$parent.caseprofileAssociatedCorporations) {
                if ($scope.$parent.caseprofileAssociatedCorporations[i].AssociatedCorporationId === $scope.$parent.editingAssociatedCorporation.AssociatedCorporationId) {
                    $scope.$parent.caseprofileAssociatedCorporations.splice(i, 1);
                }
            }
            if ($scope.$parent.caseprofileAssociatedCorporationsTableParams.data.length === 1 && $scope.$parent.caseprofileAssociatedCorporationsTableParams.page() !== 1) {
                $scope.$parent.caseprofileAssociatedCorporationsTableParams.page($scope.$parent.caseprofileAssociatedCorporationsTableParams.page() - 1);
            }
        }

        $scope.$parent.caseprofileAssociatedCorporationsOriginal = $scope.$parent.caseprofileAssociatedCorporations;
        $scope.$parent.caseprofileAssociatedCorporationsTableParams.reload();
    }

    $scope.editAssociatedCorporation = function (associatedCorporation) {
        if (associatedCorporation) {
            $scope.editingAssociatedCorporation = associatedCorporation;
        }

        $scope.caseprofileAssociatedCorporationsTableParams.reload();
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileAssociatedCorporationsController');
    };

    $scope.cancelAssociatedCorporation = function (associatedCorporation) {
        if (associatedCorporation.AssociatedCorporationId === 0) {
            $scope.$parent.caseprofileAssociatedCorporations.splice(0, 1);
            $scope.$parent.caseprofileAssociatedCorporationsTableParams.reload();
        }
        else {
            $scope.$parent.caseprofileAssociatedCorporations = angular.copy($scope.$parent.caseprofileAssociatedCorporationsOriginal);
        }

        $scope.caseprofileAssociatedCorporationsTableParams.reload();        
        ModalService.closeModal();        
    }

    $scope.saveAssociatedCorporation = function (associatedCorporation, isAddNew) {
        if (isAddNew) {
            CaseProfileService.SaveAssociatedCorporation(associatedCorporation, saveAndAddNewAssociatedCorporationCallback);
        }
        else {
            CaseProfileService.SaveAssociatedCorporation(associatedCorporation, saveAssociatedCorporationCallback);
        }
        ModalService.closeModal();        
    }

    function saveAssociatedCorporationCallback(Id) {
        if ($scope.ngDialogData.editingAssociatedCorporation.AssociatedCorporationId === 0) {
            $scope.ngDialogData.editingAssociatedCorporation.AssociatedCorporationId = Id;
        }
        $scope.ngDialogData.caseprofileAssociatedCorporationsOriginal = angular.copy($scope.ngDialogData.caseprofileAssociatedCorporations);

        $scope.ngDialogData.caseprofileAssociatedCorporationsTableParams.reload();
    }

    function saveAndAddNewAssociatedCorporationCallback(Id, isAddNew) {
        if ($scope.ngDialogData.editingAssociatedCorporation.AssociatedCorporationId === 0) {
            $scope.ngDialogData.editingAssociatedCorporation.AssociatedCorporationId = Id;
        }

        $scope.ngDialogData.caseprofileAssociatedCorporationsOriginal = angular.copy($scope.ngDialogData.caseprofileAssociatedCorporations);

        $scope.ngDialogData.caseprofileAssociatedCorporationsTableParams.reload();

        $scope.ngDialogData.addAssociatedCorporation();

    }

    function init() {

        $scope.caseprofileAssociatedCorporations = CaseProfileService.caseProfile.AssociatedCorporations;

        if ($scope.caseprofileAssociatedCorporations != null) {
            $scope.caseprofileAssociatedCorporationsTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                    total: ($scope.caseprofileAssociatedCorporations != null ? $scope.caseprofileAssociatedCorporations.length : 0), // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.caseprofileAssociatedCorporations != null ? $scope.caseprofileAssociatedCorporations.length : 0);
                        $scope.data = $scope.caseprofileAssociatedCorporations.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        return $scope.data;
                    }
                });
        }

        $scope.caseprofileAssociatedCorporationsOriginal = angular.copy($scope.caseprofileAssociatedCorporations);
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