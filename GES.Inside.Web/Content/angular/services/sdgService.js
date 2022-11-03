"use strict";
GesInsideApp.factory("SdgService", ["$http", "$q", "$window", "Upload", function($http, $q, $window, Upload) {
    return {
        GetSdgs: function() {
            return $http.get("/Sdg/GetSdgs");
        },
        SaveSdg: function (sdg, file, successCallback) {
            Upload.upload({
                url: "/Sdg/SaveSdg",
                method: "POST",
                data: {
                    sdg: sdg,
                    file: file
                }
            }).then(
                function (d) {
                    if (d.data.Status === "Success") {
                        quickNotification("Saved successfully");
                        successCallback();
                    } else {
                        quickNotification("Error occurred during saving SDG", "error");
                    }
                }
            );
        },
        GetSdgById: function(sdgId) {
            return $http.get("/Sdg/GetSdgById", { params: { sdgId: sdgId } });
        },
        DeleteSdg: function (sdgId, successCallback) {
            $http({
                url: "/Sdg/DeleteSdg",
                method: "POST",
                data: {
                    sdgId: sdgId
                }
            }).then(
                function (d) {
                    if (d.data.Status === "Success") {
                        quickNotification("Deleted SDG successfully");
                        successCallback();
                    } else {
                        quickNotification("Can not delete SDG because it is being used in case profile", "error");    
                    }
                }
            );
        }
    }
}]);