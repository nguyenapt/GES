"use strict";
GesInsideApp.controller("CaseProfileSupplementaryReadingController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, ModalService, NgTableParams) {
    $scope.editingSupplementaryReading = null;
    $scope.caseprofileSupplementaryReadings = [];
    $scope.caseprofileSupplementaryReadingsOriginal = [];

    $scope.template = '/Content/angular/templates/caseprofile/SupplementaryReadingDialog.html';
    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();
    $scope.statuses = [];

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });

    $scope.addSupplementaryReading = function () {
        var temp = $scope.caseprofileSupplementaryReadings;
        $scope.caseprofileSupplementaryReadings = [];

        var supplementary = {
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

        $scope.editingSupplementaryReading = supplementary;

        $scope.caseprofileSupplementaryReadings.push(supplementary);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.caseprofileSupplementaryReadings.push(temp[i]);
            }
        }

        $scope.caseprofileSupplementaryReadingsTableParams.reload();

        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileSupplementaryReadingController');

    };

    $scope.deleteSupplementaryReading = function (supplementary, frommodal) {
        ModalService.openConfirm("Are you sure to delete this supplementary?", function (result) {
            if (result) {
                $scope.editingSupplementaryReading = supplementary;
                if (frommodal) {
                    CaseProfileService.DeleteSupplementaryReading(supplementary, deleteSupplementaryReadingFromModalCallback);
                }
                else {
                    CaseProfileService.DeleteSupplementaryReading(supplementary, deleteSupplementaryReadingCallback);
                }
                ModalService.closeModal();
            }
        });
    };

    function deleteSupplementaryReadingCallback() {
        if ($scope.editingSupplementaryReading) {
            for (var i in $scope.caseprofileSupplementaryReadings) {
                if ($scope.caseprofileSupplementaryReadings[i].Id === $scope.editingSupplementaryReading.Id) {
                    $scope.caseprofileSupplementaryReadings.splice(i, 1);
                }
            }
            if ($scope.caseprofileSupplementaryReadingsTableParams.data.length === 1 && $scope.caseprofileSupplementaryReadingsTableParams.page() !== 1) {
                $scope.caseprofileSupplementaryReadingsTableParams.page($scope.caseprofileSupplementaryReadingsTableParams.page() - 1);
            }
        }

        $scope.caseprofileSupplementaryReadingsOriginal = $scope.caseprofileSupplementaryReadings;
        $scope.caseprofileSupplementaryReadingsTableParams.reload();
    }

    function deleteSupplementaryReadingFromModalCallback() {
        if ($scope.$parent.editingSupplementaryReading) {
            for (var i in $scope.$parent.caseprofileSupplementaryReadings) {
                if ($scope.$parent.caseprofileSupplementaryReadings[i].Id === $scope.$parent.editingSupplementaryReading.Id) {
                    $scope.$parent.caseprofileSupplementaryReadings.splice(i, 1);
                }
            }
            if ($scope.$parent.caseprofileSupplementaryReadingsTableParams.data.length === 1 && $scope.$parent.caseprofileSupplementaryReadingsTableParams.page() !== 1) {
                $scope.$parent.caseprofileSupplementaryReadingsTableParams.page($scope.$parent.caseprofileSupplementaryReadingsTableParams.page() - 1);
            }
        }

        $scope.$parent.caseprofileSupplementaryReadingsOriginal = $scope.$parent.caseprofileSupplementaryReadings;
        $scope.$parent.caseprofileSupplementaryReadingsTableParams.reload();
    }

    $scope.editSupplementaryReading = function (supplementary) {
        if (supplementary) {
            $scope.editingSupplementaryReading = supplementary;
        }

        $scope.caseprofileSupplementaryReadingsTableParams.reload();
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileSupplementaryReadingController');
    };

    $scope.cancelSupplementaryReading = function (supplementary) {
        if (supplementary.Id === 0) {
            $scope.$parent.caseprofileSupplementaryReadings.splice(0, 1);
            $scope.$parent.caseprofileSupplementaryReadingsTableParams.reload();
        }
        else {
            $scope.$parent.caseprofileSupplementaryReadings = angular.copy($scope.$parent.caseprofileSupplementaryReadingsOriginal);
        }

        $scope.caseprofileSupplementaryReadingsTableParams.reload();

        ModalService.closeModal();

    }

    $scope.saveSupplementaryReading = function (supplementary, isAddNew) {
        if (isAddNew) {
            CaseProfileService.SaveSupplementaryReading(supplementary, saveAndAddNewSupplementaryReadingCallback);
        }
        else {
            CaseProfileService.SaveSupplementaryReading(supplementary, saveSupplementaryReadingCallback);
        }
        ModalService.closeModal();
    }

    function saveSupplementaryReadingCallback(Id) {
        if ($scope.ngDialogData.editingSupplementaryReading.Id === 0) {
            $scope.ngDialogData.editingSupplementaryReading.Id = Id;
        }
        $scope.ngDialogData.caseprofileSupplementaryReadingsOriginal = angular.copy($scope.ngDialogData.caseprofileSupplementaryReadings);

        $scope.ngDialogData.caseprofileSupplementaryReadingsTableParams.reload();
    }

    function saveAndAddNewSupplementaryReadingCallback(Id, isAddNew) {
        if ($scope.ngDialogData.editingSupplementaryReading.Id === 0) {
            $scope.ngDialogData.editingSupplementaryReading.Id = Id;
        }

        $scope.ngDialogData.caseprofileSupplementaryReadingsOriginal = angular.copy($scope.ngDialogData.caseprofileSupplementaryReadings);

        $scope.ngDialogData.caseprofileSupplementaryReadingsTableParams.reload();

        $scope.ngDialogData.addSupplementaryReading();

    }

    function init() {

        $scope.caseprofileSupplementaryReadings = CaseProfileService.caseProfile.SupplementaryReadingViewModels;
        $scope.statuses.push({ id: 1, Name: "Online" })
        $scope.statuses.push({ id: 2, Name: "On Request" })

        if ($scope.caseprofileSupplementaryReadings != null && $scope.caseprofileSupplementaryReadings.length > 0) {
            for (var i = 0; i < $scope.caseprofileSupplementaryReadings.length; i++) {

                if (($scope.caseprofileSupplementaryReadings[i].Accessed != null)) {
                    $scope.caseprofileSupplementaryReadings[i].Accessed =
                        new Date(convertDate($scope.caseprofileSupplementaryReadings[i].Accessed, 'yyyy/MM/dd'));
                }
            }
        }

        if ($scope.caseprofileSupplementaryReadings != null) {
            $scope.caseprofileSupplementaryReadingsTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                    total: ($scope.caseprofileSupplementaryReadings != null ? $scope.caseprofileSupplementaryReadings.length : 0), // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.caseprofileSupplementaryReadings != null ? $scope.caseprofileSupplementaryReadings.length : 0);
                        $scope.data = $scope.caseprofileSupplementaryReadings.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        return $scope.data;
                    }
                });
        }

        $scope.caseprofileSupplementaryReadingsOriginal = angular.copy($scope.caseprofileSupplementaryReadings);
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