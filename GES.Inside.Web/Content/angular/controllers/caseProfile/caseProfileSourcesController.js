"use strict";
GesInsideApp.controller("CaseProfileSourcesController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, ModalService, NgTableParams) {
    $scope.editingSource = null;
    $scope.caseprofileSources = [];
    $scope.caseprofileSourcesOriginal = [];

    $scope.template = '/Content/angular/templates/caseprofile/SourceDialog.html';
    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();
    $scope.statuses = [];

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });

    $scope.addSource = function () {
        var temp = $scope.caseprofileSources;
        $scope.caseprofileSources = [];

        var source = {
            Id: 0,
            CaseReportId: CaseProfileService.caseProfile.I_GesCaseReports_Id,
            ManagedDocumentId: "",
            MainSource: false,
            ShowInReport: false,
            Status: 0,
            Source: "",
            PublicationYear: null,
            AvailableFrom: "",
            Accessed: "",
            Name: ""
            //TODO: document
        };

        $scope.editingSource = source;

        $scope.caseprofileSources.push(source);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.caseprofileSources.push(temp[i]);
            }
        }

        $scope.caseprofileSourcesTableParams.reload();

        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileSourcesController');

    };

    $scope.deleteSource = function (source, frommodal) {
        ModalService.openConfirm("Are you sure to delete this source?", function (result) {
            if (result) {
                $scope.editingSource = source;
                if (frommodal) {
                    CaseProfileService.DeleteSource(source, deleteSourceFromModalCallback);
                }
                else {
                    CaseProfileService.DeleteSource(source, deleteSourceCallback);
                }
                ModalService.closeModal();
            }
        });
    };

    function deleteSourceCallback() {
        if ($scope.editingSource) {
            for (var i in $scope.caseprofileSources) {
                if ($scope.caseprofileSources[i].Id === $scope.editingSource.Id) {
                    $scope.caseprofileSources.splice(i, 1);
                }
            }
            if ($scope.caseprofileSourcesTableParams.data.length === 1 && $scope.caseprofileSourcesTableParams.page() !== 1) {
                $scope.caseprofileSourcesTableParams.page($scope.caseprofileSourcesTableParams.page() - 1);
            }
        }

        $scope.caseprofileSourcesOriginal = $scope.caseprofileSources;
        $scope.caseprofileSourcesTableParams.reload();
    }

    function deleteSourceFromModalCallback() {
        if ($scope.$parent.editingSource) {
            for (var i in $scope.$parent.caseprofileSources) {
                if ($scope.$parent.caseprofileSources[i].Id === $scope.$parent.editingSource.Id) {
                    $scope.$parent.caseprofileSources.splice(i, 1);
                }
            }
            if ($scope.$parent.caseprofileSourcesTableParams.data.length === 1 && $scope.$parent.caseprofileSourcesTableParams.page() !== 1) {
                $scope.$parent.caseprofileSourcesTableParams.page($scope.$parent.caseprofileSourcesTableParams.page() - 1);
            }
        }

        $scope.$parent.caseprofileSourcesOriginal = $scope.$parent.caseprofileSources;
        $scope.$parent.caseprofileSourcesTableParams.reload();
    }

    $scope.editSource = function (source) {
        if (source) {
            $scope.editingSource = source;
        }

        $scope.caseprofileSourcesTableParams.reload();
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileSourcesController');
    };

    $scope.cancelSource = function (source) {
        if (source.Id === 0) {
            $scope.$parent.caseprofileSources.splice(0, 1);
            $scope.$parent.caseprofileSourcesTableParams.reload();
        }
        else {
            $scope.$parent.caseprofileSources = angular.copy($scope.$parent.caseprofileSourcesOriginal);
        }

        $scope.caseprofileSourcesTableParams.reload();


        ModalService.closeModal();

    }

    $scope.saveSource = function (source, isAddNew) {
        if (isAddNew) {
            CaseProfileService.SaveSource(source, saveAndAddNewSourceCallback);
        }
        else {
            CaseProfileService.SaveSource(source, saveSourceCallback);
        }


        ModalService.closeModal();

    }

    function saveSourceCallback(Id) {
        if ($scope.ngDialogData.editingSource.Id === 0) {
            $scope.ngDialogData.editingSource.Id = Id;
        }
        $scope.ngDialogData.caseprofileSourcesOriginal = angular.copy($scope.ngDialogData.caseprofileSources);

        $scope.ngDialogData.caseprofileSourcesTableParams.reload();
    }

    function saveAndAddNewSourceCallback(Id, isAddNew) {
        if ($scope.ngDialogData.editingSource.Id === 0) {
            $scope.ngDialogData.editingSource.Id = Id;
        }

        $scope.ngDialogData.caseprofileSourcesOriginal = angular.copy($scope.ngDialogData.caseprofileSources);

        $scope.ngDialogData.caseprofileSourcesTableParams.reload();

        $scope.ngDialogData.addSource();

    }

    function init() {

        $scope.caseprofileSources = CaseProfileService.caseProfile.SourcesViewModels;
        $scope.statuses.push({ id: 1, Name: "Online" })
        $scope.statuses.push({ id: 2, Name: "On Request" })

        if ($scope.caseprofileSources != null && $scope.caseprofileSources.length > 0) {
            for (var i = 0; i < $scope.caseprofileSources.length; i++) {

                if (($scope.caseprofileSources[i].Accessed != null)) {
                    $scope.caseprofileSources[i].Accessed =
                        new Date(convertDate($scope.caseprofileSources[i].Accessed, 'yyyy/MM/dd'));
                }
            }
        }

        if ($scope.caseprofileSources != null) {
            $scope.caseprofileSourcesTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                    total: ($scope.caseprofileSources != null ? $scope.caseprofileSources.length : 0), // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.caseprofileSources != null ? $scope.caseprofileSources.length : 0);
                        $scope.data = $scope.caseprofileSources.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        return $scope.data;
                    }
                });
        }

        $scope.caseprofileSourcesOriginal = angular.copy($scope.caseprofileSources);
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