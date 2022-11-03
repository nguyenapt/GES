"use strict";

GesInsideApp.factory("RoleService", ["$http", "$q", "$window", "Upload", function ($http, $q, $window, Upload) {
    return {
        /**
         * @return {null}
         */
        GetRoleDetailsById: function (roleId) {

            if (roleId !== "0") {
                return $http.get("/Roles/GetRoleDetails", { params: { id: roleId } });
            }
            return null;
        },
        GetRolePermissions: function(roleId) {
            return $http.get("/Roles/GetRolePermissions", { params: { roleId: roleId } });
        },
        UpdateRole: function (role, rolePermissions) {            
            return $http({
                url: "/Roles/UpdateRole",
                method: "POST",
                data: {
                    roleModel: role,
                    permissionModels: rolePermissions
                }
            });
        },
        DeleteRole: function (roleId, successCallback) {
            $http({
                url: "/Roles/DeleteRole",
                method: "POST",
                data: {
                    roleId: roleId
                }
            }).then(
                function (d) {
                    if (d.data.Status === "Success") {
                        quickNotification("Deleted Role successfully");
                        successCallback();
                    } else {
                        quickNotification("Can not delete Role", "error");
                    }
                }
            );
        },
        GetUsersInRoleByRoleId: function(roleId) {
            if (roleId !== "0") {
                return $http.get("/Roles/GetUsersInRole", { params: { roleId: roleId } });
            }
            return null;
        },
        DeleteUserRole: function (userRole, successCallback) {
            $http({
                url: "/Roles/DeleteUserRole",
                method: "POST",
                data: {
                    userRole: userRole
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Remove user from role successfully");
                    successCallback();
                } else {
                    quickNotification("Can not remove user from this role", "error");
                }
            });
        },
        AddUserToRole(userId, roleId, successCallback) {
            $http({
                url: "/Roles/AddUserToRole",
                method: "POST",
                data: {
                    userId: userId,
                    roleId: roleId
                }
            }).then(function (d) {
                if (d.data.Status === "Success") {
                    quickNotification("Add user to role successfully");
                    successCallback();
                } else {
                    quickNotification("Can not add user to this role", "error");
                }
            });
        }
    }
}]);