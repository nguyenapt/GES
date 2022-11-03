"use strict";
GesInsideApp.controller("GesAnnouncementController", ["$scope", "$filter", "$timeout", "GesAnnouncementService", function ($scope, $filter, $timeout, GesAnnouncementService) {
    $scope.announcements = null;
    $scope.editingAnnouncement = null;

    init();

    $scope.addNewAnnouncement = function () {
        var announcement = {
            GesAnnouncementId: "00000000-0000-0000-0000-000000000000",
            Title: "",
            LinkTitle: "",
            Content: "",
            AnnouncementDate: ""
        };

        $scope.editingAnnouncement = announcement;
        $("#announcement-dialog").modal("show");
    }

    $scope.editAnnouncement = function (announcement) {
        $scope.editingAnnouncement = announcement;
    }
    $scope.saveAnnouncement = function () {
        GesAnnouncementService.SaveAnnouncement($scope.editingAnnouncement, loadAnnouncements);
    }

    $scope.deleteAnnouncement = function (target) {
        var announcementId = target.attr("data-id");
        GesAnnouncementService.DeleteAnnouncement(announcementId, loadAnnouncements);
    }

    function init() {
        loadAnnouncements();
    }


    function convertDate(value, format) {
        if (value !== null && !value.isNullOrEmpty) {
            return $filter("date")(new Date(parseInt(value.substr(6))), format);
        }
        return null;
    };



    function loadAnnouncements() {
        GesAnnouncementService.GetAnnouncements().then(
            function (d) {
                $scope.announcements = d.data;
                $timeout(function () {
                    $(".icon-remove").confirmModal({
                        confirmCallback: $scope.deleteAnnouncement
                    });
                });
                if ($scope.announcements.length > 0) {
                    for (var i = 0; i < $scope.announcements.length; i++) {

                        if (($scope.announcements[i].AnnouncementDate !== null)) {
                            $scope.announcements[i].AnnouncementDate = new Date(convertDate($scope.announcements[i].AnnouncementDate, 'yyyy-MM-dd'));
                        }
                    }
                }
            },
            function () {
                quickNotification("Error occurred during loading GES Announcement", "error");
            }
        );
    }
}]);