"use strict";
GesInsideApp.controller("CaseProfileStakeholderController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, ModalService, NgTableParams) {
    $scope.stakeholderViews = [];
    $scope.stakeholderViewsOriginal = [];
    $scope.editingStakeholderView = null;
    $scope.templateStakeholder = '/Content/angular/templates/caseprofile/OtherStakeholderDialog.html';

    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });

    //Other stakeholder
    $scope.addStakeholder = function () {
        var temp = $scope.stakeholderViews;
        $scope.stakeholderViews = [];

        var stakeholderView = {
            OtherStakeholderViewsId: 0,
            CompanyId: getCompanyId(),
            Name: "",
            Description: "",
            Url: "",
            Created: "",
        };

        $scope.editingStakeholderView = stakeholderView;

        $scope.stakeholderViews.push(stakeholderView);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.stakeholderViews.push(temp[i]);
            }
        }

        CaseProfileService.caseProfile.StakeholderViews = $scope.stakeholderViews;

        if ($scope.stakeholderViewsTableParams != undefined) {
            $scope.stakeholderViewsTableParams.reload();
        }
        ModalService.openModal($scope.templateStakeholder, $scope, "ng-dialog-medium", 'CaseProfileStakeholderController');
    };

    $scope.editStakeholderView = function (stakeholderView) {
        if (stakeholderView) {

            $scope.editingStakeholderView = stakeholderView;
        }

        $scope.stakeholderViewsTableParams.reload();
        ModalService.openModal($scope.templateStakeholder, $scope, "ng-dialog-medium", 'CaseProfileStakeholderController');
    };

    $scope.saveStakeholderView = function (stakeholderView, isAddNew) {
        if (isAddNew) {
            CaseProfileService.SaveStakeholderView(stakeholderView, saveAndAddNewStakeholderViewCallback);
        }
        else {
            CaseProfileService.SaveStakeholderView(stakeholderView, saveStakeholderViewCallback);
        }

        ModalService.closeModal();

    }

    function saveStakeholderViewCallback(Id) {
        if ($scope.ngDialogData.editingStakeholderView.OtherStakeholderViewsId === 0) {
            $scope.ngDialogData.editingStakeholderView.OtherStakeholderViewsId = Id;
        }
        $scope.ngDialogData.stakeholderViewsOriginal = angular.copy($scope.ngDialogData.stakeholderViews);
        $scope.ngDialogData.stakeholderViewsTableParams.reload();
    }

    function saveAndAddNewStakeholderViewCallback(Id) {
        if ($scope.ngDialogData.editingStakeholderView.OtherStakeholderViewsId === 0) {
            $scope.ngDialogData.editingStakeholderView.OtherStakeholderViewsId = Id;
        }
        $scope.ngDialogData.stakeholderViewsOriginal = angular.copy($scope.ngDialogData.stakeholderViews);
        $scope.ngDialogData.stakeholderViewsTableParams.reload();
        $scope.ngDialogData.addStakeholder();
    }

    $scope.deleteStakeholderView = function (stakeholderView, frommodal) {
        ModalService.openConfirm("Are you sure to delete this stakeholder view?", function (result) {
            if (result) {
                $scope.editingStakeholderView = stakeholderView;
                if (frommodal) {
                    CaseProfileService.DeleteStakeholderView(stakeholderView, deleteStakeholderViewFromModalCallback);
                }
                else {
                    CaseProfileService.DeleteStakeholderView(stakeholderView, deleteStakeholderViewCallback);
                }

                ModalService.closeModal();

            }
        });
    }

    $scope.cancelStakeholderView = function (stakeholderView) {
        if (stakeholderView.OtherStakeholderViewsId === 0) {
            $scope.$parent.stakeholderViews.splice(0, 1);
        } else {
            $scope.$parent.stakeholderViews = angular.copy($scope.$parent.stakeholderViewsOriginal);
        }
        if ($scope.$parent.stakeholderViewsTableParams != undefined) {
            $scope.$parent.stakeholderViewsTableParams.reload();
        }


        ModalService.closeModal();

    }

    function deleteStakeholderViewCallback() {
        if ($scope.editingStakeholderView) {
            for (var i in $scope.stakeholderViews) {
                if ($scope.stakeholderViews[i].OtherStakeholderViewsId === $scope.editingStakeholderView.OtherStakeholderViewsId) {
                    $scope.stakeholderViews.splice(i, 1);
                }
            }
            if ($scope.stakeholderViewsTableParams.data.length === 1 && $scope.stakeholderViewsTableParams.page() !== 1) {
                $scope.stakeholderViewsTableParams.page($scope.stakeholderViewsTableParams.page() - 1);
            }
        }
        $scope.caseProfile.StakeholderViews = $scope.stakeholderViews;
        $scope.stakeholderViewsOriginal = $scope.stakeholderViews;
        $scope.stakeholderViewsTableParams.reload();
    }

    function deleteStakeholderViewFromModalCallback() {
        if ($scope.$parent.editingStakeholderView) {
            for (var i in $scope.$parent.stakeholderViews) {
                if ($scope.$parent.stakeholderViews[i].OtherStakeholderViewsId === $scope.$parent.editingStakeholderView.OtherStakeholderViewsId) {
                    $scope.$parent.stakeholderViews.splice(i, 1);
                }
            }
            if ($scope.$parent.stakeholderViewsTableParams.data.length === 1 && $scope.$parent.stakeholderViewsTableParams.page() !== 1) {
                $scope.$parent.stakeholderViewsTableParams.page($scope.$parent.stakeholderViewsTableParams.page() - 1);
            }
        }
        $scope.caseProfile.StakeholderViews = $scope.stakeholderViews;
        $scope.$parent.stakeholderViewsOriginal = $scope.$parent.stakeholderViews;
        $scope.$parent.stakeholderViewsTableParams.reload();
    }
    //End Other stakeholder

    function init() {
        $scope.stakeholderViews = CaseProfileService.caseProfile.StakeholderViews;

        if ($scope.stakeholderViews != null && $scope.stakeholderViews.length > 0) {
            for (var i = 0; i < $scope.stakeholderViews.length; i++) {

                if (($scope.stakeholderViews[i].Created != null)) {
                    $scope.stakeholderViews[i].Created =
                        new Date(convertDate($scope.stakeholderViews[i].Created, 'yyyy/MM/dd'));
                }
            }
        }

        if ($scope.stakeholderViews != null) {
            $scope.stakeholderViewsTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                    total: $scope.stakeholderViews.length, // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.stakeholderViews.length);
                        $scope.data = $scope.stakeholderViews.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        return $scope.data;
                    }
                });
        }

        $scope.stakeholderViewsOriginal = angular.copy($scope.stakeholderViews);
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
        if (CaseProfileService.caseProfile.I_Companies_Id != 0) {
            companyid = CaseProfileService.caseProfile.I_Companies_Id;
        }
        else {
            companyid = CaseProfileService.GetCompanyId()
        }
        return companyid;
    }
}]);