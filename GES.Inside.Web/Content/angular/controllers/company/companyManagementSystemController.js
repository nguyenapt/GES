"use strict";
GesInsideApp.controller("CompanyManagementSystemController", ["$scope", "$filter", "$timeout", "$window", "CompanyService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CompanyService, ModalService, NgTableParams) {
    $scope.companyManagementSystemTableParams = null;
    $scope.companyManagementSystems = [];
    $scope.companyManagementSystemsOriginal = [];
    $scope.editingCompanyManagementSystem = null;
    $scope.template = '/Content/angular/templates/company/ManagementSystemDialog.html';

    $scope.$on('finishInitCompany', function (e) {
        init();
    });

    $scope.addCompanyManagementSystem = function () {
        var temp = $scope.companyManagementSystems;
        $scope.companyManagementSystems = [];

        var companyManagementSystem = {
            I_CompaniesI_ManagementSystems_Id: 0,
            I_Companies_Id: getCompanyId(),
            I_ManagementSystems_Id: "",
            Certification: false,
            Coverage: "",
            Created: ""
        };

        $scope.editingCompanyManagementSystem = companyManagementSystem;
        $scope.companyManagementSystems.push(companyManagementSystem);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.companyManagementSystems.push(temp[i]);
            }
        }

        $scope.companyManagementSystemTableParams.reload();

        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CompanyManagementSystemController');

    };

    $scope.deleteCompanyManagementSystem = function (companyManagementSystem, frommodal) {
        ModalService.openConfirm("Are you sure to delete this management system?", function (result) {
            if (result) {
                $scope.editingCompanyManagementSystem = companyManagementSystem;
                if (frommodal) {
                    CompanyService.DeleteCompanyManagementSystem(companyManagementSystem, deleteCompanyManagementSystemFromModalCallback);
                }
                else {
                    CompanyService.DeleteCompanyManagementSystem(companyManagementSystem, deleteCompanyManagementSystemCallback);
                }

                ModalService.closeModal();

            }
        });
    };

    function deleteCompanyManagementSystemCallback() {
        if ($scope.editingCompanyManagementSystem) {
            for (var i in $scope.companyManagementSystems) {
                if ($scope.companyManagementSystems[i].I_CompaniesI_ManagementSystems_Id === $scope.editingCompanyManagementSystem.I_CompaniesI_ManagementSystems_Id) {
                    $scope.companyManagementSystems.splice(i, 1);
                }
            }
            if ($scope.companyManagementSystemTableParams.data.length === 1 && $scope.companyManagementSystemTableParams.page() !== 1) {
                $scope.companyManagementSystemTableParams.page($scope.companyManagementSystemTableParams.page() - 1);
            }
        }

        $scope.companyManagementSystemsOriginal = $scope.companyManagementSystems;
        $scope.companyManagementSystemTableParams.reload();
    }

    function deleteCompanyManagementSystemFromModalCallback() {
        if ($scope.$parent.editingCompanyManagementSystem) {
            for (var i in $scope.$parent.companyManagementSystems) {
                if ($scope.$parent.companyManagementSystems[i].I_CompaniesI_ManagementSystems_Id === $scope.$parent.editingCompanyManagementSystem.I_CompaniesI_ManagementSystems_Id) {
                    $scope.$parent.companyManagementSystems.splice(i, 1);
                }
            }
            if ($scope.$parent.companyManagementSystemTableParams.data.length === 1 && $scope.$parent.companyManagementSystemTableParams.page() !== 1) {
                $scope.$parent.companyManagementSystemTableParams.page($scope.$parent.companyManagementSystemTableParams.page() - 1);
            }
        }

        $scope.$parent.companyManagementSystemsOriginal = $scope.$parent.companyManagementSystems;
        $scope.$parent.companyManagementSystemTableParams.reload();
    }

    $scope.editCompanyManagementSystem = function (companyManagementSystem) {
        if (companyManagementSystem) {
            $scope.editingCompanyManagementSystem = companyManagementSystem;
        }
        $scope.companyManagementSystemTableParams.reload();
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CompanyManagementSystemController');
    };

    $scope.cancelCompanyManagementSystem = function (companyManagementSystem) {
        if (companyManagementSystem.I_CompaniesI_ManagementSystems_Id === 0) {
            $scope.$parent.companyManagementSystems.splice(0, 1);
        }
        else {
            $scope.$parent.companyManagementSystems = angular.copy($scope.$parent.companyManagementSystemsOriginal);
        }

        $scope.$parent.companyManagementSystemTableParams.reload();


        ModalService.closeModal();

    }

    $scope.saveCompanyManagementSystem = function (companyManagementSystem, isAddNew) {
        if (isAddNew) {
            CompanyService.SaveCompanyManagementSystem(companyManagementSystem, saveAndAddNewCompanyManagementSystemCallback);
        }
        else {
            CompanyService.SaveCompanyManagementSystem(companyManagementSystem, saveCompanyManagementSystemCallback);
        }

        ModalService.closeModal();
    }

    function saveCompanyManagementSystemCallback(Id) {
        if ($scope.ngDialogData.editingCompanyManagementSystem.I_CompaniesI_ManagementSystems_Id === 0) {
            $scope.ngDialogData.editingCompanyManagementSystem.I_CompaniesI_ManagementSystems_Id = Id;
        }
        $scope.ngDialogData.companyManagementSystemsOriginal = angular.copy($scope.ngDialogData.companyManagementSystems);
        $scope.ngDialogData.companyManagementSystemTableParams.reload();
    }

    function saveAndAddNewCompanyManagementSystemCallback(Id) {
        if ($scope.ngDialogData.editingCompanyManagementSystem.I_CompaniesI_ManagementSystems_Id === 0) {
            $scope.ngDialogData.editingCompanyManagementSystem.I_CompaniesI_ManagementSystems_Id = Id;
        }
        $scope.ngDialogData.companyManagementSystemsOriginal = angular.copy($scope.ngDialogData.companyManagementSystems);
        $scope.ngDialogData.companyManagementSystemTableParams.reload();

        $scope.ngDialogData.addCompanyManagementSystem();
    }

    function init() {
        $scope.companyManagementSystems = $scope.companyDetails.CompanyManagementSystems;

        if ($scope.companyManagementSystems != null && $scope.companyManagementSystems.length > 0) {
            for (var i = 0; i < $scope.companyManagementSystems.length; i++) {

                if (($scope.companyManagementSystems[i].Created != null)) {
                    $scope.companyManagementSystems[i].Created = new Date(convertDate($scope.companyManagementSystems[i].Created, 'yyyy/MM/dd'));
                }
            }
        }
        if ($scope.companyManagementSystems != null) {
            $scope.companyManagementSystemTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                    total: $scope.companyManagementSystems.length, // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.companyManagementSystems != null ? $scope.companyManagementSystems.length : 0);
                        if ($scope.companyManagementSystems != null) {
                            $scope.data = $scope.companyManagementSystems.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        }

                        return $scope.data;
                    }
                });
        }

        $scope.companyManagementSystemsOriginal = angular.copy($scope.companyManagementSystems);
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

    function getCompanyId() {
        var companyid = 0;
        if (CompanyService.companyDetails.CompanyId != 0) {
            companyid = CompanyService.companyDetails.CompanyId;
        }
        else {
            companyid = CompanyService.GetCompanyId()
        }
        return companyid;
    }
}]);