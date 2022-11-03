"use strict";
GesInsideApp.controller("RoleController", ["$scope", "$timeout", "$window", "RoleService", "ModalService", "NgTableParams", function ($scope, $timeout, $window, roleService, ModalService, NgTableParams) {
    $scope.isFormValid = false;
    $scope.message = null;
    $scope.roleDetails = null;
    $scope.rolePermissions = null;
    
    $scope.editingUserRole = null;
    $scope.isNewRole= false;
    $scope.isSaving = false;
    $scope.isAddNew = false;
    $scope.chkSelectAll = false;

    $scope.usersInRoles = [];
    $scope.usersInRolesOriginal = [];
    $scope.templateUserSelect = '/Content/angular/templates/UserListDialog.html';;

    $scope.roleId = "22b466de-6c4f-4699-9972-bd3f5c56ad60";
    init();

    function init() {
        initSelect2();
        var urlPath = $window.location.href;
        var urlPathSplit = String(urlPath).split("/");             
        
        var roleId = "0";

        if (urlPathSplit !== null && urlPathSplit.length > 0) {
            var i = urlPathSplit[urlPathSplit.length - 1].indexOf('#');
            var s = urlPathSplit[urlPathSplit.length - 1];
            if (i !== -1) {
                s  = urlPathSplit[urlPathSplit.length - 1].substring(0, i); 
            }            
            
            if (s!== 'Add') {
                roleId = s;
            } else{
                $scope.isAddNew = true;
            }
        }

        $scope.isNewRole = urlPath.includes("/Add");

        if (roleId !== "0") {
            roleService.GetRoleDetailsById(roleId).then(
                function (d) {
                    $scope.roleDetails = d.data;                         
                },
                function () {
                    alert("Failed");
                }
            );
        }

        roleService.GetRolePermissions(roleId).then(
            function (d) {

                $scope.rolePermissions = d.data;
            });
        
        initCancelSaveConfirmationBox();
        initDeleteConfirmationBox();

        loadUserGrid();
    }

    function loadUserGrid() {
        roleService.GetUsersInRoleByRoleId(roleId).then(
            function (response) {
                $scope.usersInRolesOriginal = response.data;

                $scope.usersInRoles = angular.copy($scope.usersInRolesOriginal);

                //userInRoleTableParams

                if ($scope.usersInRoles != null) {
                    $scope.userInRoleTableParams = new NgTableParams({
                        page: 1,            // show first page
                        count: 20         // count per page    

                    }, {
                        total: $scope.usersInRoles.length, // length of data
                        counts: [5, 25, 50, 100],
                        getData: function (params) {
                            params.total($scope.usersInRoles != null ? $scope.usersInRoles.length : 0);
                            if ($scope.usersInRoles != null) {
                                $scope.data = $scope.usersInRoles.slice((params.page() - 1) * params.count(), params.page() * params.count());
                            }
                            return $scope.data;
                        }
                    });
                }
            },
            function () {
                quickNotification("Error occurred during loading userinrole data", "error");
            }
        );
    }

    $scope.UpdateRole = function () {
        $scope.isSaving = true;

        roleService.UpdateRole($scope.roleDetails, $scope.rolePermissions).then(function () {
            goToRolesList();
        });
    };  

    function initSelect2() {
    }

    function goToRolesList() {
        $window.location.href = "/Roles/List";
    }

    String.isNullOrEmpty = function(value) {
        return !(typeof value === "undefined" || (typeof value === "string" && value.length > 0));
    };

    function initCancelSaveConfirmationBox() {
        $("#cancel-save").confirmModal({
            confirmCallback: goToRolesList
        });
    }

    function initDeleteConfirmationBox() {
        $("#delete-role").confirmModal({
            confirmCallback: deleteRole
        });
    }

    function deleteRole() {
        var roleId = $scope.roleDetails.Id;
        roleService.DeleteRole(roleId, goToRolesList);
    }   

    $scope.SelectAllForm = function () {
        if ($scope.chkSelectAll == true) {
            for (var i = 0; i < $scope.rolePermissions.length; i++) {
                $scope.rolePermissions[i].AllowedRead = true;
            }
        } else {
            for (var i = 0; i < $scope.rolePermissions.length; i++) {
                $scope.rolePermissions[i].AllowedRead = false;
            }
        }
    };

    $scope.deleteUserRole = function (index) {
        ModalService.openConfirm("Are you sure to remove this user from role?", function(result) {
            if (result) {
                $scope.editingUserRole = index;

                roleService.DeleteUserRole(index, deleteUserRoleCallback);
            }
        });
    };

    function deleteUserRoleCallback() {
        if ($scope.editingUserRole) {

            for (var i in $scope.usersInRoles) {
                if ($scope.usersInRoles[i].Id === $scope.editingUserRole.Id) {
                    $scope.usersInRoles.splice(i, 1);
                }
            }

            if ($scope.userInRoleTableParams.data.length === 1 && $scope.commentaryTableParams.page() !== 1) {
                $scope.userInRoleTableParams.page($scope.commentaryTableParams.page() - 1);
            }
        }

        $scope.usersInRolesOriginal = $scope.usersInRoles;
        $scope.userInRoleTableParams.reload();
    }

    $scope.openUserList = function (d) {
        $scope.userSelectDialogId = ModalService.openModal($scope.templateUserSelect, $scope, "ng-dialog-large", 'RoleController');
    };

    $scope.cancelUserSelect = function () {
        ModalService.closeModal($scope.$parent.userSelectDialogId, null);
    }

    $scope.addUserToRole = function(userId, roleId) {
        roleService.AddUserToRole(userId, roleId, addedUserRoleCallback);
    }

    function addedUserRoleCallback() {
        ModalService.closeModal($scope.$parent.userSelectDialogId, null);
        loadUserGrid();
    }

    $scope.selectUserToRole = function () {
        var grid = $("#tblSelectUser");
        var rowKey = grid.jqGrid('getGridParam', "selrow");

        if (rowKey != null) {
            $scope.$parent.addUserToRole(grid.jqGrid('getCell', rowKey, 'Id'), $scope.$parent.roleDetails.Id);
        } else {
            ModalService.openConfirm("You have not selected any user. Do you want to continue?", function (result) {
                if (!result) {
                    ModalService.closeModal($scope.$parent.userSelectDialogId, null);
                }
            });
        }
    };

}]);
