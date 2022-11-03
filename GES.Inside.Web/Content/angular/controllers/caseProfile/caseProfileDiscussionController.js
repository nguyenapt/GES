"use strict";
GesInsideApp.controller("CaseProfileDiscussionController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, ModalService, NgTableParams) {
    $scope.discussionPoints = [];
    $scope.discussionPointsOriginal = [];
    $scope.editingDiscussionPoint = null;
    $scope.templateDiscussionPoint = '/Content/angular/templates/caseprofile/DiscussionPointDialog.html';    

    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();    

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });

    //Discussion point
    $scope.addDiscussionPoint = function () {
        var temp = $scope.discussionPoints;
        $scope.discussionPoints = [];       
        
        var discussionPoint = {
            DiscussionPointsId: 0,
            CompanyId: getCompanyId(),
            Name: "",
            Description: "",
            Created: "",
        };

        $scope.editingDiscussionPoint = discussionPoint;

        $scope.discussionPoints.push(discussionPoint);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.discussionPoints.push(temp[i]);
            }
        }

        CaseProfileService.caseProfile.DiscussionPoints = $scope.discussionPoints;

        if ($scope.discussionPointsTableParams != undefined) {
            $scope.discussionPointsTableParams.reload();
        }        
        ModalService.openModal($scope.templateDiscussionPoint, $scope, "ng-dialog-medium", 'CaseProfileDiscussionController');        
    };

    $scope.editDiscussionPoint = function (discussionPoint) {
        if (discussionPoint) {

            $scope.editingDiscussionPoint = discussionPoint;
        }
        $scope.discussionPointsTableParams.reload();

        ModalService.openModal($scope.templateDiscussionPoint, $scope, "ng-dialog-medium", 'CaseProfileDiscussionController');
    };

    $scope.saveDiscussionPoint = function (discussionPoint, isAddNew) {
        if (isAddNew) {
            CaseProfileService.SaveDiscussionPoint(discussionPoint, saveAndAddNewDiscussionPointCallback);
        }
        else {
            CaseProfileService.SaveDiscussionPoint(discussionPoint, saveDiscussionPointCallback);
        }        
        ModalService.closeModal();
    }

    function saveDiscussionPointCallback(Id) {
        if ($scope.ngDialogData.editingDiscussionPoint.DiscussionPointsId === 0) {
            $scope.ngDialogData.editingDiscussionPoint.DiscussionPointsId = Id;
        }
        $scope.ngDialogData.discussionPointsOriginal = angular.copy($scope.ngDialogData.discussionPoints);
        $scope.ngDialogData.discussionPointsTableParams.reload();
    }

    function saveAndAddNewDiscussionPointCallback(Id) {
        if ($scope.ngDialogData.editingDiscussionPoint.DiscussionPointsId === 0) {
            $scope.ngDialogData.editingDiscussionPoint.DiscussionPointsId = Id;
        }

        $scope.ngDialogData.discussionPointsOriginal = angular.copy($scope.ngDialogData.discussionPoints);
        $scope.ngDialogData.discussionPointsTableParams.reload();
        $scope.ngDialogData.addDiscussionPoint();
    }

    $scope.cancelDiscussionPoint = function (discussionPoint) {
        if (discussionPoint.DiscussionPointsId === 0) {
            $scope.$parent.discussionPoints.splice(0, 1);            
        }else {
            $scope.$parent.discussionPoints = angular.copy($scope.$parent.discussionPointsOriginal);
        }

        if ($scope.$parent.discussionPointsTableParams != undefined) {
            $scope.$parent.discussionPointsTableParams.reload();
        }        
        ModalService.closeModal();        
    }

    $scope.deleteDiscussionPoint = function (discussionPoint,frommodal) {
        ModalService.openConfirm("Are you sure to delete this discussion point?", function (result) {
            if (result) {
                $scope.editingDiscussionPoint = discussionPoint;
                if (frommodal) {
                    CaseProfileService.DeleteDiscussionPoint(discussionPoint, deleteDiscussionPointFromModalCallback);
                }
                else {
                    CaseProfileService.DeleteDiscussionPoint(discussionPoint, deleteDiscussionPointCallback);
                }
                ModalService.closeModal();                
            }
        });        
    }    

    function deleteDiscussionPointCallback() {
        if ($scope.editingDiscussionPoint) {
            for (var i in $scope.discussionPoints) {
                if ($scope.discussionPoints[i].DiscussionPointsId === $scope.editingDiscussionPoint.DiscussionPointsId) {
                    $scope.discussionPoints.splice(i, 1);
                }
            }

            if ($scope.discussionPointsTableParams.data.length === 1 && $scope.discussionPointsTableParams.page() !== 1) {
                $scope.discussionPointsTableParams.page($scope.discussionPointsTableParams.page() - 1);
            }
        }

        CaseProfileService.caseProfile.DiscussionPoints = $scope.discussionPoints;
        $scope.discussionPointsOriginal = $scope.discussionPoints;
        $scope.discussionPointsTableParams.reload();
    }

    function deleteDiscussionPointFromModalCallback() {
        if ($scope.$parent.editingDiscussionPoint) {
            for (var i in $scope.$parent.discussionPoints) {
                if ($scope.$parent.discussionPoints[i].DiscussionPointsId === $scope.$parent.editingDiscussionPoint.DiscussionPointsId) {
                    $scope.$parent.discussionPoints.splice(i, 1);
                }
            }

            if ($scope.$parent.discussionPointsTableParams.data.length === 1 && $scope.$parent.discussionPointsTableParams.page() !== 1) {
                $scope.$parent.discussionPointsTableParams.page($scope.$parent.discussionPointsTableParams.page() - 1);
            }
        }

        CaseProfileService.caseProfile.DiscussionPoints = $scope.$parent.discussionPoints;
        $scope.$parent.discussionPointsOriginal = $scope.$parent.discussionPoints;
        $scope.$parent.discussionPointsTableParams.reload();
    }

    //End Discussion point    

    function init() {
        $scope.discussionPoints = CaseProfileService.caseProfile.DiscussionPoints;

        if ($scope.discussionPoints != null && $scope.discussionPoints.length > 0) {
            for (var i = 0; i < $scope.discussionPoints.length; i++) {

                if (($scope.discussionPoints[i].Created != null)) {
                    $scope.discussionPoints[i].Created =
                        new Date(convertDate($scope.discussionPoints[i].Created, 'yyyy/MM/dd'));
                }
            }
        }        
        if ($scope.discussionPoints != null) {
            $scope.discussionPointsTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5          // count per page    
            }, {
                    total: $scope.discussionPoints.length, // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.discussionPoints.length);
                        $scope.data = $scope.discussionPoints.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        return $scope.data;
                    }
                });
        }
        $scope.discussionPointsOriginal = angular.copy($scope.discussionPoints);
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