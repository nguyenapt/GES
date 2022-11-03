"use strict";
GesInsideApp.factory("GesContactService", ["$http", "$q", "$window", "Upload", function($http, $q, $window, Upload) {
    return {        
        SaveGesContact: function (contact, successCallback) {
            Upload.upload({
                url: "/GesContact/SaveGesContact",
                method: "POST",
                data: {
                    contact: contact
                }
            }).then(
                function (d) {
                    if (d.data.Status === "Success") {
                        quickNotification("Saved successfully");
                        successCallback();
                    } else {
                        quickNotification("Error occurred during saving GES contact", "error");
                    }
                }
            );
        },
        GetAllCountries: function () {
            return $http.get("/CaseProfile/GetCountries");
        },
        GetDialogueByIndividual: function (individualId,type) {
            return $http.get("/GesContact/GetDialogueByIndividual", { params: { individualId: individualId, type: type } });
        },
        DeleteGesContact: function (contactid, successCallback) {
            $http({
                url: "/GesContact/DeleteGesContact",
                method: "POST",
                data: {
                    contactid: contactid
                }
            }).then(
                function (d) {
                    if (d.data.Status === "Success") {
                        quickNotification("Deleted GES Contact successfully");
                        successCallback();
                    } else {
                        quickNotification("Can not delete GES contact", "error");
                    }
                }
            );
        }
    }
}]);