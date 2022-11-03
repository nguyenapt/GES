"use strict";
GesInsideApp.factory("GesAnnouncementService", ["$http", "$q", "$window", "Upload", function($http, $q, $window, Upload) {
    return {
        GetAnnouncements: function() {
            return $http.get("/GesAnnouncement/GetAnnouncements");
        },
        SaveAnnouncement: function (announcement, successCallback) {
            Upload.upload({
                url: "/GesAnnouncement/SaveAnnouncement",
                method: "POST",
                data: {
                    announcement: announcement
                }
            }).then(
                function (d) {
                    if (d.data.Status === "Success") {
                        quickNotification("Saved successfully");
                        successCallback();
                    } else {
                        quickNotification("Error occurred during saving GES Announcement", "error");
                    }
                }
            );
        },
        //GetAnnouncementById: function (announcementId) {
        //    return $http.get("/GesAnnouncement/GetAnnouncementById", { params: { announcementId: announcementId } });
        //},
        DeleteAnnouncement: function (announcementId, successCallback) {
            $http({
                url: "/GesAnnouncement/DeleteAnnouncement",
                method: "POST",
                data: {
                    announcementId: announcementId
                }
            }).then(
                function (d) {
                    if (d.data.Status === "Success") {
                        quickNotification("Deleted GES Announcement successfully");
                        successCallback();
                    } else {
                        quickNotification("Can not delete GES Announcement", "error");
                    }
                }
            );
        }
    }
}]);