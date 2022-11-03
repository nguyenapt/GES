"use strict";
GesInsideApp.controller("CaseProfileReferencesController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, ModalService, NgTableParams) {
    $scope.editingReference = null;
    $scope.caseprofileReferences = [];
    $scope.caseprofileReferencesOriginal = [];

    $scope.template = '/Content/angular/templates/caseprofile/ReferenceDialog.html';
    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();
    $scope.statuses = [];

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });

    $scope.addReference = function () {
        var temp = $scope.caseprofileReferences;
        $scope.caseprofileReferences = [];

        var source = {
            Id: 0,
            CaseReportId: CaseProfileService.caseProfile.I_GesCaseReports_Id,
            ManagedDocumentId: "",
            ShowInReport: false,
            Status: 0,
            Source: "",
            PublicationYear: null,
            AvailableFrom: "",
            Accessed: "",
            Name: ""
            //TODO: document
        };

        $scope.editingReference = source;

        $scope.caseprofileReferences.push(source);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.caseprofileReferences.push(temp[i]);
            }
        }

        $scope.caseprofileReferencesTableParams.reload();

        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileReferencesController');

    };

    $scope.deleteReference = function (source, frommodal) {
        ModalService.openConfirm("Are you sure to delete this source?", function (result) {
            if (result) {
                $scope.editingReference = source;
                if (frommodal) {
                    CaseProfileService.DeleteReference(source, deleteReferenceFromModalCallback);
                }
                else {
                    CaseProfileService.DeleteReference(source, deleteReferenceCallback);
                }
                ModalService.closeModal();
            }
        });
    };

    function deleteReferenceCallback() {
        if ($scope.editingReference) {
            for (var i in $scope.caseprofileReferences) {
                if ($scope.caseprofileReferences[i].Id === $scope.editingReference.Id) {
                    $scope.caseprofileReferences.splice(i, 1);
                }
            }
            if ($scope.caseprofileReferencesTableParams.data.length === 1 && $scope.caseprofileReferencesTableParams.page() !== 1) {
                $scope.caseprofileReferencesTableParams.page($scope.caseprofileReferencesTableParams.page() - 1);
            }
        }

        $scope.caseprofileReferencesOriginal = $scope.caseprofileReferences;
        $scope.caseprofileReferencesTableParams.reload();
    }

    function deleteReferenceFromModalCallback() {
        if ($scope.$parent.editingReference) {
            for (var i in $scope.$parent.caseprofileReferences) {
                if ($scope.$parent.caseprofileReferences[i].Id === $scope.$parent.editingReference.Id) {
                    $scope.$parent.caseprofileReferences.splice(i, 1);
                }
            }
            if ($scope.$parent.caseprofileReferencesTableParams.data.length === 1 && $scope.$parent.caseprofileReferencesTableParams.page() !== 1) {
                $scope.$parent.caseprofileReferencesTableParams.page($scope.$parent.caseprofileReferencesTableParams.page() - 1);
            }
        }

        $scope.$parent.caseprofileReferencesOriginal = $scope.$parent.caseprofileReferences;
        $scope.$parent.caseprofileReferencesTableParams.reload();
    }

    $scope.editReference = function (source) {
        if (source) {
            $scope.editingReference = source;
        }

        $scope.caseprofileReferencesTableParams.reload();
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileReferencesController');
    };

    $scope.cancelReference = function (source) {
        if (source.Id === 0) {
            $scope.$parent.caseprofileReferences.splice(0, 1);
            $scope.$parent.caseprofileReferencesTableParams.reload();
        }
        else {
            $scope.$parent.caseprofileReferences = angular.copy($scope.$parent.caseprofileReferencesOriginal);
        }

        $scope.caseprofileReferencesTableParams.reload();

        ModalService.closeModal();

    }

    $scope.saveReference = function (source, isAddNew) {
        if (isAddNew) {
            CaseProfileService.SaveReference(source, saveAndAddNewReferenceCallback);
        }
        else {
            CaseProfileService.SaveReference(source, saveReferenceCallback);
        }
        ModalService.closeModal();
    }

    function saveReferenceCallback(Id) {
        if ($scope.ngDialogData.editingReference.Id === 0) {
            $scope.ngDialogData.editingReference.Id = Id;
        }
        $scope.ngDialogData.caseprofileReferencesOriginal = angular.copy($scope.ngDialogData.caseprofileReferences);

        $scope.ngDialogData.caseprofileReferencesTableParams.reload();
    }

    function saveAndAddNewReferenceCallback(Id, isAddNew) {
        if ($scope.ngDialogData.editingReference.Id === 0) {
            $scope.ngDialogData.editingReference.Id = Id;
        }

        $scope.ngDialogData.caseprofileReferencesOriginal = angular.copy($scope.ngDialogData.caseprofileReferences);

        $scope.ngDialogData.caseprofileReferencesTableParams.reload();

        $scope.ngDialogData.addReference();

    }

    function init() {

        $scope.caseprofileReferences = CaseProfileService.caseProfile.ReferencesViewModels;
        $scope.statuses.push({ id: 1, Name: "Online" })
        $scope.statuses.push({ id: 2, Name: "On Request" })

        if ($scope.caseprofileReferences != null && $scope.caseprofileReferences.length > 0) {
            for (var i = 0; i < $scope.caseprofileReferences.length; i++) {

                if (($scope.caseprofileReferences[i].Accessed != null)) {
                    $scope.caseprofileReferences[i].Accessed =
                        new Date(convertDate($scope.caseprofileReferences[i].Accessed, 'yyyy/MM/dd'));
                }
            }
        }

        if ($scope.caseprofileReferences != null) {
            $scope.caseprofileReferencesTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                    total: ($scope.caseprofileReferences != null ? $scope.caseprofileReferences.length : 0), // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.caseprofileReferences != null ? $scope.caseprofileReferences.length : 0);
                        $scope.data = $scope.caseprofileReferences.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        return $scope.data;
                    }
                });
        }

        $scope.caseprofileReferencesOriginal = angular.copy($scope.caseprofileReferences);
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