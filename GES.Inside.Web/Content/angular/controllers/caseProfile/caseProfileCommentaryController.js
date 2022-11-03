"use strict";
GesInsideApp.controller("CaseProfileCommentaryController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, ModalService, NgTableParams) {
    $scope.editingCommentary = null;
    $scope.isAddNewCommentary = true;
    $scope.commentaries = [];
    $scope.commentariesOriginal = [];
    $scope.template = '/Content/angular/templates/caseprofile/CommentaryDialog.html';

    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });
    
    $scope.addCommentary = function () {
        var temp = $scope.commentaries;
        $scope.commentaries = [];

        var commentary = {
            I_GesCommentary_Id: 0,
            I_GesCaseReports_Id: CaseProfileService.caseProfile.I_GesCaseReports_Id,
            Description: "",
            CommentaryModified: "",
            CommentaryModifiedString: "",
            Created: "",
            CreatedString: ""
        };

        $scope.editingCommentary = commentary;

        $scope.commentaries.push(commentary);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.commentaries.push(temp[i]);
            }
        }

        CaseProfileService.caseProfile.CommentaryViewModels = $scope.commentaries;

        $scope.commentaryTableParams.reload();        
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileCommentaryController');        
    };

    $scope.deleteCommentary = function (commentary, frommodal) {
        ModalService.openConfirm("Are you sure to delete this commentary?", function (result) {
            if (result) {
                $scope.editingCommentary = commentary;
                if (frommodal) {
                    CaseProfileService.DeleteCommentary(commentary, deleteCommentaryFromModalCallback);                    
                }
                else {
                    CaseProfileService.DeleteCommentary(commentary, deleteCommentaryCallback);
                }                
                ModalService.closeModal();                
            }
        });        
    };

    function deleteCommentaryCallback() {
        if ($scope.editingCommentary) {

            for (var i in $scope.commentaries) {
                if ($scope.commentaries[i].I_GesCommentary_Id === $scope.editingCommentary.I_GesCommentary_Id) {
                    $scope.commentaries.splice(i, 1);
                }
            }

            if ($scope.commentaryTableParams.data.length === 1 && $scope.commentaryTableParams.page() !== 1) {
                $scope.commentaryTableParams.page($scope.commentaryTableParams.page() - 1);
            }
        }

        CaseProfileService.caseProfile.CommentaryViewModels = $scope.commentaries;
        $scope.commentariesOriginal = $scope.commentaries;
        $scope.commentaryTableParams.reload();
    }

    function deleteCommentaryFromModalCallback() {
        if ($scope.$parent.editingCommentary) {

            for (var i in $scope.$parent.commentaries) {
                if ($scope.$parent.commentaries[i].I_GesCommentary_Id === $scope.$parent.editingCommentary.I_GesCommentary_Id) {
                    $scope.$parent.commentaries.splice(i, 1);
                }
            }

            if ($scope.$parent.commentaryTableParams.data.length === 1 && $scope.$parent.commentaryTableParams.page() !== 1) {
                $scope.$parent.commentaryTableParams.page($scope.$parent.commentaryTableParams.page() - 1);
            }
        }

        CaseProfileService.caseProfile.CommentaryViewModels = $scope.$parent.commentaries;
        $scope.$parent.commentariesOriginal = $scope.$parent.commentaries;
        $scope.$parent.commentaryTableParams.reload();
    }

    $scope.editCommentary = function (commentary) {
        if (commentary) {
            $scope.editingCommentary = commentary;
        }
        $scope.commentaryTableParams.reload();

        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileCommentaryController');
    };

    $scope.cancelCommentary = function (commentary) {
        if (commentary.I_GesCommentary_Id === 0) {
            $scope.$parent.commentaries.splice(0, 1);            
        }
        else {
            $scope.$parent.commentaries = angular.copy($scope.$parent.commentariesOriginal);
        }

        $scope.$parent.commentaryTableParams.reload();        
        ModalService.closeModal();        
    }

    $scope.saveCommentary = function (commentary, isAddNew) {
        if (isAddNew) {
            CaseProfileService.SaveCommentary(commentary, saveAndAddNewCommentaryCallback);
        }
        else {
            CaseProfileService.SaveCommentary(commentary, saveCommentaryCallback);
        }        
        ModalService.closeModal();        
    }

    function saveCommentaryCallback(Id) {
        if ($scope.ngDialogData.editingCommentary.I_GesCommentary_Id === 0) {
            $scope.ngDialogData.editingCommentary.I_GesCommentary_Id = Id;
        }
        $scope.ngDialogData.commentariesOriginal = angular.copy($scope.ngDialogData.commentaries);
        $scope.ngDialogData.commentaryTableParams.reload();
    }

    function saveAndAddNewCommentaryCallback(Id) {
        if ($scope.ngDialogData.editingCommentary.I_GesCommentary_Id === 0) {
            $scope.ngDialogData.editingCommentary.I_GesCommentary_Id = Id;
        }
        $scope.ngDialogData.commentariesOriginal = angular.copy($scope.ngDialogData.commentaries);
        $scope.ngDialogData.commentaryTableParams.reload();
        $scope.ngDialogData.addCommentary();
    }

    function init() {
        $scope.commentaries = CaseProfileService.caseProfile.CommentaryViewModels;        

        if ($scope.commentariesOriginal != null && $scope.commentariesOriginal.length > 0) {
            for (var i = 0; i < $scope.commentariesOriginal.length; i++) {

                if (($scope.commentaries[i].Created != null)) {
                    $scope.commentaries[i].Created = new Date(convertDate($scope.commentaries[i].Created, 'yyyy/MM/dd'));
                }
                if (($scope.commentaries[i].CommentaryModified != null)) {
                    $scope.commentaries[i].CommentaryModified = new Date(convertDate($scope.commentaries[i].CommentaryModified, 'yyyy/MM/dd'));
                }
            }
        }
        if ($scope.commentaries != null) {
            $scope.commentaryTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5         // count per page    

            }, {
                    total: $scope.commentaries.length, // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.commentaries != null ? $scope.commentaries.length : 0);
                        if ($scope.commentaries != null) {
                            $scope.data = $scope.commentaries.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        }
                        return $scope.data;
                    }
                });
        }

        $scope.commentariesOriginal = angular.copy($scope.commentaries);
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