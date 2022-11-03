"use strict";
GesInsideApp.controller("CaseProfileGSSLinkController", ["$scope", "$filter", "$timeout", "$window", "CaseProfileService", "ModalService", "NgTableParams", function ($scope, $filter, $timeout, $window, CaseProfileService, ModalService, NgTableParams) {
    $scope.editingGSSLink = null;
    $scope.isAddNewGSSLink = true;
    $scope.gsslinks = [];
    $scope.gsslinksOriginal = [];
    $scope.template = '/Content/angular/templates/caseprofile/GSSLinkDialog.html';

    $scope.isNewCaseProfile = CaseProfileService.IsAddNewCaseProfile();

    $scope.$on('finishInitCaseProfile', function (e) {
        init();
    });
    
    $scope.addGSSLink = function () {
        var temp = $scope.gsslinks;
        $scope.gsslinks = [];

        var gsslink = {
            I_GSSLink_Id: "00000000-0000-0000-0000-000000000000",
            I_GesCaseReports_Id: CaseProfileService.caseProfile.I_GesCaseReports_Id,
            Description: "",
            GSSLinkModified: "",
            GSSLinkModifiedString: "",
            Created: "",
            CreatedString: ""
        };

        $scope.editingGSSLink = gsslink;

        $scope.gsslinks.push(gsslink);

        if (temp != null) {
            for (var i = 0; i < temp.length; i++) {
                $scope.gsslinks.push(temp[i]);
            }
        }

        CaseProfileService.caseProfile.GSSLinkViewModels = $scope.gsslinks;

        $scope.gsslinkTableParams.reload();        
        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileGSSLinkController');        
    };

    $scope.deleteGSSLink = function (gsslink, frommodal) {
        ModalService.openConfirm("Are you sure to delete this gss link?", function (result) {
            if (result) {
                $scope.editingGSSLink = gsslink;
                if (frommodal) {
                    CaseProfileService.deleteGSSLink(gsslink, deleteGSSLinkFromModalCallback);                    
                }
                else {
                    CaseProfileService.deleteGSSLink(gsslink, deleteGSSLinkCallback);
                }                
                ModalService.closeModal();                
            }
        });        
    };

    function deleteGSSLinkCallback() {
        if ($scope.editingGSSLink) {

            for (var i in $scope.gsslinks) {
                if ($scope.gsslinks[i].I_GSSLink_Id === $scope.editingGSSLink.I_GSSLink_Id) {
                    $scope.gsslinks.splice(i, 1);
                }
            }

            if ($scope.gsslinkTableParams.data.length === 1 && $scope.gsslinkTableParams.page() !== 1) {
                $scope.gsslinkTableParams.page($scope.gsslinkTableParams.page() - 1);
            }
        }

        CaseProfileService.caseProfile.GSSLinkViewModels = $scope.gsslinks;
        $scope.gsslinksOriginal = $scope.gsslinks;
        $scope.gsslinkTableParams.reload();
    }

    function deleteGSSLinkFromModalCallback() {
        if ($scope.$parent.editingGSSLink) {

            for (var i in $scope.$parent.gsslinks) {
                if ($scope.$parent.gsslinks[i].I_GSSLink_Id === $scope.$parent.editingGSSLink.I_GSSLink_Id) {
                    $scope.$parent.gsslinks.splice(i, 1);
                }
            }

            if ($scope.$parent.gsslinkTableParams.data.length === 1 && $scope.$parent.gsslinkTableParams.page() !== 1) {
                $scope.$parent.gsslinkTableParams.page($scope.$parent.gsslinkTableParams.page() - 1);
            }
        }

        CaseProfileService.caseProfile.GSSLinkViewModels = $scope.$parent.gsslinks;
        $scope.$parent.gsslinksOriginal = $scope.$parent.gsslinks;
        $scope.$parent.gsslinkTableParams.reload();
    }

    $scope.editGSSLink = function (gsslink) {
        if (gsslink) {
            $scope.editingGSSLink = gsslink;
        }
        $scope.gsslinkTableParams.reload();

        ModalService.openModal($scope.template, $scope, "ng-dialog-medium", 'CaseProfileGSSLinkController');
    };

    $scope.cancelGSSLink = function (gsslink) {
        if (gsslink.I_GSSLink_Id === 0) {
            $scope.$parent.gsslinks.splice(0, 1);            
        }
        else {
            $scope.$parent.gsslinks = angular.copy($scope.$parent.gsslinksOriginal);
        }

        $scope.$parent.gsslinkTableParams.reload();        
        ModalService.closeModal();        
    }

    $scope.saveGSSLink = function (gsslink, isAddNew) {
        if (isAddNew) {
            CaseProfileService.saveGSSLink(gsslink, saveAndAddNewGSSLinkCallback);
        }
        else {
            CaseProfileService.saveGSSLink(gsslink, saveGSSLinkCallback);
        }        
        ModalService.closeModal();        
    }

    function saveGSSLinkCallback(Id) {
        if ($scope.ngDialogData.editingGSSLink.I_GSSLink_Id === 0) {
            $scope.ngDialogData.editingGSSLink.I_GSSLink_Id = Id;
        }
        $scope.ngDialogData.gsslinksOriginal = angular.copy($scope.ngDialogData.gsslinks);
        $scope.ngDialogData.gsslinkTableParams.reload();
    }

    function saveAndAddNewGSSLinkCallback(Id) {
        if ($scope.ngDialogData.editingGSSLink.I_GSSLink_Id === 0) {
            $scope.ngDialogData.editingGSSLink.I_GSSLink_Id = Id;
        }
        $scope.ngDialogData.gsslinksOriginal = angular.copy($scope.ngDialogData.gsslinks);
        $scope.ngDialogData.gsslinkTableParams.reload();
        $scope.ngDialogData.addGSSLink();
    }

    function init() {
        $scope.gsslinks = CaseProfileService.caseProfile.GSSLinkViewModels;        

        if ($scope.gsslinksOriginal != null && $scope.gsslinksOriginal.length > 0) {
            for (var i = 0; i < $scope.gsslinksOriginal.length; i++) {

                if (($scope.gsslinks[i].Created != null)) {
                    $scope.gsslinks[i].Created = new Date(convertDate($scope.gsslinks[i].Created, 'yyyy/MM/dd'));
                }
                if (($scope.gsslinks[i].GSSLinkModified != null)) {
                    $scope.gsslinks[i].GSSLinkModified = new Date(convertDate($scope.gsslinks[i].GSSLinkModified, 'yyyy/MM/dd'));
                }
            }
        }
        if ($scope.gsslinks != null) {
            $scope.gsslinkTableParams = new NgTableParams({
                page: 1,            // show first page
                count: 5         // count per page    

            }, {
                    total: $scope.gsslinks.length, // length of data
                    counts: [5, 25, 50, 100],
                    getData: function (params) {
                        params.total($scope.gsslinks != null ? $scope.gsslinks.length : 0);
                        if ($scope.gsslinks != null) {
                            $scope.data = $scope.gsslinks.slice((params.page() - 1) * params.count(), params.page() * params.count());
                        }
                        return $scope.data;
                    }
                });
        }

        $scope.gsslinksOriginal = angular.copy($scope.gsslinks);
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